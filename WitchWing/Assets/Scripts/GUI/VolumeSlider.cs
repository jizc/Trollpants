// <copyright file="VolumeSlider.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System;
    using Data;
    using Environment;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioType audioType;

        private Slider slider;
        private bool ignoreValueChanged;

        private enum AudioType
        {
            Music,
            Sfx
        }

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            ignoreValueChanged = true;
            switch (audioType)
            {
                case AudioType.Music:
                    slider.value = PlayerSettings.MusicVolume;
                    break;
                case AudioType.Sfx:
                    slider.value = PlayerSettings.SfxVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ignoreValueChanged = false;
        }

        private void OnValueChanged(float volume)
        {
            if (ignoreValueChanged)
            {
                return;
            }

            switch (audioType)
            {
                case AudioType.Music:
                    AudioClipPlayer.SetMusicVolume(volume);
                    break;
                case AudioType.Sfx:
                    AudioClipPlayer.SetSfxVolume(volume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PlayerSettings.Save();
        }
    }
}
