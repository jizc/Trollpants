// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.Audio;

    /// <summary>
    /// All sounds that can be played on an event
    /// </summary>
    public enum Sound
    {
        None,
        Death,
        Checkpoint,
        SideBoost,
        MainBoost,
        Button,
        Score,
        Chatter,
        Sirens,
        CopDeath
    }

    public enum Music
    {
        None,
        Menu,
        Playing
    }

    public enum SoundType
    {
        Music,
        Effect
    }

    public class SoundManager : MonoBehaviour
    {
        private static SoundManager s_instance;

        public AudioMixer gameMixer;
        public AudioMixerGroup master;
        public AudioMixerGroup music;
        public AudioMixerGroup sfx;

        public AudioSource sideBoostSource;
        public AudioSource checkpointSource;
        public AudioSource deathSource;
        public AudioSource musicSource;
        public AudioSource scoreSource;
        public AudioSource buttonSource;
        public AudioSource chatterSource;
        public AudioSource sirenSource;
        public AudioSource copDeathSource;

        public AudioClip[] scoreSounds;
        public AudioClip[] buttonSounds;
        public AudioClip[] sideBoostSounds;
        public AudioClip[] checkpointSounds;
        public AudioClip[] musicTracks;
        public AudioClip[] copDeathSounds;

        public static SoundManager Instance
        {
            get { return s_instance; }
            set
            {
                if (s_instance != null)
                {
                    return;
                }

                s_instance = value;
            }
        }

        public void Init()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void MuteSound(bool isMuted)
        {
            gameMixer.SetFloat("MasterVol", isMuted ? -80f : 0f);
            CloudVariables.IsMuted = isMuted;
        }

        public void PlaySound(Sound sound)
        {
            switch (sound)
            {
                case Sound.None:
                    break;
                case Sound.Death:
                    if (!deathSource.isPlaying)
                    {
                        deathSource.Play();
                    }

                    break;
                case Sound.Checkpoint:
                    checkpointSource.clip = checkpointSounds[Random.Range(0, 3)];
                    if (!checkpointSource.isPlaying)
                    {
                        checkpointSource.Play();
                    }

                    break;
                case Sound.MainBoost:
                    break;
                case Sound.SideBoost:
                    break;
                case Sound.Button:
                    buttonSource.clip = buttonSounds[Random.Range(0, 3)];
                    if (!buttonSource.isPlaying)
                    {
                        buttonSource.Play();
                    }

                    break;
                case Sound.Score:
                    scoreSource.clip = scoreSounds[Random.Range(0, 6)];
                    if (!scoreSource.isPlaying)
                    {
                        scoreSource.Play();
                    }

                    break;
                case Sound.Chatter:
                    if (!scoreSource.isPlaying)
                    {
                        scoreSource.Play();
                    }

                    break;
                case Sound.Sirens:
                    if (!sirenSource.isPlaying)
                    {
                        sirenSource.Play();
                    }

                    break;
                case Sound.CopDeath:
                    copDeathSource.clip = copDeathSounds[Random.Range(0, 4)];
                    copDeathSource.Play();
                    break;
            }
        }

        public void StopSound(Sound sound)
        {
            switch (sound)
            {
                case Sound.None:
                    break;
                case Sound.Death:
                    deathSource.Stop();
                    break;
                case Sound.Checkpoint:
                    checkpointSource.Stop();
                    break;
                case Sound.MainBoost:
                    break;
                case Sound.SideBoost:
                    break;
                case Sound.Button:
                    buttonSource.Stop();
                    break;
                case Sound.Score:
                    scoreSource.Stop();
                    break;
                case Sound.Chatter:
                    chatterSource.Stop();
                    break;
                case Sound.Sirens:
                    sirenSource.Stop();
                    break;
                case Sound.CopDeath:
                    copDeathSource.Stop();
                    break;
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            if (CloudVariables.IsMuted)
            {
                MuteSound(true);
            }

            var rand = Random.Range(0, 10);
            musicSource.clip = rand < 7 ? musicTracks[0] : musicTracks[1];
            musicSource.Play();
            musicSource.loop = true;
        }
    }
}
