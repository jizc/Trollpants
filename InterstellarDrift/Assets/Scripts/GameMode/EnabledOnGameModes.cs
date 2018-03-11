// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnabledOnGameModes.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using Mode = GameMode.Mode;

    public class EnabledOnGameModes : MonoBehaviour
    {
        public Mode[] Modes = { Mode.Standard };

        private void Awake()
        {
            foreach (var mode in Modes)
            {
                if (GameMode.Instance.CurrentMode == mode)
                {
                    continue;
                }

                gameObject.SetActive(false);
            }
        }
    }
}
