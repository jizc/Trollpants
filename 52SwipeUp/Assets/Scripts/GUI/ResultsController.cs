// <copyright file="ResultsController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using System.Globalization;
    using UnityEngine;
    using UnityEngine.UI;

    public class ResultsController : MonoBehaviour
    {
        [SerializeField] private SessionData sessionData;
        [SerializeField] private ParticleSystem rightStream;
        [SerializeField] private ParticleSystem leftStream;
        [SerializeField] private Text scoreCountText;
        [SerializeField] private Text streakCountText;
        [SerializeField] private GameObject newBestScore;
        [SerializeField] private GameObject newBestStreak;

        private float countDuration;
        private float timeCounter;
        private int playerScore;
        private float currentScore;
        private int playerStreak;
        private float currentStreak;
        private bool isAnimating;

        // 1. Get the score from sessiondata
        // 2. Get the streak from sessiondata
        // 3. Check if it's a new record in any of the two
        // 4. Start counting up the score, activating the particle systems
        // 5. When counting is done, deactivate particles
        // 6. Show a "New Best" something tag on any that deserve it
        public void BeginResultsAnimation(float countUpDuration)
        {
            // Set variables used in animation
            countDuration = countUpDuration;
            playerScore = sessionData.Score;
            playerStreak = sessionData.HighestStreakInSession;

            // Activate particle systems
            leftStream.Play(true);
            rightStream.Play(true);

            // Allow animation to run
            isAnimating = true;
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Initialize();
                BeginResultsAnimation(3f);
            }
#endif

            if (!isAnimating)
            {
                return;
            }

            // Adjustment for time since Lerp-function clamps at 0-1
            var rel = 1f / countDuration;

            // Lerp the score from 0 to total, convert to int floored and set text
            currentScore = Mathf.Lerp(0f, playerScore, timeCounter * rel);
            var scoreAsInt = Mathf.FloorToInt(currentScore);
            scoreCountText.text = scoreAsInt.ToString(CultureInfo.InvariantCulture);

            // Lerp the streak from 0 to total, convert to int floored and set text
            currentStreak = Mathf.Lerp(0f, playerStreak, timeCounter * rel);
            var streakAsInt = Mathf.FloorToInt(currentStreak);
            streakCountText.text = streakAsInt.ToString(CultureInfo.InvariantCulture);

            // When totals are reached, stop animating and activate tags
            if (currentScore >= playerScore && currentStreak >= playerStreak)
            {
                isAnimating = false;

                // Stop the particle systems
                leftStream.Stop();
                rightStream.Stop();

                // Show the "New High Score" tag if relevant
                if (sessionData.NewHighScoreSet)
                {
                    newBestScore.SetActive(true);
                }
            }

            // Increment counter
            timeCounter += Time.deltaTime;
        }

        private void Initialize()
        {
            // Set default states on objects and variables
            isAnimating = false;
            rightStream.Stop(true);
            leftStream.Stop(true);

            newBestScore.SetActive(false);
            currentScore = 0f;
            scoreCountText.text = string.Empty;

            newBestStreak.SetActive(false);
            currentStreak = 0f;
            streakCountText.text = string.Empty;

            timeCounter = 0f;
        }
    }
}
