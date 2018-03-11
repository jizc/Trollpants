// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopMenuEquipmentManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    public class ShopMenuEquipmentManager : ShopMenuManager
    {
        protected override void ShowExplanationIfNeeded()
        {
            if (!SavedData.EquipmentHaveBeenExplained)
            {
                ShowExplanation();
                SavedData.EquipmentHaveBeenExplained = true;
            }
        }
    }
}
