// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObstacleHerder.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    [RequireComponent(typeof(PlayArea))]
    public class ObstacleHerder : MonoBehaviour
    {
        [SerializeField] private GameObject[] obstaclePrefabs;

        private ObjectPooler objectPooler;
        private PlayArea playArea;

        private void Awake()
        {
            playArea = GetComponent<PlayArea>();
            playArea.OnSpawnPossible += SpawnObstacle;
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }

        private void OnDestroy()
        {
            playArea.OnSpawnPossible -= SpawnObstacle;
        }

        private void SpawnObstacle()
        {
            var obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // fake radius for all collider2D
            var radius = obstaclePrefab.GetComponent<Obstacle>().ApproximateRadius;

            Vector2 spawnLocation;
            if (!playArea.GetSpawnLocation(radius, out spawnLocation))
            {
                return;
            }

            var obstacle = objectPooler.Spawn(obstaclePrefab, null, spawnLocation);
            if (obstacle.Equals(null))
            {
#if DEBUG
                Debug.LogWarning("Tried to spawn an obstacle, but got no prefab from the pool.");
#endif
                return;
            }

            obstacle.GetComponent<Obstacle>().SetAllowedArea(playArea.ActiveArea);
        }
    }
}
