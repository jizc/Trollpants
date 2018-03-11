// <copyright file="DetectionRadius.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using System.Collections.Generic;
    using System.Linq;
    using CloudOnce;
    using UnityEngine;

    public class DetectionRadius : MonoBehaviour
    {
        [SerializeField] private float _scoreThreshold = 1f;
        [SerializeField] private float _detectionRadius = 7.5f;
        [SerializeField] private LayerMask _detectionLayer;

        private bool isInitialized;
        private Dictionary<GameObject, float> previouslyDetected;

        public LayerMask DetectionLayer
        {
            get { return _detectionLayer; }
            set { _detectionLayer = value; }
        }

        public void Init(int detectionLayer)
        {
            if (isInitialized)
            {
                return;
            }

            previouslyDetected = new Dictionary<GameObject, float>();
            DetectionLayer = 1 << detectionLayer;   // Bitwise shift

            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            CheckDetectedAndAttemptToScore(GetGameObjectsWithinDetectionRadius(_detectionRadius, DetectionLayer));
        }

#if DEBUG
        private void OnDrawGizmos()
        {
            if (!isInitialized)
            {
                return;
            }

            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
#endif

        private void CheckDetectedAndAttemptToScore(ICollection<GameObject> currentDetected)
        {
            // Stores the removable keys in the dictionary, because we cannot modify the collection while looping through it.
            var removables = new List<GameObject>();

            // Clean list of previously detected of keys that are no longer present, increment time detected of still present keys
            foreach (var previousPair in previouslyDetected)
            {
                if (currentDetected.Contains(previousPair.Key))
                {
                    continue;
                }

                removables.Add(previousPair.Key);
            }

            // Actual cleaning outside of loop
            foreach (var removable in removables)
            {
                previouslyDetected.Remove(removable);
            }

            // Increases the time the gameobject has been detected for scoring purposes.
            for (var i = 0; i < previouslyDetected.Count; i++)
            {
                previouslyDetected[previouslyDetected.ElementAt(i).Key] += Time.deltaTime;
            }

            // Add new, unregistered detected gameobjects to list of previously detected
            foreach (var current in currentDetected)
            {
                if (previouslyDetected.ContainsKey(current))
                {
                    continue;
                }

                previouslyDetected.Add(current, 0f);
            }

            // Holds the keys for the values that have exceeded the scorethreshold and must be reset
            var resetables = new List<GameObject>();

            foreach (var previous in previouslyDetected)
            {
                if (previous.Value >= _scoreThreshold)
                {
                    // # Add previous to resetables
                    resetables.Add(previous.Key);

                    var distanceBetweenShipAndAsteroid = Vector3.Distance(transform.position, GetClosestPointOnTargetBounds(previous.Key.transform));

                    // Report on achievement
                    if (distanceBetweenShipAndAsteroid <= 0.5f)
                    {
                        distanceBetweenShipAndAsteroid = 0.5f;
                        Achievements.NearMiss.Unlock();
                    }

                    // The closer the ship and asteroid are, the more points
                    var score = 1f / distanceBetweenShipAndAsteroid;

                    // Maximum points are 50
                    score *= 50;

                    // Add score to score-display
                    ScoreSupervisor.Instance.IncreaseScore((int)score, true);
                }
            }

            // Turns back the hands of time like R. Kelly
            foreach (var resetable in resetables)
            {
                previouslyDetected[resetable] = 0f;
            }
        }

        /// <summary>
        /// Get a list of detected objects on a given layer, at a given radius.
        /// </summary>
        /// <param name="radius">The radius of the detection circle.</param>
        /// <param name="detectionLayer">The layer the gameobjects must be a member of to be detected.</param>
        /// <returns>List of detected gameobjects.</returns>
        private List<GameObject> GetGameObjectsWithinDetectionRadius(float radius, LayerMask detectionLayer)
        {
            if (radius <= 0)
            {
#if DEBUG
                Debug.LogWarning("ScoreSupervisor: Detection radius was zero or below.");
#endif
                return null;
            }

            var detectedColliders = Physics2D.OverlapCircleAll(transform.position, radius, detectionLayer);
            var detectedObjects = new List<GameObject>(detectedColliders.Length);
            detectedObjects.AddRange(detectedColliders.Select(detected => detected.gameObject));

            return detectedObjects;
        }

        private Vector3 GetClosestPointOnTargetBounds(Transform target)
        {
            var adjustedPoint = target.position;
            if (target.GetComponent<Collider2D>())
            {
                // Not the exact distance, and is completely wrong for irregularly shaped objects, but works for circular ones.
                var distanceToEdge = target.GetComponent<Collider2D>().bounds.extents.y;

                var direction = target.position - transform.position;

                adjustedPoint = target.position - (direction.normalized * distanceToEdge);
            }

            return adjustedPoint;
        }
    }
}
