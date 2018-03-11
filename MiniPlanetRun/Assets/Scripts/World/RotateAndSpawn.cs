// <copyright file="RotateAndSpawn.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.World
{
    using System.Collections.Generic;
    using UnityEngine;

    public enum ObstacleType
    {
        None,
        Jumpable,
        Slideable,
        Currency,
        Plattform
    }

    public class RotateAndSpawn : MonoBehaviour
    {
        private const float spawnInterval = 0.9f;

        [SerializeField] private ObjectPooler objectPooler;
        [SerializeField] private float speed;
        [SerializeField] private float speedCap = 3f;
        [SerializeField] private List<GameObject> jumpables;
        [SerializeField] private List<GameObject> slideables;
        [SerializeField] private List<GameObject> platforms;
        [SerializeField] private GameObject currency;

        private float obstacleAdjustment;

        private Transform cachedTransform;
        private Transform obstacleContainer;

        private bool isSpawning;
        private float spawnTime;

        public void SetSpawning(bool value)
        {
            isSpawning = value;
        }

        public void SetSpeed(float value)
        {
            speed = value;
        }

        private void Awake()
        {
            cachedTransform = transform;
            obstacleContainer = GameObject.Find("ObjectContainer").transform;
            obstacleAdjustment = cachedTransform.parent.position.y - 3;
        }

        private Vector3 CurrencyPosition(int posNum)
        {
            Vector3[] pos =
            {
                new Vector3(0, -2.6f + obstacleAdjustment, cachedTransform.position.z),
                new Vector3(0, -1.3f + obstacleAdjustment, cachedTransform.position.z)
            };
            return pos[posNum];
        }

        private void PreSpawnStuff()
        {
            const int howMany = 5;

            foreach (var j in jumpables)
            {
                PrespawnThing(j, howMany);
            }

            foreach (var s in slideables)
            {
                PrespawnThing(s, howMany);
            }

            foreach (var p in platforms)
            {
                PrespawnThing(p, howMany);
            }

            PrespawnThing(currency, howMany);
        }

        private void PrespawnThing(GameObject thing, int howMany)
        {
            var gs = new GameObject[howMany];

            for (var i = 0; i < howMany; i++)
            {
                gs[i] = objectPooler.Spawn(
                    "Prefabs/",
                    thing.name,
                    obstacleContainer,
                    Vector3.zero);
            }

            for (var i = 0; i < howMany; i++)
            {
                objectPooler.Recycle(gs[i]);
            }
        }

        private void SpawnObstacle()
        {
            var spawnType = (ObstacleType)Random.Range(0, 5);
            GameObject spawn;
            switch (spawnType)
            {
                case ObstacleType.None:
                    return;

                case ObstacleType.Slideable:
                    spawn = objectPooler.Spawn(
                        "Prefabs/",
                        slideables[Random.Range(0, slideables.Count)].name,
                        obstacleContainer,
                        new Vector3(0, -2.2f + obstacleAdjustment, cachedTransform.position.z));
                    spawn.transform.eulerAngles = new Vector3(0, 0, 270);
                    break;

                case ObstacleType.Jumpable:
                    spawn = objectPooler.Spawn(
                        "Prefabs/",
                        jumpables[Random.Range(0, jumpables.Count)].name,
                        obstacleContainer,
                        new Vector3(0, -1f + obstacleAdjustment, cachedTransform.position.z));
                    spawn.transform.eulerAngles = new Vector3(0, 0, 270);
                    break;

                case ObstacleType.Plattform:
                    spawn = objectPooler.Spawn(
                        "Prefabs/",
                        platforms[Random.Range(0, platforms.Count)].name,
                        obstacleContainer,
                        new Vector3(0, 1f + obstacleAdjustment, cachedTransform.position.z));
                    spawn.transform.eulerAngles = new Vector3(0, 0, 170);

                    GameObject cur;
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            cur = objectPooler.Spawn(
                                "Prefabs/",
                                currency.name,
                                obstacleContainer,
                                CurrencyPosition(0));
                            cur.transform.eulerAngles = Vector3.zero;
                            cur.layer = 14;
                            break;
                        case 1:
                            cur = objectPooler.Spawn(
                                "Prefabs/",
                                currency.name,
                                obstacleContainer,
                                CurrencyPosition(1));
                            cur.transform.eulerAngles = Vector3.zero;
                            cur.layer = 14;
                            break;
                        case 2:
                            // Highest cherry
                            cur = objectPooler.Spawn(
                                "Prefabs/",
                                currency.name,
                                obstacleContainer,
                                new Vector3(-2, -3.2f + obstacleAdjustment, cachedTransform.position.z));
                            cur.transform.eulerAngles = Vector3.zero;
                            cur.layer = 14;
                            break;
                    }

                    break;

                case ObstacleType.Currency:
                    spawn = objectPooler.Spawn(
                        "Prefabs/",
                        currency.name,
                        obstacleContainer,
                        CurrencyPosition(0));
                    spawn.transform.eulerAngles = Vector3.zero;
                    break;

                default:
                    return;
            }

            spawn.layer = 14;
        }

        private void Start()
        {
            PreSpawnStuff();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            cachedTransform.Rotate(Vector3.forward, 60 * deltaTime * speed);

            if (isSpawning)
            {
                spawnTime += deltaTime;

                if (spawnTime >= spawnInterval)
                {
                    spawnTime = spawnTime - spawnInterval;
                    SpawnObstacle();
                    if (speed < speedCap)
                    {
                        speed += 0.005f;
                    }
                }
            }
        }
    }
}
