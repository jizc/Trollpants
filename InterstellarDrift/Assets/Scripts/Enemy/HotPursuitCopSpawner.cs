// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HotPursuitCopSpawner.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class HotPursuitCopSpawner : MonoBehaviour
    {
        public GameObject cop;
        public GameObject[] spawners;
        public float spawnTime = 5f;

        private float time;

        private void Update()
        {
            time += Time.deltaTime;

            if (time > spawnTime)
            {
                Instantiate(cop, spawners[Random.Range(0, 1)].transform.position, Quaternion.identity);
                time = 0;
            }
        }
    }
}
