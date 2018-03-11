// <copyright file="PoolingEventHook.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    public class PoolingEventHook : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        private ObjectPooler objectPooler;

        public void SpawnPrefab()
        {
            if (objectPooler == null)
            {
                objectPooler = GetObjectPooler();
                if (objectPooler == null)
                {
                    return;
                }
            }

            var o = objectPooler.Spawn(prefab, null, transform.position, transform.localRotation);
            var particles = o.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                objectPooler.Recycle(o, particles.main.duration + 1f);
            }
        }

        private static ObjectPooler GetObjectPooler()
        {
            var poolerGameObject = GameObject.FindWithTag("ObjectPooler");
            return poolerGameObject?.GetComponent<ObjectPooler>();
        }

        private void Awake()
        {
            objectPooler = GetObjectPooler();
        }
    }
}
