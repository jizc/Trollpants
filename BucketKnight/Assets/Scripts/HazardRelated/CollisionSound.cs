// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollisionSound.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class CollisionSound : MonoBehaviour
    {
        public AudioClip CollisionSoundToPlay;
        public AudioSource source;
        public float minPitch = 0.5f;
        public float maxPitch = 1;

        public void MakeSoundWithPitch()
        {
            source.clip = CollisionSoundToPlay;
            source.pitch = Random.Range(minPitch, maxPitch);
            source.Play();
        }

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }
    }
}
