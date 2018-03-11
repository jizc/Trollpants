// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundMusicManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.Audio;

    public class BackgroundMusicManager : MonoBehaviour
    {
        public AudioSource backgroundMusic;
        public AudioSource backgroundSound;

        public AudioMixer masterMixer;

        private void OnEnable()
        {
            Events.instance.AddListener<SoundToggled>(OnSoundToggled);
        }

        private void Awake()
        {
            if (SavedData.SoundToggledOn)
            {
                masterMixer.SetFloat("MasterVolume", 0);
                backgroundMusic.Play();
                backgroundSound.Play();
            }
            else
            {
                masterMixer.SetFloat("MasterVolume", -80);
            }
        }

        public void ToggleBackgroundMusic()
        {
            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
            else
            {
                backgroundMusic.Play();
            }
        }

        public void ToggleBackgroundSound()
        {
            if (backgroundSound.isPlaying)
            {
                backgroundSound.Stop();
            }
            else
            {
                backgroundSound.Play();
            }
        }

        private void OnSoundToggled(SoundToggled onSoundToggledEvent)
        {

            if (onSoundToggledEvent.toggledOn)
            {
                masterMixer.SetFloat("MasterVolume", 0);
                backgroundMusic.Play();
                backgroundSound.Play();
            }
            else
            {
                masterMixer.SetFloat("MasterVolume", -80);
                backgroundMusic.Stop();
                backgroundSound.Stop();
            }
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<SoundToggled>(OnSoundToggled);
        }
    }
}
