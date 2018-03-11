// <copyright file="SessionData.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class SessionData : MonoBehaviour
    {
        private int lives;
        private float timeLeft;
        private int score;

        public event UnityAction<int> LivesChanged;
        public event UnityAction<float> TimeLeftChanged;
        public event UnityAction<int> ScoreChanged;

        public int Lives
        {
            get { return lives; }
            set
            {
                if (lives != value)
                {
                    lives = value;
                    LivesChanged?.Invoke(value);
                }
            }
        }

        public float TimeLeft
        {
            get { return timeLeft; }
            set
            {
                if (Math.Abs(timeLeft - value) > 0.01f)
                {
                    timeLeft = (float)Math.Round(value, 1);
                    TimeLeftChanged?.Invoke(value);
                }
            }
        }

        public int Score
        {
            get { return score; }
            set
            {
                if (score != value)
                {
                    score = value;
                    ScoreChanged?.Invoke(value);
                }
            }
        }

        public bool NewHighScoreSet { get; set; }
        public bool IsOnStreak { get; set; }
        public int CurrentStreak { get; set; }
        public int HighestStreakInSession { get; set; }

        public void ResetProperties()
        {
            Lives = 4;
            TimeLeft = 30f;
            Score = 0;
            NewHighScoreSet = false;
            IsOnStreak = false;
            CurrentStreak = 0;
            HighestStreakInSession = 0;
        }
    }
}
