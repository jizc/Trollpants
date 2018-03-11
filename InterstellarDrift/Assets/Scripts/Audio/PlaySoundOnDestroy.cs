// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaySoundOnDestroy.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class PlaySoundOnDestroy : MonoBehaviour
    {
        public Sound SoundOnDestroy = Sound.Death;

        private void OnDestroy()
        {
            if (SoundManager.Instance)
            {
                SoundManager.Instance.PlaySound(SoundOnDestroy);
            }
        }
    }
}
