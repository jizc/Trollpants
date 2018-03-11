// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraSizeRelativeToVelocity.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class CameraSizeRelativeToVelocity : MonoBehaviour
    {
        [SerializeField] private bool _initializeSelf;

        // CAMERA SIZE VARIABLES
        public Rigidbody2D TargetRigidbody2D;
        public Camera[] Cameras;

        [SerializeField] private float _minCameraSize = 15f;
        [SerializeField] private float _maxCameraSize = 30f;
        [SerializeField] private float _sizeChangeDuration = 1f;

        private bool driveCameraValues;
        private float currentTargetSize;
        private float sizeChangeVelocity;

        public void Init()
        {
            driveCameraValues = true;

            if (TargetRigidbody2D == null)
            {
#if DEBUG
                Debug.LogWarning("No TargetRigidbody2D. Camera-sizes will not be modified.");
#endif
            }

            // Check if there are any cameras
            if (Cameras.Length <= 0)
            {
#if DEBUG
                Debug.LogWarning("No cameras to drive values in.");
#endif
                driveCameraValues = false;
            }

            // Make sure that all cameras in the list are orthogonal-projection only.
            foreach (var cam in Cameras)
            {
                if (!cam.orthographic)
                {
#if DEBUG
                    Debug.LogWarning(cam.name + " is not set to orthogonal projection. Making orthogonal.");
#endif
                    cam.orthographic = true;
                }
            }

            // Set initial values
            currentTargetSize = _minCameraSize;
        }

        private void Awake()
        {
            if (_initializeSelf)
            {
                Init();
            }
        }

        private void Update()
        {
            if (!driveCameraValues || TargetRigidbody2D == null)
            {
                return;
            }

            // Adjust's the size of the orthogonal cameras view linearly with the velocity of the target object
            var velocityMagnitude = Vector2.SqrMagnitude(TargetRigidbody2D.velocity);
            currentTargetSize = Mathf.Clamp(velocityMagnitude, _minCameraSize, _maxCameraSize);

            foreach (var cam in Cameras)
            {
                cam.orthographicSize = Mathf.SmoothDamp(
                    cam.orthographicSize,
                    currentTargetSize,
                    ref sizeChangeVelocity,
                    _sizeChangeDuration);
            }
        }
    }
}
