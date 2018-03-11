// <copyright file="GameState.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Data
{
    using UnityEngine.Events;

    public static class GameState
    {
        private static int s_coinValue = 1;
        private static int s_difficultyLevel = 1;
        private static bool s_isPaused = true;

        public static event UnityAction<bool> IsPausedChanged;

        public static bool IsPaused
        {
            get
            {
                return s_isPaused;
            }

            set
            {
                if (s_isPaused == value)
                {
                    return;
                }

                s_isPaused = value;
                IsPausedChanged.SafeInvoke(value);
            }
        }

        public static int DifficultyLevel
        {
            get { return s_difficultyLevel; }
            set { s_difficultyLevel = value; }
        }

        public static int CoinValue
        {
            get { return s_coinValue; }
            set { s_coinValue = value; }
        }
    }
}
