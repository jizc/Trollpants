// <copyright file="AudioClipPlayer.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using DG.Tweening;
    using Player;
    using UnityEngine;
    using UnityEngine.Events;
    using Random = UnityEngine.Random;

    public class AudioClipPlayer : MonoBehaviour
    {
        private readonly List<AudioSource> sfxSources = new List<AudioSource>();

        [Header("Settings")]
        [SerializeField] private int audioSourceCount = 5;

        [Header("Boost clips")]
        [SerializeField] private List<AudioClip> boostStartClips;
        [SerializeField] private List<AudioClip> boostClips;
        [SerializeField] private List<AudioClip> boostEndClips;

        [Header("Currency clips")]
        [SerializeField] private List<AudioClip> coinClips;
        [SerializeField] private List<AudioClip> gemClips;
        [SerializeField] private List<AudioClip> potionClips;

        [Header("Misc clips")]
        [SerializeField] private List<AudioClip> deathClips;
        [SerializeField] private List<AudioClip> clickClips;
        [SerializeField] private List<AudioClip> upgradeClips;
        [SerializeField] private AudioClip checkpoint;
        [SerializeField] private AudioClip incorrect;

        [Header("Music clips")]
        [SerializeField] private List<AudioClip> inGameMusic;
        [SerializeField] private AudioClip menuMusic;

        private AudioSource menuSource;
        private AudioSource mainBoostSource;
        private AudioSource boostLoopSource1;
        private AudioSource boostLoopSource2;
        private int lastBoostSource = 2;
        private int boostClipIndex = -1;

        public static AudioClipPlayer Instance { get; private set; }

        private AudioSource NextBoostLoopSource
        {
            get
            {
                if (lastBoostSource == 2)
                {
                    lastBoostSource = 1;
                    return boostLoopSource1;
                }

                lastBoostSource = 2;
                return boostLoopSource2;
            }
        }

        public static void PlayBoostStart()
        {
            var boostStartClip = Instance.boostStartClips[Random.Range(0, Instance.boostStartClips.Count)];
            Instance.PlayAudioClip(boostStartClip, PlayerSettings.SfxVolume, false, Instance.mainBoostSource);
            Instance.StartCoroutine(Instance.BoostLoop(0f));
        }

        public static void PlayBoostEnd()
        {
            Instance.boostLoopSource1.Stop();
            Instance.boostLoopSource2.Stop();
            Instance.PlayRandomAudioClipFromList(Instance.boostEndClips, PlayerSettings.SfxVolume, false, Instance.mainBoostSource);
        }

        public static void PlayCoin()
        {
            Instance.PlayRandomAudioClipFromList(Instance.coinClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayGem()
        {
            Instance.PlayRandomAudioClipFromList(Instance.gemClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayPotion()
        {
            Instance.PlayRandomAudioClipFromList(Instance.potionClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayDeath()
        {
            Instance.PlayRandomAudioClipFromList(Instance.deathClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayUpgrade()
        {
            Instance.PlayRandomAudioClipFromList(Instance.upgradeClips, PlayerSettings.SfxVolume, false);
        }

        public static void PlayCheckpoint()
        {
            Instance.PlayAudioClip(Instance.checkpoint, PlayerSettings.SfxVolume, false);
        }

        public static void FadeFromMenuToInGameMusic(float duration = 0.5f)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(Instance.menuSource.DOFade(0f, duration * 0.5f));
            sequence.AppendCallback(() =>
            {
                Instance.menuSource.Stop();
                Instance.menuSource.volume = 0f;
                Instance.PlayRandomAudioClipFromList(Instance.inGameMusic, PlayerSettings.MusicVolume, true, Instance.menuSource);
            });
            sequence.Append(Instance.menuSource.DOFade(PlayerSettings.MusicVolume, duration * 0.5f));
        }

        public static void FadeOutInGameMusic(float duration, UnityAction onComplete = null)
        {
            Instance.menuSource.DOFade(0f, duration)
                               .SetEase(Ease.Linear)
                               .OnComplete(onComplete.SafeInvoke);
        }

        public static void FadeInMenuMusic(float duration, UnityAction onComplete = null)
        {
            Instance.menuSource.volume = 0f;
            PlayMenuMusic();
            Instance.menuSource.DOFade(1f, duration)
                                      .SetEase(Ease.Linear)
                                      .OnComplete(onComplete.SafeInvoke);
        }

        public static void PlayIncorrect()
        {
            Instance.PlayAudioClip(Instance.incorrect, PlayerSettings.SfxVolume, false);
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

            Instance.mainBoostSource.volume = volume;
            Instance.boostLoopSource1.volume = volume;
            Instance.boostLoopSource2.volume = volume;

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

            Instance.mainBoostSource.mute = mute;
            Instance.boostLoopSource1.mute = mute;
            Instance.boostLoopSource2.mute = mute;

            PlayerSettings.MuteSfx = mute;
        }

        public void PlayClick()
        {
            PlayRandomAudioClipFromList(clickClips, PlayerSettings.SfxVolume, false);
        }

        private static void PlayMenuMusic()
        {
            Instance.PlayAudioClip(Instance.menuMusic, PlayerSettings.MusicVolume, true, Instance.menuSource);
        }

        private static int GetRandomIndex(int count, int excludeIndex)
        {
            if (count == 1 && excludeIndex == 0)
            {
                throw new ArgumentException();
            }

            var index = excludeIndex;
            while (index == excludeIndex)
            {
                index = Random.Range(0, count);
            }

            return index;
        }

        private IEnumerator BoostLoop(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (Player.IsBoosting)
            {
                var startTime = 1f;
                if (boostClipIndex == -1)
                {
                    startTime = 0f;
                }

                boostClipIndex = GetRandomIndex(boostClips.Count, boostClipIndex);
                var boostClip = boostClips[boostClipIndex];
                var nextDelay = (boostClip.length - 1f) * 0.5f;

                PlayAudioClip(boostClip, PlayerSettings.SfxVolume, false, NextBoostLoopSource, startTime);
                StartCoroutine(BoostLoop(nextDelay));
            }
            else
            {
                boostClipIndex = -1;
            }
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

            for (var i = 0; i < audioSourceCount - 1; i++)
            {
                var source = CreateAudioSource();
                sfxSources.Add(source);
            }

            menuSource = CreateAudioSource("MusicSource");
            mainBoostSource = CreateAudioSource("MainBoostSource");
            boostLoopSource1 = CreateAudioSource("BoostLoopSource1");
            boostLoopSource2 = CreateAudioSource("BoostLoopSource2");
        }

        private void Start()
        {
            menuSource.mute = PlayerSettings.MuteMusic;
            menuSource.volume = PlayerSettings.MusicVolume;
            foreach (var source in sfxSources)
            {
                source.mute = PlayerSettings.MuteSfx;
                source.volume = PlayerSettings.SfxVolume;
            }

            mainBoostSource.mute = PlayerSettings.MuteSfx;
            boostLoopSource1.mute = PlayerSettings.MuteSfx;
            boostLoopSource2.mute = PlayerSettings.MuteSfx;
            mainBoostSource.volume = PlayerSettings.SfxVolume;
            boostLoopSource1.volume = PlayerSettings.SfxVolume;
            boostLoopSource2.volume = PlayerSettings.SfxVolume;

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
