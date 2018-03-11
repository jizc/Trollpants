// <copyright file="AudioClipPlayer.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Audio
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using UnityEngine;

    public class AudioClipPlayer : MonoBehaviour
    {
        private readonly List<AudioSource> sfxSources = new List<AudioSource>();

        [Header("Settings")]
        [SerializeField] private int audioSourceCount = 5;

        [Header("SFX clips")]
        [SerializeField] private List<AudioClip> clickClips;
        [SerializeField] private List<AudioClip> swipeClips;
        [SerializeField] private List<AudioClip> errorSwipeClips;
        [SerializeField] private AudioClip countdownClip;

        [Header("Music clips")]
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip inGameMusic;

        private AudioSource menuSource;

        public static AudioClipPlayer Instance { get; private set; }

        public static void PlaySwipe()
        {
            Instance.PlayRandomAudioClipFromList(Instance.swipeClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlaySwipeError()
        {
            Instance.PlayRandomAudioClipFromList(Instance.errorSwipeClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayCountdown()
        {
            Instance.PlayAudioClip(Instance.countdownClip, PlayerSettings.SfxVolume, false);
        }

        public static void StopCountdown()
        {
            Instance.StopAudioClip(Instance.countdownClip);
        }

        public static void FadeFromMenuToInGameMusic(float duration = 0.5f)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(Instance.menuSource.DOFade(0f, duration * 0.5f));
            sequence.AppendCallback(() =>
            {
                Instance.menuSource.Stop();
                Instance.menuSource.volume = 0f;
                Instance.PlayAudioClip(Instance.inGameMusic, PlayerSettings.MusicVolume, true, Instance.menuSource);
            });
            sequence.Append(Instance.menuSource.DOFade(PlayerSettings.MusicVolume, duration * 0.5f));
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

        public void PlayClick()
        {
            PlayRandomAudioClipFromList(clickClips, PlayerSettings.SfxVolume, false);
        }

        public void FadeFromInGameMusicToMenuMusic()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(menuSource.DOFade(0f, 0.25f));
            sequence.AppendCallback(() =>
            {
                menuSource.Stop();
                menuSource.volume = 0f;
                PlayAudioClip(menuMusic, PlayerSettings.MusicVolume, true, menuSource);
            });
            sequence.Append(menuSource.DOFade(PlayerSettings.MusicVolume, 0.25f));
        }

        private static void PlayMenuMusic()
        {
            Instance.PlayAudioClip(Instance.menuMusic, PlayerSettings.MusicVolume, true, Instance.menuSource);
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

        private void StopAudioClip(AudioClip audioClip)
        {
            var source = sfxSources.FirstOrDefault(s => s.clip == audioClip);
            if (source != null)
            {
                source.Stop();
            }
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

            PlayMenuMusic();
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
