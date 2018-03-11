// <copyright file="MeasureVelocity.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    public class MeasureVelocity : MonoBehaviour
    {
        [SerializeField] private float _interval;

        private Rigidbody2D cachedRigidbody2D;
        private float timeAlive;
        private float velocityCount;

        private void Awake()
        {
            cachedRigidbody2D = GetComponent<Rigidbody2D>();
            Init();
            Invoke("LogVelocity", _interval);
        }

        private void Update()
        {
            timeAlive += Time.deltaTime;

            velocityCount += cachedRigidbody2D.velocity.magnitude * Time.deltaTime * 60;
        }

        private void Init()
        {
            timeAlive = 0;
            velocityCount = 0;
        }

        private void LogVelocity()
        {
            TrackedData.Instance.SessionData.DistanceTravelled += Mathf.FloorToInt(velocityCount/timeAlive);
            Init();

            Invoke("LogVelocity", _interval);
        }
    }
}
