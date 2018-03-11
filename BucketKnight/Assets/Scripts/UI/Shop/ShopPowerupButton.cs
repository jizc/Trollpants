// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopPowerupButton.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class ShopPowerupButton : MonoBehaviour
    {
        public enum ShopButtonType
        {
            unlock,
            equip
        }

        public Sprite powerupImage;
        public Enums.PowerupType powerupType;
        public ShopButtonType buttonType;
        protected BuyWindowInfo buyWindowInfo;
        public BuyWindow buyWindow;

        protected virtual void Start()
        {
            buyWindowInfo = new BuyWindowInfo
            {
                powerup = PowerupManager.GetPowerup(powerupType),
                powerupImage = powerupImage,
                buttonType = buttonType
            };
        }

        public void OpenBuyWindow()
        {
            buyWindow.ShowWindow(buyWindowInfo);
        }
    }
}
