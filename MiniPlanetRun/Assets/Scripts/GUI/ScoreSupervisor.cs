// <copyright file="ScoreSupervisor.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.GUI
{
    using Audio;
    using CloudOnce;
    using Data;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(SessionData))]
    public class ScoreSupervisor : MonoBehaviour
    {
        [SerializeField] private Text scoreNumerics;
        [SerializeField] private Text highScoreNumerics;
        [SerializeField] private Text cherriesText;

        private SessionData sessionData;
        private bool animateScore;
        private float targetScore;
        private float currentScore;
        private float currentVelocity;

        public void Init()
        {
            scoreNumerics.text = string.Empty + 0;
            highScoreNumerics.text = string.Empty + 0;

            targetScore = 0f;
            currentScore = 0f;
            currentVelocity = 0f;
            animateScore = false;
        }

        public void SetSessionScore()
        {
            targetScore = sessionData.Score;
            animateScore = true;

            if (targetScore > 0)
            {
                AudioClipPlayer.PlayScoreFill();
            }

#if DEBUG
            Debug.Log("Setting session score to " + targetScore);
#endif
        }

        public void SetHighScore()
        {
            var highScore = CloudVariables.HighScore;

            highScoreNumerics.text = highScore.ToString();

#if DEBUG
            Debug.Log("Setting highscore to " + highScore);
#endif
        }

        private void Awake()
        {
            sessionData = GetComponent<SessionData>();
            sessionData.CherriesChanged += OnCherriesChanged;
            OnCherriesChanged(sessionData.Cherries);
        }

        private void Update()
        {
            if (!animateScore)
            {
                return;
            }

            currentScore = Mathf.SmoothDamp(currentScore, targetScore, ref currentVelocity, 0.5f);
            var currentToInt = Mathf.RoundToInt(currentScore);
            scoreNumerics.text = string.Empty + currentToInt;
        }

        private void OnCherriesChanged(int count)
        {
            cherriesText.text = count.ToString();
        }
    }
}
