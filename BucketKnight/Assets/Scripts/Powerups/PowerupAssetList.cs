// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerupAssetList.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class PowerupAssetList : MonoBehaviour
    {
        public List<Sprite> AssetList = new List<Sprite>();

        public Sprite GetSprite(Enums.PowerupType type)
        {
            var countdownImageName = PowerupManager.GetPowerup(type).countDownImageName;
            return AssetList.FirstOrDefault(image => image.name == countdownImageName);
        }
    }
}
