// <copyright file="PlayerEffects.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Player
{
    using UnityEngine;

    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem trailParticleSystem;
        [SerializeField] private ParticleSystem boostParticleSystem;
        [SerializeField] private ParticleSystem deathParticleSystem;
        [SerializeField] private Color reviveParticlesColor;

        private ParticleSystem.MinMaxGradient cachedDeathParticlesColor;

        public void EnableBoostTrail()
        {
            trailParticleSystem.Stop();
            boostParticleSystem.Play();
        }

        public void EnableNormalTrail()
        {
            boostParticleSystem.Stop();
            trailParticleSystem.Play();
        }

        public void PlayCrashAnimation()
        {
            var mainModule = deathParticleSystem.main;
            mainModule.startColor = cachedDeathParticlesColor;
            deathParticleSystem.Play(true);
        }

        public void PlayResurrectionAnimation()
        {
            var mainModule = deathParticleSystem.main;
            mainModule.startColor = reviveParticlesColor;
            deathParticleSystem.Play(true);
        }

        public void DisableTrail()
        {
            trailParticleSystem.Stop();
            boostParticleSystem.Stop();
        }

        private void Awake()
        {
            cachedDeathParticlesColor = deathParticleSystem.main.startColor;
            EnableNormalTrail();
        }
    }
}
