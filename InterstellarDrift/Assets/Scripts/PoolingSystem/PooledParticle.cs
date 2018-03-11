// <copyright file="PooledParticle.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    public class PooledParticle : MonoBehaviour, ISpawnable
    {
        [SerializeField] private float _activationDelayOnSpawned = 0.15f;

        private ParticleSystem cachedParticleSystem;

        void ISpawnable.OnSpawned(ObjectPooler pooler)
        {
            if (!cachedParticleSystem)
            {
#if DEBUG
                Debug.LogWarning("No cached particle system. Is this script on a gameobject with a particle system on it?");
#endif

                return;
            }

            Invoke("ActivateParticles", _activationDelayOnSpawned);
        }

        void ISpawnable.OnRecycled()
        {
            if (!cachedParticleSystem)
            {
#if DEBUG
                Debug.LogWarning("No cached particle system. Is this script on a gameobject with a particle system on it?");
#endif

                return;
            }

            cachedParticleSystem.Stop();
        }

        private void Awake()
        {
            cachedParticleSystem = GetComponent<ParticleSystem>();
        }

        private void ActivateParticles()
        {
            cachedParticleSystem.Play();
        }
    }
}
