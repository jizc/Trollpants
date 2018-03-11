// <copyright file="CloudShepherd.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System;
    using Data;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class CloudShepherd : MonoBehaviour
    {
        private const string cloudsPath = "Clouds/";
        private const float frontSpawnRange = 1f;
        private const float midSpawnRange = 1f;
        private const float backSpawnRange = 0.8f;

        [SerializeField] private Transform backContainer;
        [SerializeField] private Transform midContainer;
        [SerializeField] private Transform frontContainer;
        [SerializeField] private Transform pool;

        private float currentFrontSpawnDelay;
        private float currentMidSpawnDelay;
        private float currentBackSpawnDelay;

        private float clusterCountdownFront;
        private float clusterCountdownMid;
        private float clusterCountdownBack;

        private float activeClusterCountdownFront;
        private float activeClusterCountdownMid;
        private float activeClusterCountdownBack;

        private bool isFrontClusterTime;
        private bool isMidClusterTime;
        private bool isBackClusterTime;

        public enum CloudDistance
        {
            Front,
            Mid,
            Back
        }

        public void ResetClouds()
        {
            var backClouds = backContainer.GetComponentsInChildren<CloudScroller>();
            var midClouds = midContainer.GetComponentsInChildren<CloudScroller>();
            var frontClouds = frontContainer.GetComponentsInChildren<CloudScroller>();

            foreach (var cloud in backClouds)
            {
                RecycleCloud(cloud.gameObject);
            }

            foreach (var cloud in midClouds)
            {
                RecycleCloud(cloud.gameObject);
            }

            foreach (var cloud in frontClouds)
            {
                RecycleCloud(cloud.gameObject);
            }

            PrewarmClouds();
        }

        public void RecycleCloud(GameObject cloud)
        {
            if (cloud == null)
            {
                Debug.LogError("The cloud you want to recycle is null.");
                return;
            }

            if (pool == null)
            {
                Debug.Log("Cloud pool has been destroyed");
                return;
            }

            cloud.SetActive(false);

            var cloudTypePool = pool.Find(cloud.name + "s");
            if (cloudTypePool == null)
            {
                var newGameObject = new GameObject { name = cloud.name + "s" };
                var ngt = newGameObject.transform;
                ngt.parent = pool;
                ngt.localPosition = Vector3.zero;
                cloudTypePool = ngt;
            }

            cloud.transform.parent = cloudTypePool;
            cloud.transform.localPosition = Vector3.zero;
        }

        private static float RandomRotation()
        {
            return Random.Range(0, 2) == 0 ? 0f : 180f;
        }

        private static void PrewarmPlacement(GameObject cloud)
        {
            var p = cloud.transform.localPosition;
            cloud.transform.localPosition = new Vector3(Random.Range(0f, -25f), p.y, p.z);
        }

        private void Awake()
        {
            currentFrontSpawnDelay = Random.Range(0.1f, 3f);
            currentMidSpawnDelay = Random.Range(0.1f, 2f);
            currentBackSpawnDelay = Random.Range(0.1f, 1.5f);
            clusterCountdownFront = Random.Range(3f, 7f);
            clusterCountdownMid = Random.Range(3f, 7f);
            clusterCountdownBack = Random.Range(3f, 7f);
        }

        private void Start()
        {
            PrewarmClouds();
        }

        private void Update()
        {
            if (GameState.IsPaused)
            {
                return;
            }

            var adjustedDeltaTime = Time.deltaTime * WorldScroller.ScrollSpeed / 5f;

            HandleClusterTime(adjustedDeltaTime);

            if (IsTimeToSpawn(CloudDistance.Front, adjustedDeltaTime))
            {
                var cloudFront = SpawnCloud();
                RandomlyPlaceAndActivateNewCloud(cloudFront, CloudDistance.Front);
            }

            if (IsTimeToSpawn(CloudDistance.Mid, adjustedDeltaTime))
            {
                var cloudMid = SpawnCloud();
                RandomlyPlaceAndActivateNewCloud(cloudMid, CloudDistance.Mid);
            }

            if (IsTimeToSpawn(CloudDistance.Back, adjustedDeltaTime))
            {
                var cloudBack = SpawnCloud();
                RandomlyPlaceAndActivateNewCloud(cloudBack, CloudDistance.Back);
            }
        }

        private void PrewarmClouds(int numberOfClouds = 5)
        {
            for (var i = 0; i < numberOfClouds; i++)
            {
                var cloudFront = SpawnCloud();
                var cloudMid = SpawnCloud();
                var cloudBack = SpawnCloud();
                RandomlyPlaceAndActivateNewCloud(cloudFront, CloudDistance.Front);
                RandomlyPlaceAndActivateNewCloud(cloudMid, CloudDistance.Mid);
                RandomlyPlaceAndActivateNewCloud(cloudBack, CloudDistance.Back);
                PrewarmPlacement(cloudFront);
                PrewarmPlacement(cloudMid);
                PrewarmPlacement(cloudBack);
            }
        }

        private void HandleClusterTime(float deltaTime)
        {
            if (!isFrontClusterTime)
            {
                if (clusterCountdownFront > 0)
                {
                    clusterCountdownFront -= deltaTime;
                }
                else
                {
                    activeClusterCountdownFront = Random.Range(4f, 6f);
                    isFrontClusterTime = true;
                }
            }
            else
            {
                if (activeClusterCountdownFront > 0)
                {
                    activeClusterCountdownFront -= deltaTime;
                }
                else
                {
                    clusterCountdownFront = Random.Range(6f, 10f);
                    isFrontClusterTime = false;
                }
            }

            if (!isMidClusterTime)
            {
                if (clusterCountdownMid > 0)
                {
                    clusterCountdownMid -= deltaTime;
                }
                else
                {
                    activeClusterCountdownMid = Random.Range(2.5f, 4f);
                    isMidClusterTime = true;
                }
            }
            else
            {
                if (activeClusterCountdownMid > 0)
                {
                    activeClusterCountdownMid -= deltaTime;
                }
                else
                {
                    clusterCountdownMid = Random.Range(5f, 9f);
                    isMidClusterTime = false;
                }
            }

            if (!isBackClusterTime)
            {
                if (clusterCountdownBack > 0)
                {
                    clusterCountdownBack -= deltaTime;
                }
                else
                {
                    activeClusterCountdownBack = Random.Range(1f, 2.5f);
                    isBackClusterTime = true;
                }
            }
            else
            {
                if (activeClusterCountdownBack > 0)
                {
                    activeClusterCountdownBack -= deltaTime;
                }
                else
                {
                    clusterCountdownBack = Random.Range(4f, 8f);
                    isBackClusterTime = false;
                }
            }
        }

        private bool IsTimeToSpawn(CloudDistance cloudDistance, float deltaTime)
        {
            switch (cloudDistance)
            {
                case CloudDistance.Front:
                    if (currentFrontSpawnDelay > 0)
                    {
                        currentFrontSpawnDelay -= deltaTime;
                        return false;
                    }

                    currentFrontSpawnDelay = isFrontClusterTime
                        ? Random.Range(1f, 2f)
                        : Random.Range(3f, 5f);

                    break;
                case CloudDistance.Mid:
                    if (currentMidSpawnDelay > 0)
                    {
                        currentMidSpawnDelay -= deltaTime;
                        return false;
                    }

                    currentMidSpawnDelay = isMidClusterTime
                        ? Random.Range(0.5f, 1.5f)
                        : Random.Range(2f, 4f);
                    break;
                case CloudDistance.Back:
                    if (currentBackSpawnDelay > 0)
                    {
                        currentBackSpawnDelay -= deltaTime;
                        return false;
                    }

                    currentBackSpawnDelay = isBackClusterTime
                        ? Random.Range(0.2f, 0.4f)
                        : Random.Range(2f, 4f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("cloudDistance");
            }

            return true;
        }

        private GameObject SpawnCloud()
        {
            var prefabName = "Cloud" + Random.Range(1, 7);
            var cloudTypePool = pool.Find(prefabName + "s");
            var rotation = Quaternion.Euler(RandomRotation(), RandomRotation(), RandomRotation());
            GameObject cloud;

            if (cloudTypePool != null && cloudTypePool.childCount > 0)
            {
                cloud = cloudTypePool.GetChild(0).gameObject;
                cloud.transform.rotation = rotation;
            }
            else
            {
                cloud = Instantiate(Resources.Load<GameObject>(cloudsPath + prefabName), Vector3.zero, rotation);
                cloud.name = prefabName;
            }

            return cloud;
        }

        private void RandomlyPlaceAndActivateNewCloud(GameObject cloud, CloudDistance cloudDistance)
        {
            var cloudScroller = cloud.GetComponent<CloudScroller>();
            float yPosition;
            float xScale;
            float yScale;

            switch (cloudDistance)
            {
                case CloudDistance.Front:
                    yScale = 1f + Random.Range(0f, 0.5f);
                    xScale = yScale + Random.Range(0f, 0.7f);
                    cloud.transform.parent = frontContainer;
                    yPosition = Random.Range(-frontSpawnRange, frontSpawnRange);
                    cloud.transform.GetChild(0).gameObject.layer = 17;
                    break;
                case CloudDistance.Mid:
                    yScale = 0.6f + Random.Range(0f, 0.25f);
                    xScale = yScale + Random.Range(0f, 0.5f);
                    cloud.transform.parent = midContainer;
                    yPosition = Random.Range(-midSpawnRange, midSpawnRange);
                    cloud.transform.GetChild(0).gameObject.layer = 16;
                    break;
                case CloudDistance.Back:
                    yScale = 0.3f + Random.Range(0f, 0.1f);
                    xScale = yScale + Random.Range(0f, 0.3f);
                    cloud.transform.parent = backContainer;
                    yPosition = Random.Range(-backSpawnRange + 0.3f, backSpawnRange);
                    cloud.transform.GetChild(0).gameObject.layer = 15;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("cloudDistance");
            }

            cloud.transform.localScale = new Vector3(xScale, yScale, yScale);
            cloud.transform.localPosition = new Vector3(0f, yPosition, 0f);
            cloudScroller.Initialize(this, cloudDistance); 
            cloud.SetActive(true);
        }
    }
}
