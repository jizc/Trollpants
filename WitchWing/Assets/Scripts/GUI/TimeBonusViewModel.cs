// <copyright file="TimeBonusViewModel.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasGroup))]
    public class TimeBonusViewModel : MonoBehaviour
    {
        private const float screenHeight = 1080f;
        private const float halfScreenHeight = screenHeight * 0.5f;
        private const float quarterScreenHeight = screenHeight * 0.25f;
        private const float maxDelta = 4f;

        [SerializeField] private Text rewardText;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        public void ShowTimeBonus(int rewardAmount)
        {
            rewardText.text = "+" + rewardAmount;
            StartCoroutine(FadeIn());
        }

        private void Awake()
        {
            rectTransform = (RectTransform)transform;
            canvasGroup = GetComponent<CanvasGroup>();
            ResetFadeAndPosition();
        }

        private IEnumerator FadeIn()
        {
            var effectsComplete = 0;
            var fadeComplete = false;
            var moveComplete = false;

            while (effectsComplete < 2)
            {
                if (Math.Abs(canvasGroup.alpha - 1f) < 0.0001f)
                {
                    if (!fadeComplete)
                    {
                        effectsComplete++;
                        fadeComplete = true;
                    }
                }
                else
                {
                    canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, maxDelta * Time.deltaTime);
                }

                if (rectTransform.anchoredPosition.y >= halfScreenHeight)
                {
                    if (!moveComplete)
                    {
                        effectsComplete++;
                        moveComplete = true;
                    }
                }
                else
                {
                    rectTransform.anchoredPosition = Vector2.MoveTowards(
                        rectTransform.anchoredPosition,
                        new Vector2(rectTransform.anchoredPosition.x, halfScreenHeight),
                        Time.deltaTime * halfScreenHeight * maxDelta);
                }

                yield return null;
            }

            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha = Mathf.MoveTowards(
                    canvasGroup.alpha,
                    0f,
                    Time.deltaTime * maxDelta * maxDelta * 0.5f);
                rectTransform.anchoredPosition = Vector2.MoveTowards(
                    rectTransform.anchoredPosition,
                    new Vector2(rectTransform.anchoredPosition.x, screenHeight),
                    Time.deltaTime * screenHeight * maxDelta * 0.5f);
                yield return null;
            }

            ResetFadeAndPosition();
        }

        private void ResetFadeAndPosition()
        {
            rectTransform.anchoredPosition = new Vector2(0f, quarterScreenHeight);
            canvasGroup.alpha = 0f;
        }
    }
}
