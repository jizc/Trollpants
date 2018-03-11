// <copyright file="ParticlesPlayOnEnable.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.World
{
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesPlayOnEnable : MonoBehaviour
    {
        private ParticleSystem cachedParticleSystem;

        private void Awake()
        {
            cachedParticleSystem = gameObject.GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            cachedParticleSystem.Play();
        }
    }
}
