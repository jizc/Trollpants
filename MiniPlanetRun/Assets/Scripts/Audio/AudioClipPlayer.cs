// <copyright file="AudioClipPlayer.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.Audio
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using DG.Tweening;
    using UnityEngine;

    public class AudioClipPlayer : MonoBehaviour
    {
        private readonly List<AudioSource> sfxSources = new List<AudioSource>();

        [Header("Settings")]
        [SerializeField] private int audioSourceCount = 5;

        [Header("SFX clips")]
        [SerializeField] private List<AudioClip> buttonClips;
        [SerializeField] private List<AudioClip> cherryClips;
        [SerializeField] private List<AudioClip> deathClips;
        [SerializeField] private AudioClip newCharacterClip;
        [SerializeField] private AudioClip scoreFillClip;

        [Header("Music clips")]
        [SerializeField] private AudioClip musicClip;

        private AudioSource menuSource;

        public static AudioClipPlayer Instance { get; private set; }

        public static void PlayButton()
        {
            Instance.PlayRandomAudioClipFromList(Instance.buttonClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayCherry()
        {
            Instance.PlayRandomAudioClipFromList(Instance.cherryClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayDeath()
        {
            Instance.PlayRandomAudioClipFromList(Instance.deathClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayNewCharacter()
        {
            Instance.PlayAudioClip(Instance.newCharacterClip, PlayerSettings.SfxVolume, false);
        }

        public static void PlayScoreFill()
        {
            Instance.PlayAudioClip(Instance.scoreFillClip, PlayerSettings.SfxVolume, false);
        }

        public static void SetMusicVolume(float volume)
        {
            Instance.menuSource.volume = volume;
            PlayerSettings.MusicVolume = volume;
        }

        public static void SetSfxVolume(float volume)
        {
            foreach (var source in Instance.sfxSources)
            {
                source.volume = volume;
            }

            PlayerSettings.SfxVolume = volume;
        }

        public static void MuteMusic(bool mute)
        {
            Instance.menuSource.mute = mute;
            PlayerSettings.MuteMusic = mute;
        }

        public static void MuteSfx(bool mute)
        {
            foreach (var source in Instance.sfxSources)
            {
                source.mute = mute;
            }

            PlayerSettings.MuteSfx = mute;
        }

        private static void FadeInMusic(float duration)
        {
            Instance.menuSource.volume = 0f;
            Instance.PlayAudioClip(Instance.musicClip, PlayerSettings.MusicVolume, true, Instance.menuSource);
            Instance.menuSource.DOFade(PlayerSettings.MusicVolume, duration).SetEase(Ease.Linear);
        }

        private void PlayRandomAudioClipFromList(IList<AudioClip> audioClips, float volume, bool loop, AudioSource audioSource = null)
        {
            PlayAudioClip(audioClips[Random.Range(0, audioClips.Count)], volume, loop, audioSource);
        }

        private void PlayAudioClip(AudioClip audioClip, float volume, bool loop, AudioSource audioSource = null, float startTime = 0f)
        {
            if (audioSource == null)
            {
                audioSource = sfxSources.FirstOrDefault(source => !source.isPlaying);
                if (audioSource == null)
                {
                    Debug.LogWarning("Can't play AudioClip, no available sources.");
                    return;
                }
            }
            else if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.time = startTime;
            audioSource.loop = loop;
            audioSource.Play();
        }

        private void Awake()
        {
            Instance = this;
            PlayerSettings.Load();

            for (var i = 0; i < audioSourceCount - 1; i++)
            {
                var source = CreateAudioSource();
                source.volume = PlayerSettings.SfxVolume;
                source.mute = PlayerSettings.MuteSfx;
                sfxSources.Add(source);
            }

            menuSource = CreateAudioSource("MusicSource");
            menuSource.mute = PlayerSettings.MuteMusic;

            FadeInMusic(0.5f);
        }

        private AudioSource CreateAudioSource(string sourceName = "AudioSource")
        {
            var audioSourceObject = new GameObject(sourceName, typeof(AudioSource));
            audioSourceObject.transform.SetParent(transform);
            var audioSource = audioSourceObject.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            return audioSource;
        }
    }
}
