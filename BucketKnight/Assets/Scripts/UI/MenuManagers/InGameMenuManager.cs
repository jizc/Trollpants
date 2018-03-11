// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InGameMenuManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using CloudOnce;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class InGameMenuManager : MenuManager
    {
        private const int c_fontDefaultSize = 26;
        private const float c_secondsBeforeEachRewardedAd = 180f;
        private float _startTime;

        public Canvas inGameMenu;
        public Canvas pauseMenu;
        public Canvas gameOverMenu;
        //public RectTransform gameOverPanel;
        public Image BlackFadeImage;

        protected override void OnEnable()
        {
            base.OnEnable();

            // Global event listeners
            Events.instance.AddListener<EnterPauseMenu>(OnEnterPauseMenu);
            Events.instance.AddListener<GamePaused>(OnGamePaused);
            Events.instance.AddListener<GameResumed>(OnGameResumed);
            Events.instance.AddListener<QuitToMainMenu>(OnQuitToMainMenu);
            Events.instance.AddListener<PlayerLost>(OnPlayerLost);
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
        }

        protected override void Start()
        {
            base.Start();
            _startTime = Time.time;
            DOTween.Init();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            SetFullInGameMenu(!SavedData.PlayTutorial);
            ActivateMenu(inGameMenu);
        }

        protected override void EscapeButtonPressed()
        {
            if (!SavedData.GameIsPaused)
            {
                Events.instance.Raise(new GamePaused());
                Events.instance.Raise(new EnterPauseMenu());
            }
            else if (pauseMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new GameResumed());
            }
            else if (gameOverMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new QuitToMainMenu());
            }
            else if (settingsMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new ExitSettingsMenu());
            }
            else if (tutorialMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new ExitTutorialMenu());
            }
            else if (shopMenu.gameObject.activeInHierarchy)
            {
                Events.instance.Raise(new ExitShopMenu());
            }
        }

        private void SetFullInGameMenu(bool fullMenu)
        {
            inGameMenu.GetComponent<CanvasGroup>().interactable = fullMenu;
            inGameMenu.GetComponent<CanvasGroup>().blocksRaycasts = fullMenu;
            inGameMenu.transform.Find("PauseButton").gameObject.SetActive(fullMenu);
            inGameMenu.transform.transform.Find("EquipmentSlots").Find("PowerupSlotOne").gameObject.SetActive(fullMenu);
            inGameMenu.transform.Find("EquipmentSlots").Find("PowerupSlotTwo").gameObject.SetActive(fullMenu);
            inGameMenu.transform.Find("ScoreText").gameObject.SetActive(!SavedData.PlayTutorial);
            inGameMenu.transform.Find("Multiplier").gameObject.SetActive(!SavedData.PlayTutorial);
        }

        #region Event handlers

        private void OnEnterPauseMenu(EnterPauseMenu enterPauseMenuEvent)
        {
            ToggleBlackFade(true);
            ActivateMenu(pauseMenu);
            defaultUI.gameObject.SetActive(true);
        }

        private void OnGamePaused(GamePaused gamePausedEvent)
        {
            Time.timeScale = 0;
            SetFullInGameMenu(false);
        }

        protected override void OnExitSettingsMenu(ExitSettingsMenu exitSettingsMenuEvent)
        {
            if (GameObject.Find("Player").GetComponent<PlayerStats>().PlayerHealth > 0)
            {
                Events.instance.Raise(new EnterPauseMenu());
            }
            else
            {
                ActivateMenu(gameOverMenu);
            }
        }

        protected override void OnEnterTutorialMenu(EnterTutorialMenu enterTutorialMenuEvent)
        {
            ActivateMenu(tutorialMenu);
        }

        protected override void OnExitTutorialMenu(ExitTutorialMenu exitTutorialMenuEvent)
        {
            Events.instance.Raise(new EnterSettingsMenu());
        }

        private void OnGameResumed(GameResumed gameResumedEvent)
        {
            defaultUI.gameObject.SetActive(false);
            SetFullInGameMenu(true);
            ActivateMenu(inGameMenu);
            ToggleBlackFade(false);
            Time.timeScale = 1;
        }

        public void OnQuitToMainMenu(QuitToMainMenu quitToMainMenuEvent)
        {
            if (SavedData.PlayTutorial)
            {
                Events.instance.Raise(new TutorialToggled(false));
            }

            SceneManager.LoadScene(1);
        }

        private void OnPlayerLost(PlayerLost playerLostEvent)
        {
            if (SavedData.PlayTutorial)
            {
                return;
            }

            var timePlayed = Time.time - _startTime;
            SavedData.SecondsPlayedSinceLastAd += timePlayed;
            Debug.Log(string.Format("Played {0} seconds this session. Played a total of {1} since last ad watched.",
                timePlayed, SavedData.SecondsPlayedSinceLastAd));

            SavedData.HighScore = playerLostEvent.finalScore;
            ActivateMenu(gameOverMenu);

            ToggleBlackFade(true);

            //if (SavedData.SecondsPlayedSinceLastAd > c_secondsBeforeEachRewardedAd)
            //{
            //    Debug.Log(string.Format("Over {0} seconds since last ad was watched, showing rewarded ad button.", c_secondsBeforeEachRewardedAd));
            //    adsPanel.gameObject.SetActive(true);
            //    adsCanvasGroup.interactable = true;
            //    adsCanvasGroup.blocksRaycasts = true;
            //    adsCanvasGroup.alpha = 1f;
            //    adsPanel.DOMoveX(365f, 1.1f)
            //        .From(true)
            //        .SetEase(Ease.OutElastic, 1.1f, 0f);
            //    DOTween.ToAlpha(() => AdsButtonFlashImage.color, x => AdsButtonFlashImage.color = x, 0.47f, 0.5f)
            //        .SetEase(Ease.InQuad)
            //        .SetLoops(-1, LoopType.Yoyo);
            //    DOTween.To(() => AdsText.fontSize, x => AdsText.fontSize = x, 32, 0.5f)
            //        .SetEase(Ease.InQuad)
            //        .SetLoops(-1, LoopType.Yoyo);
            //    gameOverPanel.DOMoveX(-860f, 0.75f).From();
            //}
            //else
            //{
            //    adsPanel.gameObject.SetActive(false);
            //    gameOverPanel.DOMoveX(-860f, 0.5f).From();
            //}

            Cloud.Storage.Save();
        }

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            _startTime = Time.time;
        }

        protected override void OnExitShopMenu(ExitShopMenu exitShopMenuEvent)
        {
            ActivateMenu(gameOverMenu);
            Cloud.Storage.Save();
        }

        #endregion

        private void ToggleBlackFade(bool fadeOn, float amount = 0.47f, float duration = 0.75f, Ease ease = Ease.OutQuint)
        {
            DOTween.ToAlpha(() => BlackFadeImage.color, x => BlackFadeImage.color = x, fadeOn ? amount : 0f, duration).SetEase(ease);
        }

        private void OnApplicationFocus(bool focused)
        {
            if (!focused && !SavedData.GameIsPaused)
            {
                Events.instance.Raise(new GamePaused());
                Events.instance.Raise(new EnterPauseMenu());
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Events.instance.RemoveListener<EnterPauseMenu>(OnEnterPauseMenu);
            Events.instance.RemoveListener<GamePaused>(OnGamePaused);
            Events.instance.RemoveListener<GameResumed>(OnGameResumed);
            Events.instance.RemoveListener<QuitToMainMenu>(OnQuitToMainMenu);
            Events.instance.RemoveListener<PlayerLost>(OnPlayerLost);
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
        }
    }
}
