// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sounds.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class Sounds : MonoBehaviour
    {
        public AudioClip defaultButton;
        public AudioClip buyEquipment;
        public AudioClip sellEquipment;
        public AudioClip unlockEquipment;
        public AudioClip enterShop;
        public AudioSource audioSource;

        public void DefaultButtonClick()
        {
            audioSource.clip = defaultButton;
            RandomizePitchAndPlay(0.5f);
        }

        public void BuyEquipmentButtonClick()
        {
            audioSource.clip = buyEquipment;
            RandomizePitchAndPlay();
        }

        public void SellEquipmentButtonClick()
        {
            audioSource.clip = sellEquipment;
            RandomizePitchAndPlay(0.9f);
        }

        public void UnlockEquipmentButtonClick()
        {
            audioSource.clip = unlockEquipment;
            RandomizePitchAndPlay();
        }

        public void EnterShopButtonClick()
        {
            audioSource.clip = enterShop;
            RandomizePitchAndPlay();
        }

        private void RandomizePitchAndPlay(float min = 1f, float max = 1f)
        {
            audioSource.pitch = Random.Range(min, max);
            audioSource.Play();
        }
    }
}
