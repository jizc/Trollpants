// <copyright file="CanvasCoordinator.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using DG.Tweening;
    using Environment;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using PlayerSettings = Data.PlayerSettings;

    public class CanvasCoordinator : MonoBehaviour
    {
        private static CanvasCoordinator s_instance;

        [SerializeField] private Image fadeImage;
        [SerializeField] private Image logo;
        [SerializeField] private LogoAnimator logoAnimator;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject shopMenu;
        [SerializeField] private CanvasGroup options;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject hud;

        private CanvasGroup hudCanvasGroup;
        private Sequence initSequence;

        public static void GoToUpgradesFromMainMenu()
        {
            s_instance.mainMenu.SetActive(false);
            s_instance.shopMenu.SetActive(true);
        }

        public static void SetHudInteractable(bool interactable)
        {
            s_instance.hudCanvasGroup.interactable = interactable;
        }

        public static void HideMenuesAndShowHud(UnityAction onFadeToBlack)
        {
            if (s_instance.initSequence != null)
            {
                s_instance.initSequence.Complete(true);
            }

            var sequence = DOTween.Sequence();
            sequence.Append(s_instance.fadeImage.DOFade(1f, 0.25f));
            sequence.AppendCallback(() =>
            {
                s_instance.mainMenu.SetActive(false);
                s_instance.shopMenu.SetActive(false);
                s_instance.hud.SetActive(true);
                onFadeToBlack.SafeInvoke();
            });
            sequence.Append(s_instance.fadeImage.DOFade(0f, 0.25f));
        }

        public static void HideHudAndShowShopMenu()
        {
            s_instance.hud.SetActive(false);
            s_instance.shopMenu.SetActive(true);
        }

        public static void FadeOutAndIn(float totalFadeDuration = 0.5f, UnityAction onFadeOut = null)
        {
            var sequence = DOTween.Sequence();
            sequence.OnStart(() =>
            {
                s_instance.fadeImage.color = new Color(0f, 0f, 0f, 0f);
                s_instance.fadeImage.gameObject.SetActive(true);
            });
            sequence.Append(s_instance.fadeImage.DOFade(1f, totalFadeDuration * 0.5f));
            sequence.AppendCallback(onFadeOut.SafeInvoke);
            sequence.Append(s_instance.fadeImage.DOFade(0f, totalFadeDuration * 0.5f));
            sequence.OnComplete(() => s_instance.fadeImage.gameObject.SetActive(false));
        }

        public void OpenOptions()
        {
            options.gameObject.SetActive(true);
            options.DOFade(1f, 0.1f);
        }

        public void CloseOptions()
        {
            PlayerSettings.Save();
            options.DOFade(0f, 0.1f)
                   .OnComplete(() => options.gameObject.SetActive(false));
        }

        public void GoToMainMenu()
        {
            shopMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        private static void TurnOffCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }

        private void Awake()
        {
            s_instance = this;

            hudCanvasGroup = hud.GetComponent<CanvasGroup>();
            var hudViewModel = hud.GetComponent<HudViewModel>();
            hudViewModel.Init();

            logo.rectTransform.localScale = new Vector3(10f, 10f, 0f);
            logo.rectTransform.anchoredPosition = new Vector2(0f, 1980f);

            fadeImage.color = Color.black;
            fadeImage.gameObject.SetActive(true);

            initSequence = DOTween.Sequence();
            initSequence.Append(fadeImage.DOFade(0f, 1f)
                                         .SetEase(Ease.Linear)
                                         .OnComplete(() => fadeImage.gameObject.SetActive(false)));
            initSequence.Append(logo.rectTransform.DOScale(Vector3.one, 0.25f));
            initSequence.Join(logo.rectTransform.DOAnchorPosY(-50f, 0.25f));
            initSequence.OnComplete(() =>
            {
                logoAnimator.enabled = true;
                initSequence = null;
            });

            mainMenu.SetActive(true);

            shopMenu.SetActive(false);
            TurnOffCanvasGroup(options);
            pausePanel.SetActive(false);
            hud.SetActive(false);
        }

        private void Update()
        {
            if (mainMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else if (shopMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
            {
                GoToMainMenu();
                AudioClipPlayer.Instance.PlayClick();
            }
        }
    }
}
