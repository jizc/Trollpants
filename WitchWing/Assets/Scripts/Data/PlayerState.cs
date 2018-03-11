// <copyright file="PlayerState.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Data
{
    using System;
    using System.Diagnostics;
    using CloudOnce;
    using CloudOnce.CloudPrefs;
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerState : MonoBehaviour
    {
        private readonly int fiveHoursInSeconds = (int)TimeSpan.FromHours(5.0).TotalSeconds;
        private readonly Stopwatch stopwatch = new Stopwatch();

        [Header("Unlocked Levels")]
        [SerializeField] [Range(1, UpgradeInfo.MaxUpgradeLevel)] private int manaAmountLevel = 1;
        [SerializeField] [Range(1, UpgradeInfo.MaxUpgradeLevel)] private int verticalSpeedLevel = 1;
        [SerializeField] [Range(1, UpgradeInfo.MaxUpgradeLevel)] private int boostSpeedLevel = 1;
        [SerializeField] [Range(1, UpgradeInfo.MaxUpgradeLevel)] private int manaRegenLevel = 1;

        [Header("Potions")]
        [SerializeField] private bool isWisdomActive;
        [SerializeField] private bool isMidasActive;

        [Header("Session Data")]
        [SerializeField] private int distanceTraveled;
        [SerializeField] private int coinsCollected;
        [SerializeField] private int timeBonus;

        private CloudCurrencyInt coins;
        private float playerStartPosition;

        public event UnityAction<int> CoinsChanged;
        public event UnityAction<int> TimeBonusChanged;
        public event UnityAction<int> DistanceRecordChanged;
        public event UnityAction<int> DistanceTraveledChanged;
        public event UnityAction<int> WisdomPotionsChanged;
        public event UnityAction<int> MidasPotionsChanged;

        public float ManaAmount
        {
            get { return UpgradeInfo.GetUpgradeValue(Upgrade.ManaAmount, manaAmountLevel); }
        }

        public float VerticalSpeed
        {
            get { return UpgradeInfo.GetUpgradeValue(Upgrade.VerticalSpeed, verticalSpeedLevel); }
        }

        public float BoostSpeed
        {
            get { return UpgradeInfo.GetUpgradeValue(Upgrade.BoostSpeed, boostSpeedLevel); }
        }

        public float ManaRegenRate
        {
            get { return UpgradeInfo.GetUpgradeValue(Upgrade.ManaRegenRate, manaRegenLevel); }
        }

        public bool IsWisdomActive
        {
            get { return isWisdomActive; }
            set { isWisdomActive = value; }
        }

        public bool IsMidasActive
        {
            get { return isMidasActive; }
            set { isMidasActive = value; }
        }

        /// <summary>
        /// Distance traveled in the current flight.
        /// </summary>
        public int DistanceTraveled
        {
            get { return distanceTraveled; }
            private set { SetProperty(ref distanceTraveled, value, DistanceTraveledChanged); }
        }

        /// <summary>
        /// Coins collected in the current flight.
        /// </summary>
        public int CoinsCollected
        {
            get { return coinsCollected; }
            set { coinsCollected = value; }
        }

        public int Coins
        {
            get
            {
                return coins.Value;
            }

            set
            {
                if (coins.Value == value)
                {
                    return;
                }

                coins.Value = value;
                CoinsChanged.SafeInvoke(value);
            }
        }

        public int WisdomPotions
        {
            get
            {
                return CloudVariables.WisdomPotions;
            }

            set
            {
                if (CloudVariables.WisdomPotions == 0)
                {
                    return;
                }

                CloudVariables.WisdomPotions = value;
                WisdomPotionsChanged.SafeInvoke(value);
            }
        }

        public int MidasPotions
        {
            get
            {
                return CloudVariables.MidasPotions;
            }

            set
            {
                if (CloudVariables.MidasPotions == 0)
                {
                    return;
                }

                CloudVariables.MidasPotions = value;
                MidasPotionsChanged.SafeInvoke(value);
            }
        }

        public int TimeBonus
        {
            get { return timeBonus; }
            set { SetProperty(ref timeBonus, value, TimeBonusChanged); }
        }

        public bool IsDead { get; set; }

        public int RewardTimeBonus()
        {
            if (timeBonus > 0f)
            {
                var returnAmount = timeBonus;
                Coins += timeBonus;
                timeBonus = 0;
                return returnAmount;
            }

            return 0;
        }

        public void SaveAndReset()
        {
            Leaderboards.DistanceRecords.SubmitScore(DistanceTraveled);
            Leaderboards.CoinsCollectedInOneFlight.SubmitScore(CoinsCollected);
            Leaderboards.BiggestCoinEarners.SubmitScore(coins.Additions);

            Achievements.OffToAFlyingStart.Increment(DistanceTraveled, 100.0);
            Achievements.GetYourWings.Increment(DistanceTraveled, 250.0);
            Achievements.FlyingHigh.Increment(DistanceTraveled, 500.0);
            Achievements.WithFlyingColors.Increment(DistanceTraveled, 1000.0);
            Achievements.BroomAce.Increment(DistanceTraveled, 2000.0);

            CloudVariables.TotalSecondsPlayed += (int)stopwatch.Elapsed.TotalSeconds;
            CloudVariables.TotalDistanceTraveled += DistanceTraveled;

            Achievements.VeteranWitch.Increment(CloudVariables.TotalSecondsPlayed, fiveHoursInSeconds);
            Achievements.FrequentFlyer.Increment(CloudVariables.TotalDistanceTraveled, 20000.0);

            DistanceTraveled = 0;
            CoinsCollected = 0;
            TimeBonus = 0;
            stopwatch.Reset();

            Cloud.Storage.Save();
        }

        public void Refresh()
        {
            manaAmountLevel = CloudVariables.UnlockedManaAmountLevel;
            verticalSpeedLevel = CloudVariables.UnlockedVerticalSpeedLevel;
            boostSpeedLevel = CloudVariables.UnlockedBoostSpeedLevel;
            manaRegenLevel = CloudVariables.UnlockedManaRegenLevel;
        }

        private static void SetProperty<T>(ref T property, T value, UnityAction<T> onChanged)
        {
            if (Equals(property, value))
            {
                return;
            }

            property = value;
            onChanged.SafeInvoke(value);
        }

        private void Awake()
        {
            coins = new CloudCurrencyInt("Coins");
            playerStartPosition = transform.position.x;
            GameState.IsPausedChanged += OnIsPausedChanged;
        }

        private void OnIsPausedChanged(bool isPaused)
        {
            if (isPaused)
            {
                stopwatch.Stop();
            }
            else
            {
                stopwatch.Start();
            }
        }

        private void Start()
        {
            Refresh();
        }

        private void LateUpdate()
        {
            if (IsDead)
            {
                return;
            }

            DistanceTraveled = Mathf.RoundToInt(transform.position.x - playerStartPosition);
            if (DistanceTraveled <= CloudVariables.DistanceRecord)
            {
                return;
            }

            CloudVariables.DistanceRecord = DistanceTraveled;
            DistanceRecordChanged.SafeInvoke(DistanceTraveled);
        }
    }
}
