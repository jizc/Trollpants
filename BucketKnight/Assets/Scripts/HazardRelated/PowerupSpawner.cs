// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerupSpawner.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using CloudOnce;
    using UnityEngine;

    public class PowerupSpawner : MonoBehaviour
    {
        public List<GameObject> ListOfPossibleSpawns;

        private float _spawnDelay;
        public int defaultSpawnDelay;
        public int secondsDecreasedPerPowerup;

        public float spawnZPos;

        private float _timer;
        private GameObject _objectToSpawn;
        private Vector3 _objectPlacement;

        public ObjectPooler objectPooler;

        private void OnEnable()
        {
            Events.instance.AddListener<SpeedChanged>(OnSpeedChanged);
        }

        private void Start()
        {
            _spawnDelay = defaultSpawnDelay - secondsDecreasedPerPowerup * CloudVariables.NumberOfPowerupsUnlocked;
        }

        private void Update()
        {
            if (!SavedData.GameIsPaused && CloudVariables.NumberOfPowerupsUnlocked > 0)
            {
                _timer += Time.deltaTime;
                if (_timer > _spawnDelay)
                {
                    SpawnHazard();
                    _timer = 0;
                }
            }
        }

        private void SpawnObject(GameObject objectToSpawn, Vector3 position, Quaternion rotation)
        {
            objectPooler.Spawn(objectToSpawn.name, transform, position, rotation);
        }

        private void SpawnHazard()
        {
            _objectToSpawn = ListOfPossibleSpawns[0];
            var properties = _objectToSpawn.gameObject.GetComponent<SpawningProperties>();
            _objectPlacement = new Vector3(Random.Range(properties.minX, properties.maxX),
                Random.Range(properties.minY, properties.maxY), spawnZPos);

            // if object should be rotated on spawn
            SpawnObject(_objectToSpawn, _objectPlacement, Quaternion.identity);
        }

        private void OnSpeedChanged(SpeedChanged speedChangedEvent)
        {
            foreach (Transform movingObject in transform)
            {
                var m = movingObject.GetComponent<MovingObject>();
                if (m != null)
                {
                    m.Speed = -speedChangedEvent.newPlayerSpeed;
                }
            }
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<SpeedChanged>(OnSpeedChanged);
        }
    }
}
