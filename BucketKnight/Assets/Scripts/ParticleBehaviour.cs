// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParticleBehaviour.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections;
    using UnityEngine;

    public class ParticleBehaviour : MonoBehaviour
    {
        public float timer;
        public new ParticleSystem particleSystem;

        protected virtual void OnEnable()
        {
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
        }

        public virtual void PlayParticles()
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play(true);
            StopCoroutine("AutoDestruct");
            StartCoroutine("AutoDestruct");
        }

        private void OnGameRestarted(GameRestarted e)
        {
            particleSystem.gameObject.SetActive(false);
            particleSystem.Stop(true);
            StopCoroutine("AutoDestruct");
        }

        private IEnumerator AutoDestruct()
        {
            yield return new WaitForSeconds(timer);
            particleSystem.Stop(true);
        }

        protected virtual void OnDisable()
        {
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
        }
    }
}
