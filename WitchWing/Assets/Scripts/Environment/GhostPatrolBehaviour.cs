// <copyright file="GhostPatrolBehaviour.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System;
    using Data;
    using UnityEngine;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public class GhostPatrolBehaviour : MonoBehaviour
    {
        [SerializeField] private Axis alongAxis = Axis.Y;
        [SerializeField] private float range = 10;
        [SerializeField] private float speed = 1;
        [SerializeField] private float currentDistance;

        private Transform cachedTransform;
        private float deltaTime;
        private bool isMovingTowards = true;

        public void ResetDistance()
        {
            currentDistance = 0;
        }

        private void Start()
        {
            cachedTransform = transform;
        }

        private void Update()
        {
            if (GameState.IsPaused)
            {
                return;
            }

            deltaTime = Time.deltaTime;

            switch (alongAxis)
            {
                case Axis.X:
                    cachedTransform.Translate(speed * deltaTime, 0, 0);
                    break;

                case Axis.Y:
                    if (speed < 0)
                    {
                        cachedTransform.Translate(0, speed * deltaTime, 0);
                    }
                    else
                    {
                        cachedTransform.Translate(0, speed * 2 * deltaTime, 0);
                    }

                    break;

                case Axis.Z:
                    cachedTransform.Translate(0, 0, speed * deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (speed < 0)
            {
                currentDistance += speed * deltaTime;
            }
            else
            {
                currentDistance += speed * deltaTime * 2;
            }

            if (isMovingTowards && currentDistance > range)
            {
                isMovingTowards = false;
                speed *= -1;
            }
            else if (!isMovingTowards && currentDistance < 0)
            {
                isMovingTowards = true;
                speed *= -1;
            }
        }
    }
}
