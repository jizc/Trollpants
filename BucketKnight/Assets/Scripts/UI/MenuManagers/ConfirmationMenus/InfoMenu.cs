// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoMenu.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine.UI;

    public class InfoMenu : ConfirmationMenu
    {
        private Button _okButton;

        private void Start()
        {
            _okButton = transform.Find("ConfirmButton").GetComponent<Button>();

            _okButton.onClick.AddListener(OnOkButtonClicked);
        }

        private void OnOkButtonClicked()
        {
            Events.instance.Raise(new ConfirmationBoxAnswered(true));
        }
    }
}
