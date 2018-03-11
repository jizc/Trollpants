// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopMenuSellManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class ShopMenuSellManager : MonoBehaviour
    {
        private void OnEnable()
        {
            Events.instance.AddListener<SellEquipmentOne>(OnSellPowerupOne);
            Events.instance.AddListener<SellEquipmentTwo>(OnSellPowerupTwo);
        }

        private void OnSellPowerupOne(SellEquipmentOne e)
        {
            SavedData.TotalCoins += PowerupManager.GetPowerup(SavedData.PowerupInSlotOne).equipPrice;
        }

        private void OnSellPowerupTwo(SellEquipmentTwo e)
        {
            SavedData.TotalCoins += PowerupManager.GetPowerup(SavedData.PowerupInSlotTwo).equipPrice;
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<SellEquipmentOne>(OnSellPowerupOne);
            Events.instance.RemoveListener<SellEquipmentTwo>(OnSellPowerupTwo);
        }
    }
}
