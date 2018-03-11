// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfirmationMenuManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class ConfirmationMenuManager : MonoBehaviour
    {
        private ConfirmationMenu _warningMenu;
        private ConfirmationMenu _infoMenu;

        public bool confirmationBoxIsShowing;

        private void Awake()
        {
            _warningMenu = transform.Find("WarningMenu").GetComponent<ConfirmationMenu>();
            _infoMenu = transform.Find("InfoMenu").GetComponent<ConfirmationMenu>();

            HideAll();
        }

        private void OnEnable()
        {
            Events.instance.AddListener<ShowInfoBox>(OnShowInfoBox);
            Events.instance.AddListener<ShowWarningBox>(OnShowWarningBox);
            Events.instance.AddListener<ConfirmationBoxAnswered>(OnConfirmationBoxAnswered);
        }

        private void HideAll()
        {
            _warningMenu.gameObject.SetActive(false);
            _infoMenu.gameObject.SetActive(false);
            confirmationBoxIsShowing = false;
        }

        #region Event handlers

        private void OnShowInfoBox(ShowInfoBox showInfoBoxEvent)
        {
            _infoMenu.SetText(showInfoBoxEvent.infoText);
            _infoMenu.gameObject.SetActive(true);
            confirmationBoxIsShowing = true;
        }

        private void OnShowWarningBox(ShowWarningBox showWarningBoxEvent)
        {
            _warningMenu.SetText(showWarningBoxEvent.infoText);
            _warningMenu.gameObject.SetActive(true);
            confirmationBoxIsShowing = true;
        }

        private void OnConfirmationBoxAnswered(ConfirmationBoxAnswered confirmationBoxAnsweredEvent)
        {
            HideAll();
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<ShowInfoBox>(OnShowInfoBox);
            Events.instance.RemoveListener<ShowWarningBox>(OnShowWarningBox);
            Events.instance.RemoveListener<ConfirmationBoxAnswered>(OnConfirmationBoxAnswered);
        }
    }
}
