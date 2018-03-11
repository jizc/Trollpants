// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Spawner.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Spawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        public List<GameObject> ListOfPossibleSpawns;

        private float _spawnDelay;
        public int defaultSpawnsPerMinute;
        private int _spawnsPerMinute;
        private const int c_secondsInAMinute = 60;

        public float minSecBetweenRareSpawns = 5;
        private float _rareSpawnTimer;

        public float spawnZPos;

        private float _timer;
        private GameObject _objectToSpawn;
        private Vector3 _objectPlacement;
        private Quaternion _objectRotation;

        public bool bridgeShouldSpawn;

        private void OnEnable()
        {
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
            Events.instance.AddListener<SpeedChanged>(OnSpeedChanged);
            Events.instance.AddListener<HazardSpawnIncrease>(OnHazardSpawnRateIncreased);
            Events.instance.AddListener<HazardSpawnDecrease>(OnHazardSpawnRateDecreased);
        }

        // Use this for initialization
        private void Start()
        {
            _spawnsPerMinute = defaultSpawnsPerMinute;
            CalculateSpawnsPerMinute();

            Restart();
        }

        private void CalculateSpawnsPerMinute()
        {
            _spawnDelay = c_secondsInAMinute / (float)_spawnsPerMinute;
        }

        private void Restart()
        {
            if (gameObject.name != "TutorialHazardSpawner")
            {
                bridgeShouldSpawn = false;
            }

            _rareSpawnTimer = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!SavedData.GameIsPaused)
            {
                _timer += Time.deltaTime;
                if (_timer > _spawnDelay)
                {
                    SpawnHazard();
                    _timer = 0;
                }

                _rareSpawnTimer += Time.deltaTime;
            }
        }

        private void SpawnObject(GameObject objectToSpawn, Vector3 position, Quaternion rotation)
        {
            objectPooler.Spawn(objectToSpawn.name, transform, position, rotation);
        }

        private GameObject GetObjectToSpawn()
        {
            var spwn = ListOfPossibleSpawns[0];
            var found = false;

            while (!found)
            {
                spwn = ListOfPossibleSpawns[Random.Range(0, ListOfPossibleSpawns.Count)];

                if (!bridgeShouldSpawn && (spwn.name == "HazardBroWrapper" || spwn.name == "TutorialHazardBroWrapper"))
                {
                    continue;
                }

                var prop = spwn.gameObject.GetComponent<SpawningProperties>();

                if (Random.Range(0, 100) <= prop.spawningFrequency &&
                    (!prop.rareSpawn || _rareSpawnTimer >= minSecBetweenRareSpawns))
                {
                    found = true;
                    if (prop.rareSpawn)
                    {
                        _rareSpawnTimer = 0;
                    }
                }
            }

            return spwn;
        }

        private void SpawnHazard()
        {
            _objectToSpawn = GetObjectToSpawn();

            var properties = _objectToSpawn.gameObject.GetComponent<SpawningProperties>();

            _objectPlacement = new Vector3(Random.Range(properties.minX, properties.maxX),
                Random.Range(properties.minY, properties.maxY), spawnZPos);

            // if object should be rotated on spawn
            _objectRotation = properties.rotateOnSpawn
                ? new Quaternion(0, Random.Range(0, 20), 0, 10)
                : Quaternion.identity;

            SpawnObject(_objectToSpawn, _objectPlacement, _objectRotation);
        }

        #region Event handlers

        public void OnSpeedChanged(SpeedChanged speedChangedEvent)
        {
            foreach (Transform movingObject in transform)
            {
                var m = movingObject.GetComponent<MovingObject>();
                if (m != null)
                {
                    m.Speed = -speedChangedEvent.newPlayerSpeed;
                }
            }

            if (gameObject.name != "TutorialHazardSpawner")
            {
                bridgeShouldSpawn = GameObject.FindWithTag("Player").GetComponent<PlayerStats>().Score >= 1000;
            }
        }

        public void OnHazardSpawnRateIncreased(HazardSpawnIncrease spawnIncrease)
        {
            _spawnsPerMinute += spawnIncrease.spawnsPerMinute;
            CalculateSpawnsPerMinute();
        }

        public void OnHazardSpawnRateDecreased(HazardSpawnDecrease spawnDecrease)
        {
            CalculateSpawnsPerMinute();
            _spawnsPerMinute -= spawnDecrease.spawnsPerNimute;
        }

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            _spawnsPerMinute = defaultSpawnsPerMinute;
            CalculateSpawnsPerMinute();
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
            Events.instance.RemoveListener<SpeedChanged>(OnSpeedChanged);
            Events.instance.RemoveListener<HazardSpawnIncrease>(OnHazardSpawnRateIncreased);
            Events.instance.RemoveListener<HazardSpawnDecrease>(OnHazardSpawnRateDecreased);
        }
    }
}
