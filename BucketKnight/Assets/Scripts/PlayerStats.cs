// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerStats.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using CloudOnce;
    using UnityEngine;

    [Serializable]
    public struct MultiplierData
    {
        public float timeLimit;
        public int coinsUntilNext;
    }

    public class PlayerStats : MonoBehaviour
    {
        public const float StartingSpeed = 15;
        public int BaseCoinValue;

        private float _scoreNumber;
        private float _scoremultiplier = 0.2f;

        // Powerup objekter
        public GameObject bubbleShield;
        public GameObject boat;
        public GameObject paddle;
        public Material origMaterial;
        public Material boatPowerupMat;
        public Material paddlePowerupMat;

        // speedIncrementRelated
        public float speedIncreasePerLevel = 1.2f;
        private int _level = 1;
        private const int c_baseScoreIncrementPerLevel = 1000;
        private int _nextLevelScoreRequirement = c_baseScoreIncrementPerLevel;

        public PlayerAnimController playerAnim;
        public AudioManagerScript audioManager;

        private int _currentMultiplier;

        public int CurrentMultiplier
        {
            get { return _currentMultiplier; }
            set
            {
                if (value > multiplierData.Length)
                {
                    value = multiplierData.Length;
                }

                _currentMultiplier = value;
                Events.instance.Raise(new MultiplierChanged(CurrentMultiplier, multiplierData[CurrentMultiplier - 1].timeLimit));
            }
        }

        public MultiplierData[] multiplierData;
        public float multiplierTimer;
        public bool freezeMultiplierTimer;
        public int coinCountThisMultiplier;

        // Dodging
        private bool _isDodging;

        public bool IsInvincible;

        // PowerupVariables
        private List<Enums.Enemy> _powerupInvincibilityGrantetAgainst = new List<Enums.Enemy>();
        private Enums.PowerupType _activePowerup;
        private int _canTakeHit;

        /*ACHIEVEMENTS*/

        // Bucket Chivalry
        private bool _powerupActivatedThisRun;

        // Questing King
        private float _questingKnightAchievementTimer;

        // Unstoppable Rafter
        private int _numberOfTimesHurtThisRun;

        private int tempHealth;

        public int TempHealth
        {
            get { return tempHealth; }
            set
            {
                var tH = value;
                if (tH > PlayerHealth)
                {
                    tH = _playerHealth;
                }

                tempHealth = tH;
                Events.instance.Raise(new PlayerHealthChanged(PlayerHealth, tempHealth));
            }
        }

        public int defaultHealth;
        private int _playerHealth;

        public int PlayerHealth
        {
            get { return _playerHealth; }
            set
            {
                var h = value;
                if (h > defaultHealth)
                {
                    h = defaultHealth;
                }
                _playerHealth = h;
                Events.instance.Raise(new PlayerHealthChanged(_playerHealth, TempHealth));
            }
        }

        private float _speed;

        public float Speed
        {
            get { return _speed; }
            set
            {
                Events.instance.Raise(new SpeedChanged(value - _speed, value));
                _speed = value;
            }
        }

        private int _score;

        public int Score
        {
            get { return _score + (int)TravelDistance; }
            private set
            {
                _score = value;
                Events.instance.Raise(new ScoreChanged(Score));
            }
        }

        private float _travelDistance;

        public float TravelDistance
        {
            get { return _travelDistance; }
            set
            {
                _travelDistance = value;
                Events.instance.Raise(new TravelDistanceChanged(TravelDistance));
                Events.instance.Raise(new ScoreChanged(Score));
                if (_score + _travelDistance >= _nextLevelScoreRequirement)
                {
                    if (_nextLevelScoreRequirement < 9000)
                    {
                        Speed += speedIncreasePerLevel;
                    }
                    else
                    {
                        Events.instance.Raise(new HazardSpawnIncrease(5));
                    }

                    Events.instance.Raise(new MoneySpawnIncrease(2));
                    _nextLevelScoreRequirement = c_baseScoreIncrementPerLevel + (_level * c_baseScoreIncrementPerLevel);
                    _level++;
                    _scoreNumber = _speed * (_scoremultiplier + (_level / 5f));
                }
            }
        }

        private int _coins;

        public int Coins
        {
            get { return _coins; }
            set
            {
                _coins = value;
                Events.instance.Raise(new InGameCoinsNumberChanged(Coins));
            }
        }

        private void Awake()
        {
            Speed = StartingSpeed;
        }

        private void OnEnable()
        {
            Events.instance.AddListener<PlayerHit>(OnPlayerHit);
            Events.instance.AddListener<CoinsPickedUp>(OnCoinsPickedUp);
            Events.instance.AddListener<PowerupPickup>(OnPowerupPickup);
            Events.instance.AddListener<PowerupEnded>(OnPowerupEnded);
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
        }

        // Use this for initialization
        private void Start()
        {
            Restart();
        }

        public void Restart()
        {
            _level = 1;
            _scoreNumber = _speed * (_scoremultiplier + (_level * 0.1f));
            _nextLevelScoreRequirement = c_baseScoreIncrementPerLevel;

            PlayerHealth = defaultHealth;
            TempHealth = 0;
            TravelDistance = 0;
            Coins = 0;
            Score = 0;
            Speed = StartingSpeed;
            CurrentMultiplier = 1;
            multiplierTimer = multiplierData[CurrentMultiplier - 1].timeLimit;
            coinCountThisMultiplier = 0;
            freezeMultiplierTimer = false;
            _powerupActivatedThisRun = false;
            _activePowerup = Enums.PowerupType.None;
            SetOverWater();
            _questingKnightAchievementTimer = 0;
            _numberOfTimesHurtThisRun = 0;
        }

        private void Update()
        {
            TravelDistance += Time.deltaTime * _scoreNumber;

            // count down multiplier if it is not freezed and you are not at x1 with 0 coins
            if (!freezeMultiplierTimer && !(CurrentMultiplier == 1 && coinCountThisMultiplier == 0))
            {
                multiplierTimer -= Time.deltaTime;
            }

            if (multiplierTimer < 0)
            {
                DecreaseMultiplier();
            }

            if (CurrentMultiplier == multiplierData.Length &&
                _questingKnightAchievementTimer < SavedData.QuestingKingTimeToGet)
            {
                _questingKnightAchievementTimer += Time.deltaTime;
                if (_questingKnightAchievementTimer >= SavedData.QuestingKingTimeToGet)
                {
                    Achievements.QuestingKnight.Unlock();
                }
            }
        }

        private IEnumerator PowerupCountdown(float duration)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Events.instance.Raise(new PowerupEnded(_activePowerup));
        }

        public void AddToScore(int pointsToAdd)
        {
            Score = _score + pointsToAdd;
        }

        public void SetUnderWater()
        {
            audioManager.SetUnderwater();
            _isDodging = true;
        }

        public void SetOverWater()
        {
            audioManager.SetOverWater();
            _isDodging = false;
        }

        private void DecreaseMultiplier()
        {
            if (CurrentMultiplier > 1)
            {
                CurrentMultiplier--;
            }

            multiplierTimer = multiplierData[CurrentMultiplier - 1].timeLimit;
            coinCountThisMultiplier = 0;
            _questingKnightAchievementTimer = 0;
        }

        private void IncreaseMultiplier(int increaseBy)
        {
            if (CurrentMultiplier < multiplierData.Length)
            {

                CurrentMultiplier += increaseBy;
            }

            coinCountThisMultiplier = 0;
            multiplierTimer = multiplierData[CurrentMultiplier - 1].timeLimit;
        }

        private void BubblePowerup()
        {
            _powerupInvincibilityGrantetAgainst.Clear();

            _powerupInvincibilityGrantetAgainst.Add(Enums.Enemy.Fish);
            _powerupInvincibilityGrantetAgainst.Add(Enums.Enemy.Log);
            _powerupInvincibilityGrantetAgainst.Add(Enums.Enemy.Beaver);
            _powerupInvincibilityGrantetAgainst.Add(Enums.Enemy.Bridge);
            bubbleShield.SetActive(true);
        }

        private void GameOver(Enums.Enemy killedBy)
        {
            TempHealth = 0;
            PlayerHealth = 0;
            SavedData.TotalCoins += Coins;
            if (!_powerupActivatedThisRun && Score >= SavedData.BucketChivalryScoreToGet)
            {
                Achievements.BucketChivalry.Unlock();
            }

            Events.instance.Raise(new PlayerLost(Score, _coins, killedBy));
        }

        #region Event handlers

        private void OnPlayerHit(PlayerHit playerHitEvent)
        {
            /* FIRST WE CHECK FOR CONDITIONS THAT MIGHT SPARE THE PLAYER FROM GETTING HURT */

            // if player is invincible
            if (IsInvincible)
            {
                return;
            }

            // if spinning & hit by dodgeable hazard
            if (_isDodging && playerHitEvent.hitIsDodgeable)
            {
                Events.instance.Raise(new SuccessfulDodge());
                return;
            }

            // If player has powerup that grant invincibility against the enemy
            if (_powerupInvincibilityGrantetAgainst.Count != 0)
            {
                foreach (var enemy in _powerupInvincibilityGrantetAgainst)
                {
                    if (playerHitEvent.enemyType == enemy)
                    {
                        return;
                    }
                }
            }

            // if player has invincibility-powerup
            if (_canTakeHit > 0)
            {
                _canTakeHit -= 1;
                if (_canTakeHit == 0 && _activePowerup != Enums.PowerupType.None)
                {
                    Events.instance.Raise(new PowerupEnded(Enums.PowerupType.Sword));
                }

                return;
            }

            /* THE PLAYER IS ACTUALLY HURT IF YOU GET TO THIS POINT! */

            playerAnim.PlayerDamaged();
            Events.instance.Raise(new PlayerDamaged());
            GetComponent<CollisionSound>().MakeSoundWithPitch();

            DecreaseMultiplier();

            _numberOfTimesHurtThisRun++;
            if (_numberOfTimesHurtThisRun == SavedData.UnstoppableRafterHitsToGet)
            {
                Cloud.Achievements.UnlockAchievement(CloudIDs.AchievementIDs.UnstoppableRafter);
            }

            // if we hit a One-Hit-KO-hazard
            if (playerHitEvent.hpToLose == -1)
            {
                if (TempHealth > 0)
                {
                    TempHealth = 0;
                }
                else
                {
                    GameOver(playerHitEvent.enemyType);
                }

                ScreenShakeOnPlayerLost.ShakeCamera();
                return;
            }

            var hpLost = playerHitEvent.hpToLose;

            // if we have one or more wooden hearts
            if (TempHealth > 0)
            {
                if (TempHealth >= hpLost)
                {
                    TempHealth -= hpLost;
                    return;
                }

                hpLost -= TempHealth;
                TempHealth = 0;
            }

            PlayerHealth -= hpLost;
            ScreenShakeOnPlayerLost.ShakeCamera();

            if (PlayerHealth <= 0)
            {
                GameOver(playerHitEvent.enemyType);
            }
        }

        private void OnCoinsPickedUp(CoinsPickedUp coinsPickedUpEvent)
        {
            Coins += coinsPickedUpEvent.coinsPickedUp;
            AddToScore(BaseCoinValue * coinsPickedUpEvent.coinsPickedUp * _currentMultiplier);
            coinCountThisMultiplier++;
            if (coinCountThisMultiplier >= multiplierData[CurrentMultiplier - 1].coinsUntilNext)
            {
                IncreaseMultiplier(1);
            }
            else
            {
                multiplierTimer = multiplierData[CurrentMultiplier - 1].timeLimit;
            }
        }

        private void OnPowerupPickup(PowerupPickup onPowerupPickupEvent)
        {
            _powerupActivatedThisRun = true;

            if (_activePowerup != Enums.PowerupType.None)
            {
                Events.instance.Raise(new PowerupEnded(_activePowerup, true));
            }

            var poww = PowerupManager.GetPowerup(onPowerupPickupEvent.powerup);
            _activePowerup = poww.powerupType;
            if (poww.Duration > 0)
            {
                StartCoroutine("PowerupCountdown", (float)poww.Duration);
            }

            switch (_activePowerup)
            {
                case Enums.PowerupType.Bubble:
                    BubblePowerup();
                    break;

                case Enums.PowerupType.FillHp:
                    PlayerHealth += 1;
                    break;

                case Enums.PowerupType.Grail:
                    IncreaseMultiplier(4);
                    freezeMultiplierTimer = true;
                    break;

                case Enums.PowerupType.Sword:
                    boat.GetComponent<Renderer>().material = boatPowerupMat;
                    _canTakeHit = 1;
                    break;
                case Enums.PowerupType.TemporaryHp:
                    TempHealth += 1;
                    break;

                case Enums.PowerupType.Windmill:
                    paddle.GetComponent<Renderer>().material = paddlePowerupMat;
                    Events.instance.Raise(new TurningSpeedChanged(true));
                    break;

                case Enums.PowerupType.MoneyBag:
                    Events.instance.Raise(new CoinPowerup(10));
                    break;
            }
        }

        private void OnPowerupEnded(PowerupEnded powerupEndedEvent)
        {
            StopCoroutine("PowerupCountdown");

            switch (_activePowerup)
            {
                case Enums.PowerupType.Bubble:
                    bubbleShield.SetActive(false);
                    _powerupInvincibilityGrantetAgainst.Clear();
                    break;

                case Enums.PowerupType.FillHp:
                    break;

                case Enums.PowerupType.Grail:
                    freezeMultiplierTimer = false;
                    break;

                case Enums.PowerupType.Sword:
                    boat.GetComponent<Renderer>().material = origMaterial;
                    _canTakeHit = 0;
                    break;
                case Enums.PowerupType.TemporaryHp:
                    break;

                case Enums.PowerupType.Windmill:
                    paddle.GetComponent<Renderer>().material = origMaterial;
                    Events.instance.Raise(new TurningSpeedChanged(false));
                    break;

                case Enums.PowerupType.MoneyBag:
                    Events.instance.Raise(new CoinPowerup(0));
                    break;
            }

            _activePowerup = Enums.PowerupType.None;
        }

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            Restart();
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<PlayerHit>(OnPlayerHit);
            Events.instance.RemoveListener<CoinsPickedUp>(OnCoinsPickedUp);
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
            Events.instance.RemoveListener<PowerupEnded>(OnPowerupEnded);
            Events.instance.RemoveListener<PowerupPickup>(OnPowerupPickup);
        }
    }
}
