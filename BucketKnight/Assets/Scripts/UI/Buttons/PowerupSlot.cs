// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerupSlot.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PowerupSlot : MonoBehaviour
    {
        public Image buttonImage;
        public Color noImageColor;
        public Color powerupColor;

        private Sprite noImageSprite;

        public void AddPowerup(Sprite sprite)
        {
            buttonImage.sprite = sprite;
            buttonImage.color = powerupColor;
        }

        public void RemovePowerup()
        {
            buttonImage.sprite = noImageSprite;
            buttonImage.color = noImageColor;
        }

        private void Awake()
        {
            noImageSprite = Sprite.Create(null, Rect.zero, Vector2.zero);
        }
    }
}
