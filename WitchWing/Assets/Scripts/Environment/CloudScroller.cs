// <copyright file="CloudScroller.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System;
    using Data;
    using Player;
    using UnityEngine;

    public class CloudScroller : MonoBehaviour
    {
        private Vector3 spawnPosition = Vector3.zero;
        private CloudShepherd cloudShepherd;
        private CloudShepherd.CloudDistance cloudDistance;
        private Transform cloudTransform;

        public void Initialize(CloudShepherd shepherd, CloudShepherd.CloudDistance distance)
        {
            cloudShepherd = shepherd;
            spawnPosition = transform.localPosition;
            cloudDistance = distance;
        }

        private void Awake()
        {
            cloudTransform = transform;
        }

        private void Update()
        {
            if (GameState.IsPaused || Player.State.IsDead)
            {
                return;
            }

            if (cloudTransform.localPosition.x < -30f)
            {
                cloudShepherd.RecycleCloud(gameObject);
                return;
            }

            float cloudSpeed;
            switch (cloudDistance)
            {
                case CloudShepherd.CloudDistance.Front:
                    cloudSpeed = WorldScroller.CloudScrollSpeedFront;
                    break;
                case CloudShepherd.CloudDistance.Mid:
                    cloudSpeed = WorldScroller.CloudScrollSpeedMid;
                    break;
                case CloudShepherd.CloudDistance.Back:
                    cloudSpeed = WorldScroller.CloudScrollSpeedBack;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var targetPostion = new Vector3(
                cloudTransform.localPosition.x - cloudSpeed,
                spawnPosition.y,
                spawnPosition.z + (WorldScroller.CameraZBias * cloudSpeed * -0.25f));

            cloudTransform.localPosition = Vector3.MoveTowards(
                cloudTransform.localPosition, targetPostion, cloudSpeed * Time.deltaTime);
        }
    }
}
