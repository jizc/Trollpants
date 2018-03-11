// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LookAtTarget2D.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class LookAtTarget2D : MonoBehaviour
    {
        public Transform TargetTransform;

        public void Init(Transform targetTransform)
        {
            TargetTransform = targetTransform;
        }

        private static float Angle(Vector2 vector2)
        {
            vector2 = new Vector2(-vector2.x, vector2.y);
            if (vector2.x < 0)
            {
                return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
            }

            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }

        private void OnEnable()
        {
            if (TargetTransform != null)
            {
                // Prevents "popping" from the previous rotation to the new rotation when being enabled after becoming disabled
                transform.localRotation = RotateTowardsTarget(TargetTransform.position);
            }
        }

        private void Update()
        {
            if (TargetTransform == null)
            {
#if DEBUG
                Debug.LogWarning(name + " has no target transform.");
#endif
                return;
            }

            transform.localRotation = RotateTowardsTarget(TargetTransform.position);
        }

        private Quaternion RotateTowardsTarget(Vector3 targetPosition)
        {
            var direction = targetPosition - transform.position;
            return Quaternion.Euler(0, 0, Angle(direction));
        }
    }
}
