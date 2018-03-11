// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoneySpawner.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using CloudOnce;
    using UnityEngine;

    public class MoneySpawner : MonoBehaviour
    {
        private float _spawnDelay;

        public int defaultSpawnPerMinute;
        private int _spawnsPerMinute;
        private const int c_secondsInAMinute = 60;
        public int gemSpawnRate;

        private const float c_parts = 5f;
        private float _partDelay;

        public float spawnZPos;

        private float _timer;
        private float _spawnTimer;

        public GameObject money;
        public GameObject gem;
        private Vector3 _objectPlacement;

        private float _minX, _maxX;

        private float _spawnX;
        private float _nextSpawnX;
        private float _spawnDiff;

        private int _moneyToSpawn;

        public ObjectPooler objectPooler;

        private void OnEnable()
        {
            Events.instance.AddListener<SpeedChanged>(OnSpeedChanged);
            Events.instance.AddListener<MoneySpawnIncrease>(OnMoneySpawnIncrease);
            Events.instance.AddListener<MoneySpawnDecrease>(OnMoneySpawnDecrease);
            Events.instance.AddListener<CoinPowerup>(OnCoinPowerup);
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
        }

        private void CalculateSpawnsPerMinute()
        {
            _spawnDelay = c_secondsInAMinute / (float)_spawnsPerMinute;
            _partDelay = _spawnDelay / c_parts;
        }

        private void Start()
        {
            _spawnsPerMinute = defaultSpawnPerMinute;
            CalculateSpawnsPerMinute();

            var properties = money.gameObject.GetComponent<SpawningProperties>();

            _partDelay = _spawnDelay / c_parts;

            _minX = properties.minX;
            _maxX = properties.maxX;
            SetSpawn();
        }

        private void Update()
        {
            if (SavedData.GameIsPaused)
            {
                return;
            }

            _timer += Time.deltaTime;

            if (_timer > _spawnDelay)
            {
                if (_moneyToSpawn == 0)
                {
                    _moneyToSpawn++;
                }

                SetSpawn();
                _timer = 0;
            }

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer < _partDelay)
            {
                return;
            }

            _spawnTimer = 0;

            _spawnX += _spawnDiff;

            UpdateDifference();

            if (_moneyToSpawn > 0)
            {
                SpawnPickup();
            }
        }

        private void SpawnObject(GameObject objectToSpawn, Vector3 position)
        {
            objectPooler.Spawn(objectToSpawn.name, transform, position, Quaternion.identity);
        }

        private void SpawnPickup()
        {
            if (_spawnX > _maxX)
            {
                _spawnX = _maxX;
            }

            if (_spawnX < _minX)
            {
                _spawnX = _minX;
            }

            _moneyToSpawn--;
            _objectPlacement = new Vector3(_spawnX, 0.5f, spawnZPos);
            if (Random.Range(0, 100) <= gemSpawnRate && CloudVariables.NumberOfPowerupsUnlocked >= 7)
            {
                SpawnObject(gem, _objectPlacement);
            }
            else
            {
                SpawnObject(money, _objectPlacement);
            }
        }

        private void SetSpawn()
        {
            _nextSpawnX = Random.Range(_minX, _maxX);
            UpdateDifference();
        }

        private void UpdateDifference()
        {
            _spawnDiff = (_nextSpawnX + 10f - (_spawnX + 10f)) / c_parts;

            if (_spawnDiff < 0)
            {
                _spawnDiff -= 0.5f;
            }
            else
            {
                _spawnDiff += 0.5f;
            }
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
        }

        public void OnMoneySpawnIncrease(MoneySpawnIncrease spawnIncrease)
        {
            _spawnsPerMinute += spawnIncrease.spawnsPerNimute;
            CalculateSpawnsPerMinute();
        }

        public void OnMoneySpawnDecrease(MoneySpawnDecrease spawnDecrease)
        {
            _spawnsPerMinute -= spawnDecrease.spawnsPerNimute;
            CalculateSpawnsPerMinute();
        }

        public void OnCoinPowerup(CoinPowerup coinPowerupEvent)
        {
            if (coinPowerupEvent.moneyToSpawn == 0)
            {
                _moneyToSpawn = 0;
            }

            _moneyToSpawn += coinPowerupEvent.moneyToSpawn;
        }

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            _spawnsPerMinute = defaultSpawnPerMinute;
            CalculateSpawnsPerMinute();
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<SpeedChanged>(OnSpeedChanged);
            Events.instance.RemoveListener<MoneySpawnIncrease>(OnMoneySpawnIncrease);
            Events.instance.RemoveListener<MoneySpawnDecrease>(OnMoneySpawnDecrease);
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
            Events.instance.RemoveListener<CoinPowerup>(OnCoinPowerup);
        }
    }
}
