// <copyright file="LivesAnimator.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class LivesAnimator : MonoBehaviour
    {
        [SerializeField] private Image coloredBackground;
        [SerializeField] private Text livesCount;
        [SerializeField] private float interval = 0.5f;

        private float timeCounter;
        private bool flipBack;

        private void Awake()
        {
            timeCounter = 0f;
            coloredBackground.fillAmount = 1f;
            livesCount.text = "4";
            flipBack = false;
        }

        private void Update()
        {
            if (timeCounter >= interval)
            {
                timeCounter = 0f;

                if (flipBack)
                {
                    coloredBackground.fillAmount = 1f;
                    livesCount.text = "4";
                }
                else
                {
                    coloredBackground.fillAmount = .75f;
                    livesCount.text = "3";
                }

                flipBack = !flipBack;
            }

            timeCounter += Time.deltaTime;
        }
    }
}
