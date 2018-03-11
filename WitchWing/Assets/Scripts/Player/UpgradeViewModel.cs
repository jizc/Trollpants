// <copyright file="UpgradeViewModel.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Player
{
    using System;
    using Data;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class UpgradeViewModel
    {
        [SerializeField] private string playerPrefsKey;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button downgradeButton;
        [SerializeField] private Text priceLabel;
        [SerializeField] private Text levelLabel;
        [SerializeField] private int currentLevel = 1;

        public Button UpgradeButton
        {
            get { return upgradeButton; }
            set { upgradeButton = value; }
        }

        public Button DowngradeButton
        {
            get { return downgradeButton; }
            set { downgradeButton = value; }
        }

        public Text PriceLabel
        {
            get { return priceLabel; }
            set { priceLabel = value; }
        }

        public Text LevelLabel
        {
            get { return levelLabel; }
            set { levelLabel = value; }
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
        }

        public void RefreshCurrentLevel(int unlockedLevel)
        {
            var storedLevel = PlayerPrefs.GetInt(playerPrefsKey, 1);
            if (storedLevel > unlockedLevel)
            {
                currentLevel = unlockedLevel;
                PlayerPrefs.SetInt(playerPrefsKey, unlockedLevel);
                PlayerPrefs.Save();
            }
            else
            {
                currentLevel = storedLevel;
            }
        }

        public void SetCurrentLevel(int level)
        {
            currentLevel = level;
            PlayerPrefs.SetInt(playerPrefsKey, level);
            PlayerPrefs.Save();
        }

        public void SetUpgradePrice(int price)
        {
            priceLabel.text = price.ToString();
        }

        public void SetActivePriceTag(int unlockedLevel)
        {
            priceLabel.transform.parent.gameObject.SetActive(
                currentLevel < UpgradeInfo.MaxUpgradeLevel
                && currentLevel == unlockedLevel);
        }

        public void RefreshLevelLabel()
        {
            levelLabel.text = string.Format("Level {0} / {1}", currentLevel, UpgradeInfo.MaxUpgradeLevel);
        }

        public void RefreshButtonInteractivity()
        {
            upgradeButton.interactable = currentLevel < UpgradeInfo.MaxUpgradeLevel;
            downgradeButton.interactable = currentLevel > 1;
        }
    }
}
