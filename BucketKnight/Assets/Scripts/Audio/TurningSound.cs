// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TurningSound.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using UnityEngine;
    using UnityEngine.Audio;

    public class TurningSound : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioMixer mixer;

        public void setVolume(float volume)
        {
            volume = Math.Abs(volume);
            volume *= 100;
            volume = -40 + volume * 1.5f;

            if (volume > -16)
            {
                volume = -16;
            }

            mixer.SetFloat("TurningSound", volume);
        }
    }
}
