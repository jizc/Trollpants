// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParticleHitBehaviour.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    public class ParticleHitBehaviour : ParticleBehaviour
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Events.instance.AddListener<PlayerDamaged>(OnPlayerHit);
        }

        private void OnPlayerHit(PlayerDamaged e)
        {
            PlayParticles();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Events.instance.RemoveListener<PlayerDamaged>(OnPlayerHit);
        }
    }
}
