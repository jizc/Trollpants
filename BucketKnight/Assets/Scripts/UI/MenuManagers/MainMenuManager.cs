// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainMenuManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenuManager : MenuManager
    {
        public Canvas mainMenu;
        public Canvas googlePlayMenu;

        private bool _tutorialWillForcePlay;

        protected override void OnEnable()
        {
            base.OnEnable();

            SavedData.GameIsPaused = true;
            PowerupManager.GetRandomPowerup();

            //Global event listeners
            Events.instance.AddListener<ShowAchievements>(OnShowAchievements);
            Events.instance.AddListener<GoInGame>(OnGoInGame);
            Events.instance.AddListener<EnterCreditsMenu>(OnEnterCreditsMenu);
            Events.instance.AddListener<ExitCreditsMenu>(OnExitCreditsMenu);
            Events.instance.AddListener<EnterGooglePlayMenu>(OnEnterGooglePlayMenu);
            Events.instance.AddListener<ExitGooglePlayMenu>(OnExitGooglePlayMenu);
            Events.instance.AddListener<ExitGame>(OnExitGame);

            defaultUI.gameObject.SetActive(true);
        }

        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            ActivateMenu(mainMenu);
        }

        protected override void EscapeButtonPressed()
        {
            if (mainMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new ExitGame());
            }
            else if (tutorialMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new ExitTutorialMenu());
            }
            else if (creditsMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new ExitCreditsMenu());
            }
            else if (shopMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new ExitShopMenu());
            }
            else
            {
                ActivateMenu(mainMenu);
            }
        }

        #region Event handlers

        private void OnShowAchievements(ShowAchievements showAchievementsEvent)
        {
            if (Cloud.IsSignedIn)
            {
                Cloud.Achievements.ShowOverlay();
            }
            else
            {
                Debug.Log("Cant show achievements service not ready!");
#if CLOUDONCE_GOOGLE
                PlayerPrefs.SetInt(c_guestPreferenceKey, 0);
#endif
                Cloud.Initialize();
            }
        }

        private void OnGoInGame(GoInGame goInGameEvent)
        {
            SceneManager.LoadScene(2);
        }

        private void OnSkipTutorialAnswered(ConfirmationBoxAnswered confirmationBoxAnsweredEvent)
        {
            Events.instance.RemoveListener<ConfirmationBoxAnswered>(OnSkipTutorialAnswered);
            if (confirmationBoxAnsweredEvent.answeredPositive)
            {
                Events.instance.Raise(new TutorialToggled(false));
                Events.instance.Raise(new GoInGame());
            }
        }

        protected override void OnExitSettingsMenu(ExitSettingsMenu exitSettingsMenuEvent)
        {
            ActivateMenu(mainMenu);
        }

        protected override void OnExitShopMenu(ExitShopMenu exitShopMenuEvent)
        {
            ActivateMenu(mainMenu);
            Cloud.Storage.Save();
        }

        protected override void OnEnterTutorialMenu(EnterTutorialMenu enterTutorialMenuEvent)
        {
            _tutorialWillForcePlay = !enterTutorialMenuEvent.openedFromSettings;
            tutorialMenu.transform.Find("Pages")
                .transform.Find("ControlChoicePage")
                .transform.Find("BackToMainMenuButton")
                .gameObject.SetActive(_tutorialWillForcePlay);
            ActivateMenu(tutorialMenu);
        }

        private void OnEnterCreditsMenu(EnterCreditsMenu enterCreditsMenuEvent)
        {
            ActivateMenu(creditsMenu);
        }

        private void OnExitCreditsMenu(ExitCreditsMenu enterCreditsMenuEvent)
        {
            ActivateMenu(settingsMenu);
        }

        private void OnEnterGooglePlayMenu(EnterGooglePlayMenu showGooglePlayWindowEvent)
        {
            ActivateMenu(googlePlayMenu);
        }

        private void OnExitGooglePlayMenu(ExitGooglePlayMenu exitGooglePlayMenuEvent)
        {
            ActivateMenu(mainMenu);
        }

        protected override void OnExitTutorialMenu(ExitTutorialMenu exitTutorialMenuEvent)
        {
            if (_tutorialWillForcePlay)
            {
                if (exitTutorialMenuEvent.exitToPlay)
                {
                    Events.instance.Raise(new ShowWarningBox("Do you really want to skip the tutorial?"));
                    Events.instance.AddListener<ConfirmationBoxAnswered>(OnSkipTutorialAnswered);
                }
                else
                {
                    ActivateMenu(mainMenu);
                }
            }
            else
            {
                Events.instance.Raise(new EnterSettingsMenu());
            }
        }

        private void OnExitGame(ExitGame exitGameEvent)
        {
            Application.Quit();
        }

        #endregion

        protected override void OnDisable()
        {
            base.OnDisable();

            Events.instance.RemoveListener<ShowAchievements>(OnShowAchievements);
            Events.instance.RemoveListener<GoInGame>(OnGoInGame);
            Events.instance.RemoveListener<EnterCreditsMenu>(OnEnterCreditsMenu);
            Events.instance.RemoveListener<ExitCreditsMenu>(OnExitCreditsMenu);
            Events.instance.RemoveListener<ExitGame>(OnExitGame);
            Events.instance.RemoveListener<EnterGooglePlayMenu>(OnEnterGooglePlayMenu);
            Events.instance.RemoveListener<ExitGooglePlayMenu>(OnExitGooglePlayMenu);
        }
    }
}
