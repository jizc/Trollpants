// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopButtonGem.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    public class ShopButtonGem : ShopPowerupButton
    {
        public new string name;
        public string infoText;
        public int unlockPrice;
        public int equipPrice;

        protected override void Start()
        {
            buyWindowInfo = new BuyWindowInfo
            {
                powerup = new Powerup
                {
                    Name = name,
                    PowerupText = infoText,
                    unlockPrice = unlockPrice,
                    equipPrice = equipPrice
                },
                buttonType = buttonType,
                powerupImage = powerupImage
            };
        }
    }
}
