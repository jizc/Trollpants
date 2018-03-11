// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AudioManagerScript.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioManagerScript : MonoBehaviour
    {
        public AudioMixerSnapshot underwater;
        public AudioMixerSnapshot overerwater;

        public void SetUnderwater()
        {
            underwater.TransitionTo(0.1f);
        }

        public void SetOverWater()
        {
            overerwater.TransitionTo(0.1f);
        }

        private void Awake()
        {
            SetOverWater();
        }
    }
}
