// <copyright file="SessionData.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.Data
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.Events;

    public class SessionData : MonoBehaviour
    {
        private int score;
        private int cherriesThisRun;

        public event UnityAction<int> ScoreChanged;
        public event UnityAction<int> CherriesThisRunChanged;
        public event UnityAction<int> CherriesChanged;

        public static int UnlockedCharacters
        {
            get
            {
                var returnValue = 1;
                if (CloudVariables.Char2Unlocked)
                {
                    returnValue++;
                }

                if (CloudVariables.Char3Unlocked)
                {
                    returnValue++;
                }

                if (CloudVariables.Char4Unlocked)
                {
                    returnValue++;
                }

                if (CloudVariables.Char5Unlocked)
                {
                    returnValue++;
                }

                if (CloudVariables.Char6Unlocked)
                {
                    returnValue++;
                }

                return returnValue;
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

        public int CherriesThisRun
        {
            get { return cherriesThisRun; }
            set
            {
                if (cherriesThisRun != value)
                {
                    cherriesThisRun = value;
                    CherriesThisRunChanged?.Invoke(value);
                }
            }
        }

        public int Cherries
        {
            get { return CloudVariables.Cherries; }
            set
            {
                if (CloudVariables.Cherries != value)
                {
                    CloudVariables.Cherries = value;
                    CherriesChanged?.Invoke(value);
                }
            }
        }

        public void ResetProperties()
        {
            Score = 0;
            CherriesThisRun = 0;
        }
    }
}
