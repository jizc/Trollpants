// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameModeSetter.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using Mode = GameMode.Mode;

    public class GameModeSetter : MonoBehaviour
    {
        public Mode TargetMode = Mode.Standard;

        public void SetMode()
        {
            GameMode.Instance.CurrentMode = TargetMode;
        }
    }
}
