// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameOverScoreText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public class GameOverScoreText : MonoBehaviour
    {
        private int _score, _highScore;

        private void OnEnable()
        {
            _score = GameObject.Find("Player").GetComponent<PlayerStats>().Score;

            transform.Find("ScoreText").GetComponent<Text>().text = _score.ToString();
            transform.Find("HighScoreText").GetComponent<Text>().text = SavedData.HighScore.ToString();
        }
    }
}
