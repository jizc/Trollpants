// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerupManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using System.Linq;
    using CloudOnce;
    using UnityEngine;

    public static class PowerupManager
    {
        public static PowerupContainer powerupContainer;
        private static Enums.PowerupType s_powerupType;
        private static List<Enums.PowerupType> s_spawnablePowerupTypes;

        static PowerupManager()
        {
            powerupContainer = Load();
            s_spawnablePowerupTypes =
                powerupContainer.Powerups.Where(powerup => powerup.spawnAsPowerup)
                    .OrderBy(powerup => powerup.level)
                    .Select(powerup => powerup.powerupType)
                    .ToList();
        }

        private static PowerupContainer Load()
        {
            var ironCoat = new Powerup
            {
                Name = "Iron Coat",
                powerupType = Enums.PowerupType.Sword,
                Duration = 10,
                PowerupText = "Give your boat a protective sheen that blocks one hit from anything!",
                imageName = "BoatIcon",
                countDownImageName = "BoatIconRound",
                unlockPrice = 50,
                equipPrice = 50,
                spawnAsPowerup = true,
                level = 1
            };
            var heart = new Powerup
            {
                Name = "Heart",
                powerupType = Enums.PowerupType.FillHp,
                Duration = 0,
                PowerupText = "When life gets you down, a heart can get you back up! Restores one lost heart.",
                imageName = "HjerteIcon",
                countDownImageName = "HjerteIconRound",
                unlockPrice = 300,
                equipPrice = 0,
                spawnAsPowerup = true,
                level = 2
            };
            var powerPaddle = new Powerup
            {
                Name = "Power Paddle",
                powerupType = Enums.PowerupType.Windmill,
                Duration = 10,
                PowerupText = "Unleash the power of your inner paddle to give yourself a boost of speed!",
                imageName = "OarIcon",
                countDownImageName = "OarIconRound",
                unlockPrice = 900,
                equipPrice = 50,
                spawnAsPowerup = true,
                level = 3
            };
            var moneyBag = new Powerup
            {
                Name = "Bag O Money",
                powerupType = Enums.PowerupType.MoneyBag,
                Duration = 5,
                PowerupText = "Break this baby open to scatter a trail of coins into the river.",
                imageName = "PengesekkIcon",
                countDownImageName = "PengesekkIconRound",
                unlockPrice = 1800,
                equipPrice = 0,
                spawnAsPowerup = true,
                level = 4
            };
            var bubble = new Powerup
            {
                Name = "Magic Bubble",
                powerupType = Enums.PowerupType.Bubble,
                Duration = 10,
                PowerupText = "A magical bubble that protects you from everything except for rocks!",
                imageName = "BobleIcon",
                countDownImageName = "BobleIconRound",
                unlockPrice = 800,
                equipPrice = 450,
                spawnAsPowerup = true,
                level = 5
            };
            var heartShield = new Powerup
            {
                Name = "Heart Shield",
                powerupType = Enums.PowerupType.TemporaryHp,
                Duration = 0,
                PowerupText = "A wooden shield that protects one of your hearts from a hit!",
                imageName = "TrehjerteIcon",
                countDownImageName = "TrehjerteIconRound",
                unlockPrice = 3600,
                equipPrice = 0,
                spawnAsPowerup = true,
                level = 6
            };
            var gem = new Powerup
            {
                Name = "Gem",
                powerupType = Enums.PowerupType.Diamond,
                Duration = 5,
                PowerupText = "A valuable gem that's worth a handful of coins.",
                imageName = "PengesekkIcon",
                countDownImageName = "PengesekkIconRound",
                unlockPrice = 4500,
                equipPrice = 0,
                spawnAsPowerup = false,
                level = 7
            };
            var grail = new Powerup
            {
                Name = "Grail",
                powerupType = Enums.PowerupType.Grail,
                Duration = 5,
                PowerupText = "The ultimate knight accessory. Maxes your combo and protects it for 5 seconds.",
                imageName = "GralIcon",
                countDownImageName = "GralIconRound",
                unlockPrice = 9000,
                equipPrice = 600,
                spawnAsPowerup = true,
                level = 8
            };

            return new PowerupContainer
            {
                Powerups = new List<Powerup>
                {
                    ironCoat,
                    heart,
                    powerPaddle,
                    moneyBag,
                    bubble,
                    heartShield,
                    gem,
                    grail
                }
            };
        }

        public static Powerup GetRandomPowerup()
        {
            return GetPowerup(GetRandomPowerupType());
        }

        public static Powerup GetRandomPowerupAtCurrentLevel()
        {
            return GetRandomPowerupAtLevel(CloudVariables.NumberOfPowerupsUnlocked);
        }

        public static Powerup GetRandomPowerupAtLevel(int level)
        {
            return powerupContainer.Powerups.FirstOrDefault(powerup => powerup.spawnAsPowerup && powerup.level <= level);
        }

        public static List<Powerup> GetSpawnablePowerupsOrderedByLevel()
        {
            return powerupContainer.Powerups.OrderBy(powerup => powerup.level).ToList();
        }

        public static Enums.PowerupType GetRandomPowerupTypeAtLevel(int level)
        {
            var powerupTypes =
                powerupContainer.Powerups.Where(powerup => powerup.spawnAsPowerup && powerup.level <= level)
                    .Select(powerup => powerup.powerupType)
                    .ToList();
            var player = GameObject.Find("Player").GetComponent<PlayerStats>();

            if (player.PlayerHealth == 3)
            {
                powerupTypes = powerupTypes.Where(powerup => powerup != Enums.PowerupType.FillHp).ToList();
            }

            if (player.TempHealth == player.PlayerHealth)
            {
                powerupTypes = powerupTypes.Where(powerup => powerup != Enums.PowerupType.TemporaryHp).ToList();
            }

            if (player.CurrentMultiplier == 4)
            {
                powerupTypes = powerupTypes.Where(powerup => powerup != Enums.PowerupType.Grail).ToList();
            }

            return powerupTypes.ElementAt(Random.Range(0, powerupTypes.Count));
        }

        public static Enums.PowerupType GetRandomPowerupType()
        {
            var powerupTypes = GetSpawnablePowerupTypes();
            return powerupTypes.ElementAt(Random.Range(0, powerupTypes.Count));
        }

        public static List<Enums.PowerupType> GetSpawnablePowerupTypes()
        {
            return s_spawnablePowerupTypes;
        }

        public static Powerup GetPowerup(Enums.PowerupType type)
        {
            return powerupContainer.Powerups.FirstOrDefault(powerupX => powerupX.powerupType == type);
        }
    }
}
