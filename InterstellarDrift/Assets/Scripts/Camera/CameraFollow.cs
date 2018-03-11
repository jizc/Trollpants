// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraFollow.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    /// Drives the position of a gameobject containing cameras.
    /// IMPORTANT: Assumes that the script is on a container-gameobject, with the cameras you want driven around set as children.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private bool _initializeSelf;

        public Transform TargetTransform;

        [SerializeField]
        private float _followSmoothTime = .25f;
        public Vector3 Offset;

        [Range(0f, 1f)]
        public float DistanceFromMiddleRatio = .95f;

        [SerializeField]
        private float _dOffsetSmoothTime = 3f;

        private Vector3 originalValues;
        private Vector3 followVelocity;

        private Vector3 dynamicOffset;
        private Vector3 dynamicOffsetVelocity;

        private float cameraUnitDepth;
        private Camera cachedCamera;

        private Vector3 screenMiddle;
        private float xAxisExtent;
        private float yAxisExtent;

#if DEBUG
        [SerializeField]
        private Vector3[] _edgeVectors;
#endif

        private bool isInitialized;

        public void Init()
        {
#if DEBUG
            ////  Check if there is a target
            if (TargetTransform == null)
            {
                Debug.LogWarning(name + " has no target to follow.");
            }

            _edgeVectors = new Vector3[4];
#endif

            ////  Set the initial values
            originalValues = transform.position;

            cachedCamera = GetComponentInChildren<Camera>();
            cameraUnitDepth = cachedCamera.orthographicSize;

            CalculateExtents();

            isInitialized = true;
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
            if (TargetTransform == null || !isInitialized)
            {
                return;
            }

            // Calculates the edges of the screen and calculates a dynamic offset based on them
            // This makes the player ship drift towards the edges of the screen opposite of the direction of movement
            CalculateExtents();
            var doffsetTarget =
                new Vector3(
                    TargetTransform.up.normalized.x * (xAxisExtent * DistanceFromMiddleRatio),
                    TargetTransform.up.normalized.y * (yAxisExtent * DistanceFromMiddleRatio),
                    0f);

            // The offset must be dampened towards, or else it will almost immediately set the final target position which is jarring
            // This damping makes the shift towards the edge more gradual, and pleasant
            dynamicOffset = Vector3.SmoothDamp(dynamicOffset, doffsetTarget, ref dynamicOffsetVelocity, _dOffsetSmoothTime);

            // Smoothly interpolates the camera's position towards the position of the target
            // Adjusts for editor-side adjustment of the cameras transform-values with the _originalValues-variable
            transform.position = Vector3.SmoothDamp(
                transform.position,
                TargetTransform.position + originalValues + Offset + dynamicOffset,
                ref followVelocity,
                _followSmoothTime);
        }

#if DEBUG && UNITY_EDITOR
        /// <summary>
        /// Draws the cross in editor during debug-mode.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!isInitialized)
            {
                return;
            }

            Gizmos.DrawWireSphere(screenMiddle, 1f);
            foreach (var edge in _edgeVectors)
            {
                Gizmos.DrawLine(edge, screenMiddle);
            }
        }
#endif

        private void CalculateExtents()
        {
            cameraUnitDepth = Mathf.Abs(transform.position.z);
            screenMiddle = cachedCamera.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, cameraUnitDepth));

            xAxisExtent = Vector3.Distance(
                screenMiddle,
                cachedCamera.ViewportToWorldPoint(new Vector3(1f, .5f, cameraUnitDepth)));

            yAxisExtent = Vector3.Distance(
                screenMiddle,
                cachedCamera.ViewportToWorldPoint(new Vector3(.5f, 1f, cameraUnitDepth)));

#if DEBUG && UNITY_EDITOR
            // Used to draw the cross in the middle of the screen in debug-mode, in editor
            for (var i = 0; i < _edgeVectors.Length; i++)
            {
                switch (i)
                {
                    case 0: // top
                        _edgeVectors[i] = cachedCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, cameraUnitDepth));
                        break;
                    case 1: // right
                        _edgeVectors[i] = cachedCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, cameraUnitDepth));
                        break;
                    case 2: // bottom
                        _edgeVectors[i] = cachedCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, cameraUnitDepth));
                        break;
                    case 3: // left
                        _edgeVectors[i] = cachedCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, cameraUnitDepth));
                        break;
                }
            }
#endif
        }
    }
}
