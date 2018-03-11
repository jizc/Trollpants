// <copyright file="Hud.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class Hud : MonoBehaviour
    {
        private readonly Gradient gradient = new Gradient();
        private readonly GradientColorKey[] gradientColorKeys = new GradientColorKey[3];
        private readonly GradientAlphaKey[] gradientAlphaKeys = new GradientAlphaKey[2];

        [SerializeField] private SessionData sessionData;
        [SerializeField] private Image livesBackground;
        [SerializeField] private Text livesCount;
        [SerializeField] private Color green;
        [SerializeField] private Color yellow;
        [SerializeField] private Color red;
        [SerializeField] private Text timeLeftText;
        [SerializeField] private Text scoreText;

        private void Awake()
        {
            gradientColorKeys[0].color = red;
            gradientColorKeys[0].time = 0.25f;
            gradientColorKeys[1].color = yellow;
            gradientColorKeys[1].time = 0.5f;
            gradientColorKeys[2].color = green;
            gradientColorKeys[2].time = 1f;

            gradientAlphaKeys[0].alpha = 1f;
            gradientAlphaKeys[0].time = 0f;
            gradientAlphaKeys[1].alpha = 1f;
            gradientAlphaKeys[1].time = 1f;

            gradient.SetKeys(gradientColorKeys, gradientAlphaKeys);
        }

        private void Start()
        {
            sessionData.LivesChanged += OnLivesChange;
            sessionData.TimeLeftChanged += OnTimeLeftChanged;
            sessionData.ScoreChanged += OnScoreChanged;
            OnLivesChange(4);
        }

        private void OnLivesChange(int lives)
        {
            livesCount.text = lives.ToString();
            var fillAmount = lives / 4f;
            livesBackground.fillAmount = fillAmount;
            livesBackground.color = gradient.Evaluate(fillAmount);
        }

        private void OnTimeLeftChanged(float timeLeft)
        {
            timeLeftText.text = ((int)timeLeft).ToString();
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}
