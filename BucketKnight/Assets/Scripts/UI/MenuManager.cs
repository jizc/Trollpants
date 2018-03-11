// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class MenuManager : MonoBehaviour
    {
        public List<Canvas> menusToDeactivate = new List<Canvas>();

        public Canvas settingsMenu;
        public Canvas tutorialMenu;
        public Canvas shopMenu;
        public Canvas creditsMenu;
        public Canvas defaultUI;

        // CloudOnce
        protected const string c_guestPreferenceKey = "GooglePlayWantsToUseGuest";

        protected virtual void OnEnable()
        {
            Events.instance.AddListener<ShowLeaderboards>(OnShowLeaderboards);
            Events.instance.AddListener<EnterSettingsMenu>(OnEnterSettingsMenu);
            Events.instance.AddListener<ExitSettingsMenu>(OnExitSettingsMenu);
            Events.instance.AddListener<EnterShopMenu>(OnEnterShopMenu);
            Events.instance.AddListener<ExitShopMenu>(OnExitShopMenu);
            Events.instance.AddListener<EnterTutorialMenu>(OnEnterTutorialMenu);
            Events.instance.AddListener<ExitTutorialMenu>(OnExitTutorialMenu);
        }

        protected virtual void Start()
        {
            foreach (Transform subMenuTransform in transform.Find("SubMenus").transform)
            {
                menusToDeactivate.Add(subMenuTransform.GetComponent<Canvas>());
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!transform.Find("ConfirmationMenuManager").GetComponent<ConfirmationMenuManager>().confirmationBoxIsShowing)
                {
                    EscapeButtonPressed();
                }
            }
        }

        protected abstract void EscapeButtonPressed();

        protected virtual void ActivateMenu(Canvas menu)
        {
            DeactivateMenus();
            menu.gameObject.SetActive(true);
        }

        protected void DeactivateMenus()
        {
            foreach (var menuCanvas in menusToDeactivate)
            {
                menuCanvas.gameObject.SetActive(false);
            }
        }

        #region Event handlers

        protected void OnShowLeaderboards(ShowLeaderboards showLeaderboardsEvent)
        {
            if (Cloud.IsSignedIn)
            {
                Cloud.Leaderboards.ShowOverlay();
            }
            else
            {
                Debug.Log("Cant show leaderboard service not ready!");
#if CLOUDONCE_GOOGLE
                PlayerPrefs.SetInt(c_guestPreferenceKey, 0);
#endif
                Cloud.Initialize();
            }
        }

        protected void OnEnterSettingsMenu(EnterSettingsMenu enterSettingsMenuEvent)
        {
            ActivateMenu(settingsMenu);
        }

        protected abstract void OnExitSettingsMenu(ExitSettingsMenu exitSettingsMenuEvent);

        private void OnEnterShopMenu(EnterShopMenu enterShopMenuEvent)
        {
            shopMenu.transform.Find("StoreNavigation").transform.Find("UnlockToggle").GetComponent<Toggle>().isOn = true;
            shopMenu.transform.Find("StoreNavigation").transform.Find("EquipToggle").GetComponent<Toggle>().isOn = false;
            ActivateMenu(shopMenu);
        }

        protected abstract void OnExitShopMenu(ExitShopMenu exitShopMenuEvent);
        protected abstract void OnEnterTutorialMenu(EnterTutorialMenu enterTutorialMenuEvent);
        protected abstract void OnExitTutorialMenu(ExitTutorialMenu exitTutorialMenuEvent);

        #endregion

        protected virtual void OnDisable()
        {
            Events.instance.RemoveListener<ShowLeaderboards>(OnShowLeaderboards);
            Events.instance.RemoveListener<EnterSettingsMenu>(OnEnterSettingsMenu);
            Events.instance.RemoveListener<ExitSettingsMenu>(OnExitSettingsMenu);
            Events.instance.RemoveListener<EnterShopMenu>(OnEnterShopMenu);
            Events.instance.RemoveListener<ExitShopMenu>(OnExitShopMenu);
            Events.instance.RemoveListener<EnterTutorialMenu>(OnEnterTutorialMenu);
            Events.instance.RemoveListener<ExitTutorialMenu>(OnExitTutorialMenu);
        }
    }
}
