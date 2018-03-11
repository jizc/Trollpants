// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AudioManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        public AudioSource pickupAudio;
        public AudioSource PowerupAudio;

        public List<AudioClip> powerupActivateSounds;
        public List<AudioClip> powerupEndSounds;
        public List<AudioClip> coinSounds;
        public List<AudioClip> gemSounds;

        private void OnEnable()
        {
            Events.instance.AddListener<CoinsPickedUp>(OnCoinsPickedUp);
            Events.instance.AddListener<PowerupEnded>(OnPowerupEnded);
            Events.instance.AddListener<PowerupPickup>(OnPowerupPickup);
        }

        private void OnCoinsPickedUp(CoinsPickedUp coinsPickedUpEvent)
        {
            pickupAudio.clip = coinsPickedUpEvent.coinsPickedUp == 1
                ? coinSounds[Random.Range(0, coinSounds.Count)]
                : gemSounds[Random.Range(0, gemSounds.Count)];
            pickupAudio.Play();
        }

        private void OnPowerupPickup(PowerupPickup onPowerupPickupEvent)
        {
            PowerupAudio.clip = powerupActivateSounds[(int)(onPowerupPickupEvent.powerup - 1)];
            PowerupAudio.Play();
        }

        private void OnPowerupEnded(PowerupEnded onPowerupEndedEvent)
        {
            if (onPowerupEndedEvent.powerupType == Enums.PowerupType.None ||
                onPowerupEndedEvent.endedSoAnotherCouldBegin)
            {
                return;
            }

            PowerupAudio.clip = powerupEndSounds[(int)(onPowerupEndedEvent.powerupType - 1)];
            PowerupAudio.Play();
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<CoinsPickedUp>(OnCoinsPickedUp);
            Events.instance.RemoveListener<PowerupPickup>(OnPowerupPickup);
            Events.instance.RemoveListener<PowerupEnded>(OnPowerupEnded);
        }
    }
}
