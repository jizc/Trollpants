// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WarningMenu.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine.UI;

    public class WarningMenu : ConfirmationMenu
    {
        private Button _trueButton;
        private Button _falseButton;

        private void Start()
        {
            _trueButton = transform.Find("TrueButton").GetComponent<Button>();
            _falseButton = transform.Find("FalseButton").GetComponent<Button>();

            _trueButton.onClick.AddListener(OnTrueButtonClicked);
            _falseButton.onClick.AddListener(OnFalseButtonClicked);
        }

        private void OnTrueButtonClicked()
        {
            Events.instance.Raise(new ConfirmationBoxAnswered(true));
        }

        private void OnFalseButtonClicked()
        {
            Events.instance.Raise(new ConfirmationBoxAnswered(false));
        }
    }
}
