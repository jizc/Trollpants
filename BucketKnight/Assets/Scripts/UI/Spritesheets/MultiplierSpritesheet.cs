// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiplierSpritesheet.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    public class MultiplierSpritesheet : CustomSpritesheet
    {
        private void OnEnable()
        {
            Events.instance.AddListener<MultiplierChanged>(OnMultiplierChanged);
        }

        private void OnMultiplierChanged(MultiplierChanged multiplierChangedEvent)
        {
            GoToSprite(multiplierChangedEvent.multiplierLevel - 1);
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<MultiplierChanged>(OnMultiplierChanged);
        }
    }
}
