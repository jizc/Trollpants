// <copyright file="Merchant.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Player
{
    using System;
    using CloudOnce;
    using Data;
    using Environment;
    using UnityEngine;
    using UnityEngine.UI;

    public class Merchant : MonoBehaviour
    {
        [SerializeField] private Text coinAmount;
        [SerializeField] private Text distanceRecord;
        [SerializeField] private GameObject upgrades;
        [SerializeField] private GameObject potions;
        [SerializeField] private GameObject upgradesButton;
        [SerializeField] private GameObject potionsButton;
        [SerializeField] private UpgradeViewModel manaAmount;
        [SerializeField] private UpgradeViewModel verticalSpeed;
        [SerializeField] private UpgradeViewModel boostSpeed;
        [SerializeField] private UpgradeViewModel manaRegen;
        [SerializeField] private Text wisdomCount;
        [SerializeField] private Text midasCount;

        public static Merchant Instance { get; private set; }

        public void SwitchToPotions()
        {
            upgrades.SetActive(false);
            potionsButton.SetActive(false);

            potions.SetActive(true);
            upgradesButton.SetActive(true);
        }

        public void SwitchToUpgrades()
        {
            potions.SetActive(false);
            upgradesButton.SetActive(false);

            upgrades.SetActive(true);
            potionsButton.SetActive(true);
        }

        public void UpgradeSkill(Upgrade upgrade)
        {
            UpgradeViewModel upgradeViewModel;
            int unlockedLevel;
            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    upgradeViewModel = manaAmount;
                    unlockedLevel = CloudVariables.UnlockedManaAmountLevel;
                    break;
                case Upgrade.VerticalSpeed:
                    upgradeViewModel = verticalSpeed;
                    unlockedLevel = CloudVariables.UnlockedVerticalSpeedLevel;
                    break;
                case Upgrade.BoostSpeed:
                    upgradeViewModel = boostSpeed;
                    unlockedLevel = CloudVariables.UnlockedBoostSpeedLevel;
                    break;
                case Upgrade.ManaRegenRate:
                    upgradeViewModel = manaRegen;
                    unlockedLevel = CloudVariables.UnlockedManaRegenLevel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }

            if (upgradeViewModel.CurrentLevel == UpgradeInfo.MaxUpgradeLevel)
            {
                AudioClipPlayer.PlayIncorrect();
                return;
            }

            if (upgradeViewModel.CurrentLevel < unlockedLevel)
            {
                upgradeViewModel.SetCurrentLevel(upgradeViewModel.CurrentLevel + 1);
                AudioClipPlayer.Instance.PlayClick();
                UpdateGUI(upgrade);
                return;
            }

            var price = UpgradeInfo.GetUpgradePrice(unlockedLevel + 1);
            if (Player.State.Coins < price)
            {
                AudioClipPlayer.PlayIncorrect();
                return;
            }

            Player.State.Coins -= price;
            upgradeViewModel.SetCurrentLevel(upgradeViewModel.CurrentLevel + 1);
            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    CloudVariables.UnlockedManaAmountLevel++;
                    break;
                case Upgrade.VerticalSpeed:
                    CloudVariables.UnlockedVerticalSpeedLevel++;
                    break;
                case Upgrade.BoostSpeed:
                    CloudVariables.UnlockedBoostSpeedLevel++;
                    break;
                case Upgrade.ManaRegenRate:
                    CloudVariables.UnlockedManaRegenLevel++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }

            Cloud.Storage.Save();
            AudioClipPlayer.PlayUpgrade();
            UpdateGUI(upgrade);
        }

        public void DowngradeSkill(Upgrade upgrade)
        {
            UpgradeViewModel upgradeViewModel;
            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    upgradeViewModel = manaAmount;
                    break;
                case Upgrade.VerticalSpeed:
                    upgradeViewModel = verticalSpeed;
                    break;
                case Upgrade.BoostSpeed:
                    upgradeViewModel = boostSpeed;
                    break;
                case Upgrade.ManaRegenRate:
                    upgradeViewModel = manaRegen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }

            if (upgradeViewModel.CurrentLevel == 1)
            {
                AudioClipPlayer.PlayIncorrect();
                return;
            }

            upgradeViewModel.SetCurrentLevel(upgradeViewModel.CurrentLevel - 1);
            UpdateGUI(upgrade);
        }

        public void BuyWisdomPotion()
        {
            const int price = 100;
            if (Player.State.Coins < price)
            {
                AudioClipPlayer.PlayIncorrect();
                return;
            }

            Player.State.Coins -= price;
            Player.State.WisdomPotions++;
            Cloud.Storage.Save();

            AudioClipPlayer.PlayPotion();
            wisdomCount.text = Player.State.WisdomPotions.ToString();
        }

        public void BuyMidasPotion()
        {
            const int price = 100;
            if (Player.State.Coins < price)
            {
                AudioClipPlayer.PlayIncorrect();
                return;
            }

            Player.State.Coins -= price;
            Player.State.MidasPotions++;
            Cloud.Storage.Save();

            AudioClipPlayer.PlayPotion();
            midasCount.text = Player.State.MidasPotions.ToString();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GetCurrentUpgradeLevels();
            UpdateGUI();
            distanceRecord.text = CloudVariables.DistanceRecord + "m";
            SwitchToUpgrades();
            wisdomCount.text = Player.State.WisdomPotions.ToString();
            midasCount.text = Player.State.MidasPotions.ToString();
        }

        private void UpdateGUI()
        {
            UpdateGUI(Upgrade.ManaAmount, false);
            UpdateGUI(Upgrade.VerticalSpeed, false);
            UpdateGUI(Upgrade.BoostSpeed, false);
            UpdateGUI(Upgrade.ManaRegenRate);
        }

        private void UpdateGUI(Upgrade upgrade, bool updateCoinAmount = true)
        {
            SetButtonInteractivity(upgrade);
            SetUpgradePrices(upgrade);
            RefreshUpgradeLevelLabel(upgrade);
            SetActivePriceTags(upgrade);
            if (updateCoinAmount)
            {
                coinAmount.text = Player.State.Coins.ToString();
            }
        }

        private void GetCurrentUpgradeLevels()
        {
            manaAmount.RefreshCurrentLevel(CloudVariables.UnlockedManaAmountLevel);
            verticalSpeed.RefreshCurrentLevel(CloudVariables.UnlockedVerticalSpeedLevel);
            boostSpeed.RefreshCurrentLevel(CloudVariables.UnlockedBoostSpeedLevel);
            manaRegen.RefreshCurrentLevel(CloudVariables.UnlockedManaRegenLevel);
        }

        private void SetButtonInteractivity(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    manaAmount.RefreshButtonInteractivity();
                    break;
                case Upgrade.VerticalSpeed:
                    verticalSpeed.RefreshButtonInteractivity();
                    break;
                case Upgrade.BoostSpeed:
                    boostSpeed.RefreshButtonInteractivity();
                    break;
                case Upgrade.ManaRegenRate:
                    manaRegen.RefreshButtonInteractivity();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }
        }

        private void SetUpgradePrices(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    if (CloudVariables.UnlockedManaAmountLevel < UpgradeInfo.MaxUpgradeLevel)
                    {
                        manaAmount.SetUpgradePrice(UpgradeInfo.GetUpgradePrice(CloudVariables.UnlockedManaAmountLevel + 1));
                    }

                    break;
                case Upgrade.VerticalSpeed:
                    if (CloudVariables.UnlockedVerticalSpeedLevel < UpgradeInfo.MaxUpgradeLevel)
                    {
                        verticalSpeed.SetUpgradePrice(UpgradeInfo.GetUpgradePrice(CloudVariables.UnlockedVerticalSpeedLevel + 1));
                    }

                    break;
                case Upgrade.BoostSpeed:
                    if (CloudVariables.UnlockedBoostSpeedLevel < UpgradeInfo.MaxUpgradeLevel)
                    {
                        boostSpeed.SetUpgradePrice(UpgradeInfo.GetUpgradePrice(CloudVariables.UnlockedBoostSpeedLevel + 1));
                    }

                    break;
                case Upgrade.ManaRegenRate:
                    if (CloudVariables.UnlockedManaRegenLevel < UpgradeInfo.MaxUpgradeLevel)
                    {
                        manaRegen.SetUpgradePrice(UpgradeInfo.GetUpgradePrice(CloudVariables.UnlockedManaRegenLevel + 1));
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }
        }

        private void RefreshUpgradeLevelLabel(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    manaAmount.RefreshLevelLabel();
                    break;
                case Upgrade.VerticalSpeed:
                    verticalSpeed.RefreshLevelLabel();
                    break;
                case Upgrade.BoostSpeed:
                    boostSpeed.RefreshLevelLabel();
                    break;
                case Upgrade.ManaRegenRate:
                    manaRegen.RefreshLevelLabel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }
        }

        private void SetActivePriceTags(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case Upgrade.ManaAmount:
                    manaAmount.SetActivePriceTag(CloudVariables.UnlockedManaAmountLevel);
                    break;
                case Upgrade.VerticalSpeed:
                    verticalSpeed.SetActivePriceTag(CloudVariables.UnlockedVerticalSpeedLevel);
                    break;
                case Upgrade.BoostSpeed:
                    boostSpeed.SetActivePriceTag(CloudVariables.UnlockedBoostSpeedLevel);
                    break;
                case Upgrade.ManaRegenRate:
                    manaRegen.SetActivePriceTag(CloudVariables.UnlockedManaRegenLevel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("upgrade", upgrade, null);
            }
        }
    }
}
