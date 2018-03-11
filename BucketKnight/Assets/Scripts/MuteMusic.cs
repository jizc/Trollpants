// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MuteMusic.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class MuteMusic : MonoBehaviour
    {
        private bool _settingschangedThisFrame;
        private AudioSource _audioSource;

        private void Start()
        {
            _settingschangedThisFrame = false;
            _audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

            if (SavedData.SoundToggledOn)
            {
                _audioSource.Play();
            }
        }

        private void Update()
        {
            if (_settingschangedThisFrame != SavedData.SoundToggledOn)
            {
                if (!SavedData.SoundToggledOn)
                {
                    _audioSource.mute = true;
                }

                if (SavedData.SoundToggledOn)
                {
                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.Play();
                    }

                    _audioSource.mute = false;
                }
            }
            _settingschangedThisFrame = SavedData.SoundToggledOn;
        }
    }
}
