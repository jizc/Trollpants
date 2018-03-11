// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopMenuUnlockManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShopMenuUnlockManager : ShopMenuManager
    {
        public Color lockedProgressColor;
        public Color unlockedProgressColor;
        public Color invisible;
        public Color visible;
        public Sprite padLockImg;
        public Sprite checkMarkImg;

        private Sprite emptyImage;

        protected override void OnEnable()
        {
            base.OnEnable();

            Events.instance.AddListener<PowerupUnlocked>(OnPowerupUnlocked);
            UpdateUnlocks(CloudVariables.NumberOfPowerupsUnlocked);
        }

        private void OnPowerupUnlocked(PowerupUnlocked powerupUnlockedEvent)
        {
            UpdateUnlocks(powerupUnlockedEvent.unlockLevel);
        }

        private void UpdateUnlocks(int unlockLevel)
        {
            var counter = 1;
            foreach (Transform barChild in transform.Find("ProgressBar").transform)
            {
                barChild.GetComponent<Image>().color = counter <= unlockLevel
                    ? unlockedProgressColor
                    : lockedProgressColor;
                counter++;
            }

            counter = 0;
            foreach (Transform overlayChild in transform.Find("Overlays").transform)
            {
                var overlayImage = overlayChild.GetComponent<Image>();
                overlayImage.color = visible;
                if (counter == unlockLevel)
                {
                    overlayImage.sprite = emptyImage;
                    overlayImage.color = invisible;
                }
                else if (counter > unlockLevel)
                {
                    overlayImage.sprite = checkMarkImg;
                }
                else if (counter < unlockLevel)
                {
                    overlayImage.sprite = padLockImg;
                }

                counter++;
            }
        }

        protected override void ShowExplanationIfNeeded()
        {
            if (!SavedData.UnlocksHaveBeenExplained)
            {
                ShowExplanation();
                SavedData.UnlocksHaveBeenExplained = true;
            }
        }

        private void Awake()
        {
            emptyImage = Sprite.Create(null, Rect.zero, Vector2.zero);
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<PowerupUnlocked>(OnPowerupUnlocked);
        }
    }
}
