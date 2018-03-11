// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TiltToggle.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TiltToggle : CustomToggleWithText
    {
        public bool useTiltButton;
        public Toggle otherToggleController;

        private new void OnEnable()
        {
            base.OnEnable();

            otherToggleController =
                transform.parent.Find(useTiltButton ? "TouchToggle" : "TiltToggle").GetComponent<Toggle>();

            if (toggleController.isOn)
            {
                toggleController.interactable = false;
            }
        }

        protected override bool GetToggleValue()
        {
            return SavedData.UseTilt == useTiltButton;
        }

        protected override string GetTextValue()
        {
            return toggleTexts[Convert.ToInt32(SavedData.UseTilt == useTiltButton)];
        }

        protected override Color GetTextColor()
        {
            return toggleColors[Convert.ToInt32(SavedData.UseTilt == useTiltButton)];
        }

        protected override void OnToggled(bool toggledOn)
        {
            base.OnToggled(toggledOn);
            if (toggledOn)
            {
                toggleController.interactable = false;

                otherToggleController.interactable = true;
                otherToggleController.isOn = false;

                Events.instance.Raise(new TiltToggled(useTiltButton));
            }
        }
    }
}
