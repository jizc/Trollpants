// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayArea.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    ///  Defines an area around the player that is used for gameplay.
    ///  Contains some specialized helper-functions for other classes.
    /// </summary>
    public class PlayArea : MonoBehaviour
    {
        private const float spawnAreaOffset = 160f;
        private const float spawnAreaRadius = 60f;

        private const float activeAreaOffset = 50;
        private const float activeAreaRadius = 170f;

        private const int maxDensityOfObstaclesInSpawnArea = 5;

        [SerializeField] private bool _initializeSelf;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private LayerMask _obstacleMask;

        private bool isInitialized;

        public event Action OnSpawnPossible;

        public CircleCollider2D SpawnArea { get; private set; }
        public CircleCollider2D ActiveArea { get; private set; }

        public void Init()
        {
            if (targetTransform == null)
            {
                var player = GameObject.FindWithTag("Player");
                targetTransform = player?.transform ?? transform;
#if DEBUG
                Debug.LogWarning("PlayArea origin-point not set. Using default transform '" + targetTransform.name + "'.");
#endif
            }

            if (SpawnArea == null)
            {
                var spawnArea = new GameObject { name = "SpawnArea" };
                SpawnArea = spawnArea.AddComponent<CircleCollider2D>();
                SpawnArea.isTrigger = true;
                SpawnArea.radius = spawnAreaRadius;
                SpawnArea.transform.position = targetTransform.position + (targetTransform.up * spawnAreaOffset);
            }

            if (ActiveArea == null)
            {
                var activeArea = new GameObject { name = "ActiveArea" };
                ActiveArea = activeArea.AddComponent<CircleCollider2D>();
                ActiveArea.isTrigger = true;
                ActiveArea.radius = activeAreaRadius;
                ActiveArea.transform.position = targetTransform.position + (targetTransform.up * activeAreaOffset);
            }

            isInitialized = true;
        }

        public bool GetSpawnLocation(float radiusOfObject, out Vector2 spawnLocation)
        {
            for (var i = 0; i < 10; i++)
            {
                var location = GetRandomPointInSpawnCircle();
                if (Physics2D.OverlapCircleAll(location, radiusOfObject, _obstacleMask).Length < 1)
                {
                    spawnLocation = location;
                    return true;
                }
            }

            spawnLocation = Vector2.zero;
            return false;
        }

        private void Awake()
        {
            if (_initializeSelf)
            {
                Init();
            }
        }

        private void Update()
        {
            if (!isInitialized || !targetTransform)
            {
                return;
            }

            SpawnArea.transform.position = targetTransform.position + (targetTransform.up * spawnAreaOffset);

            ActiveArea.transform.position = targetTransform.position + (targetTransform.up * activeAreaOffset);

            // Notify any listeners that it is possible to spawn.
            if (GetObstaclesInSpawnArea().Count < maxDensityOfObstaclesInSpawnArea)
            {
                OnSpawnPossible?.Invoke();
            }
        }

#if DEBUG && UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!isInitialized || !targetTransform)
            {
                return;
            }

            Gizmos.DrawRay(targetTransform.position, targetTransform.up * spawnAreaOffset);
            Gizmos.DrawWireSphere(targetTransform.position + (targetTransform.up * spawnAreaOffset), spawnAreaRadius);
            Gizmos.DrawWireSphere(targetTransform.position + (targetTransform.up * activeAreaOffset), activeAreaRadius);
        }
#endif

        private Vector2 GetRandomPointInSpawnCircle()
        {
            return SpawnArea.offset + (Vector2)SpawnArea.transform.position + (Random.insideUnitCircle * SpawnArea.radius);
        }

        private List<Transform> GetObstaclesInSpawnArea()
        {
            var obstacles = new List<Transform>();

            var collidersInArea = Physics2D.OverlapCircleAll((Vector2)SpawnArea.transform.position + SpawnArea.offset, SpawnArea.radius);
            foreach (var col in collidersInArea)
            {
                if (col.GetComponent<Obstacle>())
                {
                    obstacles.Add(col.transform);
                }
            }

            return obstacles;
        }
    }
}
