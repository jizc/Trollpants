// <copyright file="HudViewModel.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.GUI
{
    using Data;
    using UnityEngine;
    using UnityEngine.UI;

    public class HudViewModel : MonoBehaviour
    {
        [SerializeField] private SessionData sessionData;
        [SerializeField] private Text cherriesText;
        [SerializeField] private Text scoreText;

        private void Awake()
        {
            sessionData.CherriesThisRunChanged += OnCherriesThisRunChanged;
            sessionData.ScoreChanged += OnScoreChanged;
        }

        private void OnCherriesThisRunChanged(int cherries)
        {
            cherriesText.text = cherries.ToString();
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}
