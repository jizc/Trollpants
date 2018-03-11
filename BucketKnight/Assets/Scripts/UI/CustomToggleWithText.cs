// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomToggleWithText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class CustomToggleWithText : CustomToggle
    {
        private Text _textController;
        public string[] toggleTexts;
        public Color[] toggleColors;

        public new void OnEnable()
        {
            base.OnEnable();

            _textController = GetComponentInChildren<Text>();
            _textController.text = GetTextValue();
            _textController.color = GetTextColor();
        }

        protected abstract string GetTextValue();

        protected abstract Color GetTextColor();

        protected override void OnToggled(bool toggledOn)
        {
            _textController.text = toggleTexts[Convert.ToInt32(toggledOn)];
            _textController.color = toggleColors[Convert.ToInt32(toggledOn)];
        }
    }
}
