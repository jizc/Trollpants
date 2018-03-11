// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SavedData.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using CloudOnce;
    using CloudOnce.CloudPrefs;
    using UnityEngine;

    public static class SavedData
    {
        /* ACHIEVEMENTS */

        // Getting Hearted
        public const int GettingHeartedUnlocksToGet = 2;

        // Armor Up
        public const int BubbleTroubleUnlocksToGet = 5;

        // Kamelot
        public const int KamelotUnlocksToGet = 8;

        // Squire
        public const int SquireScoreToGet = 5000;

        // Knight-errant
        public const int KnightErrantScoreToGet = 10000;

        // Paladin
        public const int PaladinScoreToGet = 15000;

        // Bucket Chivalry
        public const int BucketChivalryScoreToGet = 5000;

        // Questing King
        public const float QuestingKingTimeToGet = 45;

        // Unstoppable Rafter
        public const int UnstoppableRafterHitsToGet = 8;

        // Treasure Hunter
        public const int TreasureHunterTotalCoinsToGet = 30000;

        private const int maxCoinsAllowed = 99999;

        public static bool GameIsPaused { get; set; }

        public static Enums.PowerupType PowerupInSlotOne
        {
            get
            {
                return (Enums.PowerupType)CloudVariables.PowerUpSlotOne;
            }
            private set
            {
                var powerupNum = (int)value;
                if (powerupNum > Enum.GetNames(typeof(Enums.PowerupType)).Length - 1)
                {
                    powerupNum = 0;
                }

                CloudVariables.PowerUpSlotOne = powerupNum;
            }
        }

        public static Enums.PowerupType PowerupInSlotTwo
        {
            get
            {
                return (Enums.PowerupType)CloudVariables.PowerUpSlotTwo;
            }
            private set
            {
                var powerupNum = (int)value;
                if (powerupNum > Enum.GetNames(typeof(Enums.PowerupType)).Length - 1)
                {
                    powerupNum = 0;
                }

                CloudVariables.PowerUpSlotTwo = powerupNum;
            }
        }

        public static bool TutorialHasBeenLaunchedOnce
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("tutorialHasBeenLaunchedOnce", 0)); }
            set { PlayerPrefs.SetInt("tutorialHasBeenLaunchedOnce", Convert.ToInt32(value)); }
        }

        public static float SecondsPlayedSinceLastAd
        {
            get { return PlayerPrefs.GetFloat("secondsPlayedSinceLastAd", 0f); }
            set { PlayerPrefs.SetFloat("secondsPlayedSinceLastAd", value); }
        }

        public static bool UnlocksHaveBeenExplained
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("unlocksHaveBeenExplained", 0)); }
            set { PlayerPrefs.SetInt("unlocksHaveBeenExplained", Convert.ToInt32(value)); }
        }

        public static bool EquipmentHaveBeenExplained
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("equipmentHaveBeenExplained", 0)); }
            set { PlayerPrefs.SetInt("equipmentHaveBeenExplained", Convert.ToInt32(value)); }
        }

        public static int HighScore
        {
            get
            {
                return CloudVariables.HighScore;
            }
            set
            {
                if (value >= SquireScoreToGet)
                {
                    Achievements.Squire.Unlock();
                }

                if (value >= KnightErrantScoreToGet)
                {
                    Achievements.KnightErrant.Unlock();
                }

                if (value >= PaladinScoreToGet)
                {
                    Achievements.Paladin.Unlock();
                }

                Leaderboards.HighScores.SubmitScore(value);

                CloudVariables.HighScore = value;
            }
        }

        public static int TotalCoinsAllTime
        {
            get
            {
                return CloudVariables.TotalCoinsAllTime;
            }

            private set
            {
                var totalCoinsAllTime = value;
                if (value > maxCoinsAllowed)
                {
                    totalCoinsAllTime = maxCoinsAllowed;
                }

                CloudVariables.TotalCoinsAllTime = totalCoinsAllTime;

                Achievements.TreasureHunter.Increment(totalCoinsAllTime, TreasureHunterTotalCoinsToGet);
            }
        }

        public static int TotalCoins
        {
            get
            {
                return CloudVariables.TotalCoins;
            }

            set
            {
                // if coins are added, we must update TotalCoinsAllTime
                if (value - TotalCoins > 0)
                {
                    TotalCoinsAllTime += value - TotalCoins;
                }

                var totalCoins = value;
                if (totalCoins < 0)
                {
                    totalCoins = 0;
                }
                else if (totalCoins > maxCoinsAllowed)
                {
                    totalCoins = maxCoinsAllowed;
                }

                CloudVariables.TotalCoins = totalCoins;
                Events.instance.Raise(new TotalCoinsChanged(TotalCoins));
            }
        }

        public static bool UseTilt
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("tiltToggledOn", 0)); }
            private set { PlayerPrefs.SetInt("tiltToggledOn", Convert.ToInt32(value)); }
        }

        public static bool PlayTutorial
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("playTutorial", 1)); }
            private set { PlayerPrefs.SetInt("playTutorial", Convert.ToInt32(value)); }
        }

        public static bool SoundToggledOn
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("soundToggledOn", 1)); }
            private set { PlayerPrefs.SetInt("soundToggledOn", Convert.ToInt32(value)); }
        }

        public static void ForceConstructor()
        {
        }

        static SavedData()
        {
            Events.instance.AddListener<GamePaused>(OnGamePaused);
            Events.instance.AddListener<GameResumed>(OnGameResumed);
            Events.instance.AddListener<SoundToggled>(OnSoundToggled);
            Events.instance.AddListener<TiltToggled>(OnTiltToggled);
            Events.instance.AddListener<TutorialToggled>(OnTutorialToggled);
            Events.instance.AddListener<PowerupUnlocked>(OnPowerupUnlocked);
            Events.instance.AddListener<PowerupSlotOneEquipped>(OnPowerupSlotOneEquipped);
            Events.instance.AddListener<PowerupSlotTwoEquipped>(OnPowerupSlotTwoEquipped);
        }

        #region Event handlers

        private static void OnGamePaused(GamePaused gamePausedEvent)
        {
            GameIsPaused = true;
        }

        private static void OnGameResumed(GameResumed gameResumedEvent)
        {
            GameIsPaused = false;
        }

        private static void OnSoundToggled(SoundToggled soundToggledEvent)
        {
            SoundToggledOn = soundToggledEvent.toggledOn;
        }

        private static void OnTiltToggled(TiltToggled tiltToggledEvent)
        {
            UseTilt = tiltToggledEvent.useTilt;
        }

        private static void OnTutorialToggled(TutorialToggled tutorialToggledEvent)
        {
            PlayTutorial = tutorialToggledEvent.playTutorial;
        }

        private static void OnPowerupUnlocked(PowerupUnlocked powerupUnlockedEvent)
        {
            CloudVariables.NumberOfPowerupsUnlocked = powerupUnlockedEvent.unlockLevel;
        }

        private static void OnPowerupSlotOneEquipped(PowerupSlotOneEquipped powerupSlotOneEquippedEvent)
        {
            PowerupInSlotOne = powerupSlotOneEquippedEvent.powerupType;
        }

        private static void OnPowerupSlotTwoEquipped(PowerupSlotTwoEquipped powerupSlotTwoEquippedEvent)
        {
            PowerupInSlotTwo = powerupSlotTwoEquippedEvent.powerupType;
        }

        #endregion
    }
}
