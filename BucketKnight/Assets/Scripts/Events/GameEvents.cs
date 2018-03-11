// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEvents.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class GameEvent
    {
    }

    #region Player stats

    public class PlayerHit : GameEvent
    {
        public int hpToLose;
        public bool hitIsDodgeable;
        public Enums.Enemy enemyType;

        public PlayerHit(int hpToLose, bool hitIsDodgeable, Enums.Enemy enemyType)
        {
            this.hpToLose = hpToLose;
            this.hitIsDodgeable = hitIsDodgeable;
            this.enemyType = enemyType;
        }
    }

    public class SuccessfulDodge : GameEvent
    {
    }

    public class PlayerDamaged : GameEvent
    {
    }

    public class PlayerHealthChanged : GameEvent
    {
        public int health;
        public int tempHealth;

        public PlayerHealthChanged(int health, int tempHealth)
        {
            this.health = health;
            this.tempHealth = tempHealth;
        }
    }

    public class CoinsPickedUp : GameEvent
    {
        public int coinsPickedUp;

        public CoinsPickedUp(int coinsPickedUp)
        {
            this.coinsPickedUp = coinsPickedUp;
        }
    }

    public class MultiplierChanged : GameEvent
    {
        public int multiplierLevel;
        public float currentTime;
        public float currentTimeLimit;

        public MultiplierChanged(int multiplierLevel, float currentTimeLimit)
        {
            this.multiplierLevel = multiplierLevel;
            this.currentTimeLimit = currentTimeLimit;
            currentTime = Time.time;
        }
    }

    public class SpeedChanged : GameEvent
    {
        public float changeInSpeed;
        public float newPlayerSpeed;

        public SpeedChanged(float changeInSpeed, float newPlayerSpeed)
        {
            this.changeInSpeed = changeInSpeed;
            this.newPlayerSpeed = newPlayerSpeed;
        }
    }

    public class TravelDistanceChanged : GameEvent
    {
        public float travelDistance;

        public TravelDistanceChanged(float travelDistance)
        {
            this.travelDistance = travelDistance;
        }
    }

    public class ScoreChanged : GameEvent
    {
        public int score;

        public ScoreChanged(int score)
        {
            this.score = score;
        }
    }

    public class InGameCoinsNumberChanged : GameEvent
    {
        public float coins;

        public InGameCoinsNumberChanged(float coins)
        {
            this.coins = coins;
        }
    }

    public class TurningSpeedChanged : GameEvent
    {
        public bool active;

        public TurningSpeedChanged(bool active)
        {
            this.active = active;
        }
    }

    public class TotalCoinsChanged : GameEvent
    {
        public int totalCoins;

        public TotalCoinsChanged(int totalCoins)
        {
            this.totalCoins = totalCoins;
        }
    }


    #endregion

    #region Powerups

    public class PowerupPickup : GameEvent
    {
        public Enums.PowerupActivationType activationType;
        public Enums.PowerupType powerup;
        public int slotNum;

        public PowerupPickup(Enums.PowerupType powerup, Enums.PowerupActivationType activationType, int slotNum = 0)
        {
            this.slotNum = slotNum;
            this.powerup = powerup;
            this.activationType = activationType;

            if (activationType == Enums.PowerupActivationType.Equipment)
            {
                if (slotNum == 1)
                {
                    Events.instance.Raise(new PowerupSlotOneEquipped(Enums.PowerupType.None));
                }
                else
                {
                    Events.instance.Raise(new PowerupSlotTwoEquipped(Enums.PowerupType.None));
                }
            }
        }
    }

    public class CoinPowerup : GameEvent
    {
        public int moneyToSpawn;

        public CoinPowerup(int moneyToSpawn)
        {
            this.moneyToSpawn = moneyToSpawn;
        }
    }

    public class PowerupUnlocked : GameEvent
    {
        public int unlockLevel;

        public PowerupUnlocked(int unlockLevel)
        {
            this.unlockLevel = unlockLevel;
        }
    }

    public class PowerupSlotOneEquipped : GameEvent
    {
        public Enums.PowerupType powerupType;

        public PowerupSlotOneEquipped(Enums.PowerupType powerupType)
        {
            this.powerupType = powerupType;
        }
    }

    public class PowerupSlotTwoEquipped : GameEvent
    {
        public Enums.PowerupType powerupType;

        public PowerupSlotTwoEquipped(Enums.PowerupType powerupType)
        {
            this.powerupType = powerupType;
        }
    }

    public class SellEquipmentOne : GameEvent
    {
    }

    public class SellEquipmentTwo : GameEvent
    {
    }

    public class PowerupEnded : GameEvent
    {
        public bool endedSoAnotherCouldBegin;
        public Enums.PowerupType powerupType;

        public PowerupEnded(Enums.PowerupType powerupType = Enums.PowerupType.None,
            bool endedSoAnotherCouldBegin = false)
        {
            this.endedSoAnotherCouldBegin = endedSoAnotherCouldBegin;
            this.powerupType = powerupType;
        }
    }

    #endregion

    #region Environment

    public class GroundRemoved : GameEvent
    {
    }

    public class WaterRemoved : GameEvent
    {
    }

    #endregion

    #region SpawnRate

    public class HazardSpawnIncrease : GameEvent
    {
        public int spawnsPerMinute;

        public HazardSpawnIncrease(int spawnsPerMinute)
        {
            this.spawnsPerMinute = spawnsPerMinute;
        }
    }

    public class HazardSpawnDecrease : GameEvent
    {
        public int spawnsPerNimute;

        public HazardSpawnDecrease(int spawnsPerNimute)
        {
            this.spawnsPerNimute = spawnsPerNimute;
        }
    }

    public class MoneySpawnIncrease : GameEvent
    {
        public int spawnsPerNimute;

        public MoneySpawnIncrease(int spawnsPerNimute)
        {
            this.spawnsPerNimute = spawnsPerNimute;
        }
    }

    public class MoneySpawnDecrease : GameEvent
    {
        public int spawnsPerNimute;

        public MoneySpawnDecrease(int spawnsPerNimute)
        {
            this.spawnsPerNimute = spawnsPerNimute;
        }
    }

    #endregion

    #region Game states

    public class ShowLeaderboards : GameEvent
    {
    }

    public class ShowAchievements : GameEvent
    {
    }

    public class GoInGame : GameEvent
    {
    }

    public class GamePaused : GameEvent
    {
    }

    public class GameResumed : GameEvent
    {
    }

    public class GameRestarted : GameEvent
    {
        public GameRestarted()
        {
            Events.instance.Raise(new GameResumed());
            Events.instance.Raise(new PowerupEnded());
        }
    }

    public class PlayerLost : GameEvent
    {
        public int finalScore;
        public int collectedCoins;
        public Enums.Enemy killedBy;

        public PlayerLost(int finalScore, int collectedCoins, Enums.Enemy killedBy)
        {
            this.collectedCoins = collectedCoins;
            this.finalScore = finalScore;
            this.killedBy = killedBy;
            Events.instance.Raise(new GamePaused());
        }
    }

    public class ShowInfoBox : GameEvent
    {
        public string infoText;

        public ShowInfoBox(string infoText)
        {
            this.infoText = infoText;
        }
    }

    public class ShowWarningBox : GameEvent
    {
        public string infoText;

        public ShowWarningBox(string infoText)
        {
            this.infoText = infoText;
        }
    }

    public class ConfirmationBoxAnswered : GameEvent
    {
        public bool answeredPositive;

        public ConfirmationBoxAnswered(bool answeredPositive)
        {
            this.answeredPositive = answeredPositive;
        }
    }

    public class EnterSettingsMenu : GameEvent
    {
    }

    public class ExitSettingsMenu : GameEvent
    {
    }

    public class EnterShopMenu : GameEvent
    {
    }

    public class ExitShopMenu : GameEvent
    {
    }

    public class EnterCreditsMenu : GameEvent
    {
    }

    public class ExitCreditsMenu : GameEvent
    {
    }

    public class EnterGooglePlayMenu : GameEvent
    {
    }

    public class ExitGooglePlayMenu : GameEvent
    {
    }

    public class EnterTutorialMenu : GameEvent
    {
        public bool openedFromSettings;

        public EnterTutorialMenu(bool openedFromSettings)
        {
            this.openedFromSettings = openedFromSettings;
        }
    }

    public class ExitTutorialMenu : GameEvent
    {
        public bool exitToPlay;

        public ExitTutorialMenu(bool exitToPlay)
        {
            this.exitToPlay = exitToPlay;
        }

        public ExitTutorialMenu() : this(false)
        {
        }
    }

    public class EnterPauseMenu : GameEvent
    {
    }

    public class QuitToMainMenu : GameEvent
    {
    }

    public class ExitGame : GameEvent
    {
    }

    #endregion

    #region Settings

    public class SoundToggled : GameEvent
    {
        public bool toggledOn;

        public SoundToggled(bool toggledOn)
        {
            this.toggledOn = toggledOn;
        }
    }

    public class TiltToggled : GameEvent
    {
        public bool useTilt;

        public TiltToggled(bool useTilt)
        {
            this.useTilt = useTilt;
        }
    }

    public class TutorialToggled : GameEvent
    {
        public bool playTutorial;

        public TutorialToggled(bool playTutorial)
        {
            this.playTutorial = playTutorial;
        }
    }

    #endregion

    #region Google Play

    public class LogoutGooglePlay : GameEvent
    {
    }

    public class LoginGooglePlay : GameEvent
    {
    }

    #endregion
}
