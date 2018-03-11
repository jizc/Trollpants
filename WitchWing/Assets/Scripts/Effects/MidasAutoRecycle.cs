// <copyright file="MidasAutoRecycle.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Effects
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem))]
    public class MidasAutoRecycle : MonoBehaviour
    {
        private ParticleSystem system;
        private MidasEffectSpawner midasEffectSpawner;

        public void SetSpawner(MidasEffectSpawner spawner)
        {
            midasEffectSpawner = spawner;
        }

        private void Awake()
        {
            system = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            system.Play();
            StartCoroutine(CheckIfAlive());
        }

        private IEnumerator CheckIfAlive()
        {
            while (system.IsAlive(true))
            {
                yield return new WaitForSeconds(0.5f);
            }

            midasEffectSpawner.Recycle(gameObject);
        }
    }
}
