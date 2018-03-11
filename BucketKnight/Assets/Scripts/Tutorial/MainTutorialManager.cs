// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainTutorialManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine.UI;

    public class MainTutorialManager : PageManager
    {
        public Button cancelButton;

        private void Start()
        {
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        #region Button event handlers

        private void OnCancelButtonClicked()
        {
            Events.instance.Raise(new ExitTutorialMenu(true));
        }

        #endregion
    }
}
