// <copyright file="ScoreDisplay.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreDisplay : MonoBehaviour
    {
        private int targetScore;
        private Text scoreText;
        private IncrementHerder incrementHerder;

        public int DisplayedScore { get; private set; }

        public void Init()
        {
            DOTween.Clear(true);
            DOTween.Init();
            DOTween.Clear(true);

            scoreText = transform.GetComponent<Text>();

            incrementHerder = transform.GetComponentInChildren<IncrementHerder>();
            incrementHerder.Init();

            ResetScoreText();
        }

        public void IncreaseScore(int amount, bool animateIncrement)
        {
            // Show the score-text
            scoreText.gameObject.SetActive(true);

            // Set new target score
            targetScore += amount;

            // Start tweening to new target score from current displayed score
            DOTween.To(() => DisplayedScore, x => DisplayedScore = x, targetScore, .5f).OnUpdate(() => scoreText.text = string.Empty + DisplayedScore);

            // Show increase amount with increment popup
            if (animateIncrement)
            {
                incrementHerder.Increment(amount);
            }
        }

        public void DecreaseScore(int amount, bool animateDecrement)
        {
            // Show the score-text
            scoreText.gameObject.SetActive(true);

            // Set new target score, assert that new score is not below zero
            if (targetScore - amount < 0)
            {
                targetScore = 0;
            }
            else
            {
                targetScore -= amount;
            }

            // Start tweening to new target score from current displayed score
            DOTween.To(() => DisplayedScore, x => DisplayedScore = x, targetScore, .5f).OnUpdate(() => scoreText.text = string.Empty + DisplayedScore);

            // Show decrease with decrement popdown
            if (animateDecrement)
            {
                incrementHerder.Decrement(amount);
            }
        }

        private void ResetScoreText()
        {
            DisplayedScore = 0;
            targetScore = 0;
            scoreText.text = string.Empty;
            scoreText.gameObject.SetActive(false);
        }
    }
}
