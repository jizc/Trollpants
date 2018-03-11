// <copyright file="UpgradeInfo.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Data
{
    using System;
    using System.Collections.Generic;

    public enum Upgrade
    {
        ManaAmount,
        VerticalSpeed,
        BoostSpeed,
        ManaRegenRate
    }

    public static class UpgradeInfo
    {
        public const int MaxUpgradeLevel = 15;

        private const float manaStartValue = 20f;
        private const float verticalStartValue = 2.4f;
        private const float boostStartValue = 3f;
        private const float regenStartValue = 5;

        private const float manaUpgradeIncrement = 20f;
        private const float verticalUpgradeIncrement = 0.4f;
        private const float boostUpgradeIncrement = 0.5f;
        private const float regenUpgradeIncrement = 5;

        private static readonly Dictionary<int, int> s_upgradePrices = new Dictionary<int, int>
        {
            { 2, 10 },
            { 3, 25 },
            { 4, 50 },
            { 5, 100 },
            { 6, 175 },
            { 7, 275 },
            { 8, 400 },
            { 9, 550 },
            { 10, 725 },
            { 11, 925 },
            { 12, 1150 },
            { 13, 1400 },
            { 14, 1675 },
            { 15, 2000 }
        };

        public static float GetUpgradeValue(Upgrade upgrade, int level)
        {
            if (level < 1 || level > MaxUpgradeLevel)
            {
                throw new ArgumentOutOfRangeException("level");
            }

            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    return manaStartValue + ((level - 1) * manaUpgradeIncrement);
                case Upgrade.VerticalSpeed:
                    return verticalStartValue + ((level - 1) * verticalUpgradeIncrement);
                case Upgrade.BoostSpeed:
                    return boostStartValue + ((level - 1) * boostUpgradeIncrement);
                case Upgrade.ManaRegenRate:
                    return regenStartValue + ((level - 1) * regenUpgradeIncrement);
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }
        }

        public static int GetUpgradePrice(int level)
        {
            if (level < 2 || level > MaxUpgradeLevel)
            {
                throw new ArgumentOutOfRangeException("level");
            }

            return s_upgradePrices[level];
        }
    }
}
