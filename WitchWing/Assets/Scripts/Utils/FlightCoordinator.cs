// <copyright file="FlightCoordinator.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Utils
{
    using System.Collections;
    using Data;
    using Effects;
    using Environment;
    using GUI;
    using Player;
    using UnityEngine;
    using UnityEngine.Events;

    public class FlightCoordinator : MonoBehaviour
    {
        private static FlightCoordinator s_instance;

        [SerializeField] private ManaBarConstructor manaBarConstructor;
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private CloudShepherd cloudShepherd;
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private PotionEffects potionEffects;
        [SerializeField] private GameObject pauseScreen;

        private bool isFlightActive;
        private bool isFlightPaused;

        public static void OnPlayerDeath(bool payRespects = true)
        {
            CanvasCoordinator.SetHudInteractable(false);
            AudioClipPlayer.PlayDeath();
            s_instance.cameraShake.Shake();
            if (payRespects)
            {
                var coroutine = PayRespects(2f, s_instance.ResetAndGoToShop);
                s_instance.StartCoroutine(coroutine);
            }
            else
            {
                s_instance.ResetAndGoToShop();
            }
        }

        public void AbortFlight()
        {
            pauseScreen.SetActive(false);
            CanvasCoordinator.SetHudInteractable(false);
            Player.Kill();
            ResetAndGoToShop();
        }

        public void PauseFlight()
        {
            isFlightActive = false;
            GameState.IsPaused = true;
            pauseScreen.SetActive(true);
            isFlightPaused = true;
        }

        public void ResumeFlight()
        {
            isFlightPaused = false;
            pauseScreen.SetActive(false);
            GameState.IsPaused = false;
            isFlightActive = true;
        }

        public void MainMenyPlayButton()
        {
            if (PlayerSettings.HasSeenControlsTutorial)
            {
                CanvasCoordinator.GoToUpgradesFromMainMenu();
            }
            else
            {
                StartNewFlight();
            }
        }

        public void StartNewFlight()
        {
            GameState.CoinValue = 1;
            GameState.DifficultyLevel = 1;
            Player.State.Refresh();
            Player.Revive();
            potionEffects.RefreshButtonInteractable();
            CanvasCoordinator.SetHudInteractable(true);
            AudioClipPlayer.FadeFromMenuToInGameMusic();
            manaBarConstructor.UpdateManaBarSize();

            UnityAction onHudShown = () =>
            {
                if (PlayerSettings.HasSeenControlsTutorial)
                {
                    GameState.IsPaused = false;
                    isFlightActive = true;
                }
                else
                {
                    TutorialCoordinator.ActivateTiltTutorial();
                }
            };
            CanvasCoordinator.HideMenuesAndShowHud(onHudShown);
        }

        private static IEnumerator PayRespects(float duration, UnityAction onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete.Invoke();
        }

        private void Awake()
        {
            s_instance = this;
#if UNITY_EDITOR
            if (!CloudOnce.Cloud.IsSignedIn)
            {
                CloudOnce.Cloud.Initialize();
            }
#endif
            PlayerSettings.Load();
        }

        private void Update()
        {
            if (TutorialCoordinator.IsActive)
            {
                return;
            }

            if (isFlightActive && Input.GetKeyDown(KeyCode.Escape))
            {
                AudioClipPlayer.Instance.PlayClick();
                PauseFlight();
            }
            else if (isFlightPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                AudioClipPlayer.Instance.PlayClick();
                ResumeFlight();
            }
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isFlightActive && isPaused && !isFlightPaused)
            {
                PauseFlight();
            }
        }

        private void ResetAndGoToShop()
        {
            isFlightActive = false;
            const float fadeDuration = 0.4f;
            AudioClipPlayer.FadeOutInGameMusic(fadeDuration * 0.5f);
            CanvasCoordinator.FadeOutAndIn(fadeDuration, () =>
            {
                s_instance.ResetGame();
                CanvasCoordinator.HideHudAndShowShopMenu();
                AudioClipPlayer.FadeInMenuMusic(fadeDuration * 0.5f);
            });
        }

        private void ResetGame()
        {
            GameState.IsPaused = true;
            Player.State.SaveAndReset();
            worldGenerator.ResetWorld();
            WorldScroller.ResetPositions();
            cloudShepherd.ResetClouds();
        }
    }
}
