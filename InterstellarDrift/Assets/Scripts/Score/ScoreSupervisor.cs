// <copyright file="ScoreSupervisor.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Defines methods for increasing and decreasing score.
    /// </summary>
    public class ScoreSupervisor : MonoBehaviour
    {
        private static ScoreSupervisor s_instance;

        private ScoreDisplay scoreDisplay;
        private bool isInitialized;

        public static ScoreSupervisor Instance
        {
            get { return s_instance; }
            set
            {
                if (s_instance != null)
                {
                    return;
                }

                s_instance = value;
            }
        }

        public static bool Exists => s_instance != null;

        public void Init()
        {
            if (isInitialized)
            {
                return;
            }

            Instance = this;

            scoreDisplay = GameObject.FindWithTag("ScoreDisplay").GetComponent<ScoreDisplay>();
            scoreDisplay.Init();

            // Display the score 0 at the beginning
            scoreDisplay.IncreaseScore(0, false);

            isInitialized = true;
        }

        public void IncreaseScore(int amount, bool playSound)
        {
            if (TrackedData.Instance)
            {
                TrackedData.Instance.SessionData.Score += amount;
            }
            else
            {
                Debug.LogWarning("TrackedData-instance not found.");
            }

            scoreDisplay.IncreaseScore(amount, true);

            if (SoundManager.Instance && playSound)
            {
                SoundManager.Instance.PlaySound(Sound.Score);
            }
        }

        public void DecreaseScore(int amount)
        {
            if ((TrackedData.Instance.SessionData.Score - amount) < 0)
            {
                TrackedData.Instance.SessionData.Score = 0;
                return;
            }

            TrackedData.Instance.SessionData.Score -= amount;
            scoreDisplay.DecreaseScore(amount, true);
        }

        private void Awake()
        {
            Init();
        }
    }
}
