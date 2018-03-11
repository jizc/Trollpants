// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoalHerder.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    ///  Spawns the goal the player must reach before his time runs out.
    /// </summary>
    public class GoalHerder : MonoBehaviour
    {
        private const float multiplierIncreaseRate = 0.1f;
        private const float maxMultiplier = 2f;

        [SerializeField] private bool _initializeSelf;
        [SerializeField] private TimedMode _timerDisplay;
        [SerializeField] private int _goalsCapacity;
        [SerializeField] private Transform targetTransform;

        private float distanceMultiplier = 1f;

        private GameObject goalPrefab;
        private PointerFactory pointerFactory;
        private List<GameObject> goals;

        public void Init()
        {
            goalPrefab = Resources.Load<GameObject>("Prefabs/Goal");

            if (targetTransform == null)
            {
#if DEBUG
                Debug.LogWarning("GoalHerder is lacking a reference to the ship's location.");
#endif
                var player = GameObject.FindWithTag("Player");
                targetTransform = player?.transform ?? transform;
            }

            if (GameObject.FindWithTag("PointerCanvas"))
            {
                pointerFactory = GameObject.FindWithTag("PointerCanvas").GetComponent<PointerFactory>();
                pointerFactory.Init();
            }

            if (GameObject.FindWithTag("TimerDisplay"))
            {
                _timerDisplay = GameObject.FindWithTag("TimerDisplay").GetComponent<TimedMode>();
            }

            if (_goalsCapacity <= 0)
            {
                _goalsCapacity = 3;
            }

            goals = new List<GameObject>(_goalsCapacity);
        }

        /// <summary>
        /// Method that places a new goal in a random direction, in a set distance away from the TargetTransform.
        /// </summary>
        public void SpawnGoal()
        {
            const float distance = 100f;
            var direction = Random.insideUnitCircle.normalized;

            direction *= distance * distanceMultiplier;

            if (distanceMultiplier < maxMultiplier)
            {
                distanceMultiplier += multiplierIncreaseRate;
            }

#if DEBUG
            Debug.Log("Spawning new goal.");
            Debug.Log("Direction: " + direction);
            Debug.Log("Distance: " + distance);
#endif

            var position = direction + (Vector2)targetTransform.position;

            SpawnGoal(position);
        }

        private void SpawnGoal(Vector3 position)
        {
            if (goals.Count >= _goalsCapacity)
            {
#if DEBUG
                Debug.LogWarning("Tried to create more goals than capacity of " + _goalsCapacity + " allows.");
#endif
                return;
            }

            // Instantiate new goal and child it to the herder-object
            var goal = Instantiate(goalPrefab, position, Quaternion.identity) as GameObject;
            goal.transform.parent = transform;

            // Attach listener to removal event
            goal.GetComponent<Goal>().OnTouchedShip += RemoveGoal;

            // Create pointer to this goal
            pointerFactory.CreateNewPointer(goal.transform, targetTransform, true, true);

            goals.Add(goal);
        }

        private void RemoveGoal(GameObject goal, GameObject triggeringObject)
        {
            if (_timerDisplay && GameMode.Instance.CurrentMode == GameMode.Mode.Time)
            {
                _timerDisplay.RefreshDuration();
            }

            goals.Remove(goal);
            goal.GetComponent<Goal>().OnTouchedShip -= RemoveGoal;
            Destroy(goal);
            SpawnGoal();
        }

        private void Start()
        {
            if (_initializeSelf)
            {
                Init();
                SpawnGoal();
            }
        }
    }
}
