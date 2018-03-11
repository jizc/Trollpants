// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuyWindow.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.UI;

    public class BuyWindow : MonoBehaviour
    {
        public Text titleText;
        public Text infoText;
        public Text priceText;
        public int price;
        private int _powerupLevel;
        public string alreadyUnlockedText = "Already Unlocked";
        public string unavailableText = "Unavailable";
        public GameObject buyBtnBlocker;
        public Text buyBtnBlockerText;
        public Image powerupImage;
        public GameObject unlockBuyContainer;
        public GameObject equipBuyContainer;

        private Enums.PowerupType _powerupType;

        public void SetInfo(BuyWindowInfo info)
        {
            titleText.text = info.powerup.Name;
            infoText.text = info.powerup.PowerupText;
            _powerupType = info.powerup.powerupType;
            powerupImage.sprite = info.powerupImage;
            _powerupLevel = info.powerup.level;
            switch (info.buttonType)
            {
                case ShopPowerupButton.ShopButtonType.unlock:
                    unlockBuyContainer.SetActive(true);
                    equipBuyContainer.SetActive(false);

                    if (CloudVariables.NumberOfPowerupsUnlocked < info.powerup.level - 1)
                    {
                        buyBtnBlocker.SetActive(true);
                        buyBtnBlockerText.text = unavailableText;
                    }
                    else if (CloudVariables.NumberOfPowerupsUnlocked >= info.powerup.level)
                    {
                        buyBtnBlocker.SetActive(true);
                        buyBtnBlockerText.text = alreadyUnlockedText;
                    }
                    else
                    {
                        buyBtnBlocker.SetActive(false);
                    }

                    price = info.powerup.unlockPrice;
                    priceText.text = info.powerup.unlockPrice.ToString();
                    break;
                case ShopPowerupButton.ShopButtonType.equip:
                    unlockBuyContainer.SetActive(false);
                    equipBuyContainer.SetActive(true);
                    price = info.powerup.equipPrice;
                    priceText.text = info.powerup.equipPrice.ToString();
                    break;
            }
        }

        public void ShowWindow()
        {
            gameObject.SetActive(true);
        }

        public void ShowWindow(BuyWindowInfo info)
        {
            SetInfo(info);
            ShowWindow();
        }

        public void HideWindow()
        {
            gameObject.SetActive(false);
        }

        public void BuyUnlock()
        {
            if (SavedData.TotalCoins >= price)
            {
                SavedData.TotalCoins -= price;
                var newPowerupLevel = _powerupLevel;
                Events.instance.Raise(new PowerupUnlocked(newPowerupLevel));

                //Cloud.Achievements.IncrementAchievement(CloudIDs.AchievementIDs.GettingHearted, newPowerupLevel,
                //    SavedData.GettingHeartedUnlocksToGet);
                //Cloud.Achievements.IncrementAchievement(CloudIDs.AchievementIDs.BubbleTrouble, newPowerupLevel,
                //    SavedData.BubbleTroubleUnlocksToGet);
                //Cloud.Achievements.IncrementAchievement(CloudIDs.AchievementIDs.Kamelot, newPowerupLevel,
                //    SavedData.KamelotUnlocksToGet);

                gameObject.SetActive(false);
            }
            else
            {
                Events.instance.Raise(new ShowInfoBox("You do not have enough coins to unlock this."));
            }
        }

        public void BuyEquipment()
        {
            if (SavedData.PowerupInSlotOne != 0 && SavedData.PowerupInSlotTwo != 0)
            {
                Events.instance.Raise(
                    new ShowInfoBox(
                        "You have no slots available for this powerup. Sell or use one of your equipped powerups before you buy new ones."));
            }
            else if (SavedData.TotalCoins >= price)
            {
                SavedData.TotalCoins -= price;
                if (SavedData.PowerupInSlotOne == 0)
                {
                    Events.instance.Raise(new PowerupSlotOneEquipped(_powerupType));
                }
                else if (SavedData.PowerupInSlotTwo == 0)
                {
                    Events.instance.Raise(new PowerupSlotTwoEquipped(_powerupType));
                }

                gameObject.SetActive(false);
            }
            else
            {
                Events.instance.Raise(new ShowInfoBox("You do not have enough coins to buy this."));
            }
        }
    }
}
