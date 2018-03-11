// <copyright file="InstructionsTimer.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using System.Globalization;
    using Audio;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(InstructionsGuiHandler))]
    [RequireComponent(typeof(PanelAnimatorHelper))]
    public class InstructionsTimer : MonoBehaviour
    {
        [SerializeField] private int countdownSeconds = 5;
        [SerializeField] private Text timerText;

        private InstructionsGuiHandler instructionsGuiHandler;
        private PanelAnimatorHelper panelAnimator;

        private int currentCountdownValue;
        private float currentInterval;
        private bool timerActive;

        public void StartTimer()
        {
            timerActive = true;
        }

        public void ResetTimer()
        {
            currentInterval = 1;
            currentCountdownValue = countdownSeconds;
            timerText.text = currentCountdownValue.ToString(CultureInfo.InvariantCulture);
            timerActive = false;
        }

        public void SkipInstructions()
        {
            timerActive = false;
            currentCountdownValue = 0;
            instructionsGuiHandler.DisableArrows();
            AudioClipPlayer.StopCountdown();
            panelAnimator.HidePanel();
        }

        private void Awake()
        {
            instructionsGuiHandler = GetComponent<InstructionsGuiHandler>();
            panelAnimator = GetComponent<PanelAnimatorHelper>();
            currentInterval = 1;
            currentCountdownValue = countdownSeconds;
            timerText.text = currentCountdownValue.ToString(CultureInfo.InvariantCulture);
        }

        private void Update()
        {
            if (!timerActive)
            {
                return;
            }

            if (currentInterval > 0)
            {
                currentInterval -= Time.deltaTime;
                return;
            }

            currentInterval += 1;

            currentCountdownValue -= 1;
            timerText.text = currentCountdownValue.ToString(CultureInfo.InvariantCulture);

            if (currentCountdownValue <= 0)
            {
                timerActive = false;
                instructionsGuiHandler.DisableArrows();
                panelAnimator.HidePanel();
            }
        }
    }
}
