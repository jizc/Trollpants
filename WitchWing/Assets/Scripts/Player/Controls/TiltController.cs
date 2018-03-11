// <copyright file="TiltController.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Player.Controls
{
    using Environment;
    using UnityEngine;

    public class TiltController
    {
        private readonly Transform playerTransform;
        private readonly float maxRotationAngle;

        private float yVelocity;

        public TiltController(Transform playerTransform, float maxRotationAngle = 30f)
        {
            this.playerTransform = playerTransform;
            this.maxRotationAngle = maxRotationAngle;
        }

        public float TargetRotationZ { get; private set; }

        public void Move(float targetAltitude, float verticalSpeed)
        {
            var nextAltitude = Mathf.SmoothDamp(
                playerTransform.localPosition.y,
                targetAltitude,
                ref yVelocity,
                0.4f,
                verticalSpeed);

            var nextRotation = CalculatePitch(targetAltitude, verticalSpeed);

            MoveAvatar(nextRotation, nextAltitude);
        }

        private Quaternion CalculatePitch(float targetAltitude, float verticalSpeed)
        {
            TargetRotationZ = 0f;

            if (targetAltitude > playerTransform.localPosition.y)
            {
                var distance = targetAltitude - playerTransform.localPosition.y;
                TargetRotationZ += distance * (maxRotationAngle / 3f);
            }
            else if (targetAltitude < playerTransform.localPosition.y)
            {
                var distance = playerTransform.localPosition.y - targetAltitude;
                TargetRotationZ -= distance * (maxRotationAngle / 3f);
            }

            var targetRotation = Quaternion.Euler(0f, 180f, -TargetRotationZ);
            return Quaternion.Slerp(playerTransform.rotation, targetRotation, verticalSpeed * Time.deltaTime);
        }

        private void MoveAvatar(Quaternion nextRotation, float nextAltitude)
        {
            var horizontalDelta = WorldScroller.ScrollSpeed * Time.deltaTime;

            playerTransform.localPosition = new Vector3(
                playerTransform.localPosition.x + horizontalDelta,
                nextAltitude);

            playerTransform.rotation = nextRotation;
        }
    }
}
