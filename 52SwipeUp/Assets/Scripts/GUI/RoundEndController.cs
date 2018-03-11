// <copyright file="RoundEndController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using System;
    using System.Globalization;
    using UnityEngine;
    using UnityEngine.UI;
    using Random = UnityEngine.Random;

    [RequireComponent(typeof(PanelBucket))]
    [RequireComponent(typeof(RoundCoordinator))]
    public class RoundEndController : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text timeText;
        [SerializeField] private Text scoreText;
        [SerializeField] private ParticleSystem particles;

        private PanelAnimatorHelper roundResultPanel;
        private RoundCoordinator roundCoordinator;
        private int startScore;
        private int startTimeLeft;
        private int secondsLeft;
        private int currentScore;
        private float animationDuration;
        private float counter;
        private bool isAnimating;

        public void PrepareRoundEndCard(int playerScore, int timeLeft)
        {
            // Reset potentially non-default variables
            isAnimating = false;
            counter = 0f;

            // Get the session data, store it locally and update the score based on time left
            startScore = playerScore;
            currentScore = startScore;
            startTimeLeft = timeLeft;
            secondsLeft = startTimeLeft;

            timeText.text = secondsLeft.ToString(CultureInfo.InvariantCulture);
            scoreText.text = currentScore.ToString(CultureInfo.InvariantCulture);
        }

        public void RoundEndAnimation(float duration)
        {
            // Reset potentially non-default variables
            animationDuration = duration;

            // Activate the particle system
            particles.Play(true);

            // Animate the variables over the duration
            isAnimating = true;
        }

        private void Awake()
        {
            var panelBucket = GetComponent<PanelBucket>();
            roundResultPanel = panelBucket.RoundResults.GetComponent<PanelAnimatorHelper>();
            roundCoordinator = GetComponent<RoundCoordinator>();
            roundCoordinator.CurrentRoundChanged += OnCurrentRoundChanged;
        }

        private void Update()
        {
            if (!isAnimating)
            {
                return;
            }

            // Mathf.Lerp: t(0-1), so we adjust for duration.
            var rel = 1f / animationDuration;
            counter += Time.deltaTime;

            // Animate the variables
            var floatTime = Mathf.Lerp(startTimeLeft, 0f, counter * rel);

            if (floatTime <= secondsLeft)
            {
                secondsLeft = Mathf.FloorToInt(floatTime);
                currentScore = startScore + startTimeLeft - secondsLeft;

                scoreText.text = currentScore.ToString(CultureInfo.InvariantCulture);
                timeText.text = secondsLeft.ToString(CultureInfo.InvariantCulture);
            }

            if (Math.Abs(floatTime) < 0.00001f)
            {
#if DEBUG
                Debug.Log(gameObject.name + " is done animating.");
#endif
                particles.Stop(true);
                roundResultPanel.Invoke("HidePanel", 1f);
                isAnimating = false;
            }
        }

        private void OnCurrentRoundChanged(int round)
        {
            titleText.text = $"Round {round} End";
        }
    }
}
