// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundToggle.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using UnityEngine;

    public class SoundToggle : CustomToggleWithText
    {
        protected override bool GetToggleValue()
        {
            return SavedData.SoundToggledOn;
        }

        protected override string GetTextValue()
        {
            return toggleTexts[Convert.ToInt32(SavedData.SoundToggledOn)];
        }

        protected override Color GetTextColor()
        {
            return toggleColors[Convert.ToInt32(SavedData.SoundToggledOn)];
        }

        protected override void OnToggled(bool toggledOn)
        {
            base.OnToggled(toggledOn);
            Events.instance.Raise(new SoundToggled(toggledOn));
        }
    }
}
