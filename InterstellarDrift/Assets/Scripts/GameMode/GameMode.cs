// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameMode.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class GameMode : MonoBehaviour
    {
        private static GameMode s_instance;

        [SerializeField] private Mode _currentMode = Mode.Standard;

        public enum Mode
        {
            Standard,
            Time
        }

        public static GameMode Instance
        {
            get
            {
                return s_instance;
            }
            set
            {
                if (s_instance != null)
                {
                    return;
                }

                s_instance = value;
            }
        }

        public Mode CurrentMode
        {
            get { return _currentMode; }
            set { _currentMode = value; }
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}
