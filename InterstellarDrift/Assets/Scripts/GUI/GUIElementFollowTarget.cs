// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GUIElementFollowTarget.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    /// Makes a Canvas-element move towards a target point in the world.
    /// Options to make the element linger at the target or be destroyed; be clamped at screen-edges.
    /// </summary>
    public class GUIElementFollowTarget : MonoBehaviour
    {
        public Transform TargetTransform;

        private readonly Vector3[] corners = new Vector3[4];

        [SerializeField] private float _smoothTime = 0.125f;
        [SerializeField] private bool _deactivateAtTarget;
        [SerializeField] private float _deactivateDistance = 5f;
        [SerializeField] private bool _clampAtScreenEdge;

        private bool currentlyActive;
        private Canvas cachedCanvas;
        private Vector3 smoothingVelocity;

        public bool DeactivateAtTarget
        {
            get { return _deactivateAtTarget; }
            set { _deactivateAtTarget = value; }
        }

        public bool CurrentlyActive
        {
            get { return currentlyActive; }
            set
            {
                currentlyActive = value;

                for (var i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(currentlyActive);
                }
            }
        }

        public bool ClampAtScreenEdge
        {
            get { return _clampAtScreenEdge; }
            set { _clampAtScreenEdge = value; }
        }

        public void Init(Transform target, bool clampAtScreenEdge, bool deactivateAtTarget)
        {
            TargetTransform = target;
            ClampAtScreenEdge = clampAtScreenEdge;
            DeactivateAtTarget = deactivateAtTarget;
            CurrentlyActive = true;
            cachedCanvas = GetComponentInParent<Canvas>();
        }

        private void Awake()
        {
            Init(TargetTransform, ClampAtScreenEdge, DeactivateAtTarget);
        }

        private void Update()
        {
            if (TargetTransform == null)
            {
                Destroy(gameObject);
                return;
            }

            // Calculate target to move to
            var targetPosition = TargetTransform.position;

            // Clamp target if active
            if (ClampAtScreenEdge)
            {
                targetPosition = ClampTargetToScreen(targetPosition);
            }

            // Move transform
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref smoothingVelocity,
                _smoothTime);

            // Deactivate children if within deactivation distance and active
            if (DeactivateAtTarget)
            {
                var distance = Vector2.Distance(transform.position, TargetTransform.position);
                if (distance <= _deactivateDistance)
                {
                    if (CurrentlyActive)
                    {
                        CurrentlyActive = false;
                    }
                }
                else
                {
                    if (!CurrentlyActive)
                    {
                        CurrentlyActive = true;
                    }
                }
            }
        }

        private Vector3 ClampTargetToScreen(Vector3 targetScreenPosition)
        {
            // 1. Calculate camera extents
            var cameraDepth = Mathf.Abs(cachedCanvas.worldCamera.transform.position.z);
            var cameraMin = cachedCanvas.worldCamera.ViewportToWorldPoint(new Vector3(0, 0, cameraDepth));
            var cameraMax = cachedCanvas.worldCamera.ViewportToWorldPoint(new Vector3(1f, 1f, cameraDepth));

            // 2. Adjust for rect-transform world-size
            var rectTransform = transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.GetWorldCorners(corners); // index-0: bottomLeft; index-1: topLeft; index-2: topRight; index-3: bottomRight
            }

            var halfSize = new Vector3(Vector3.Distance(corners[0], corners[3]) / 2, Vector3.Distance(corners[1], corners[2]) / 2, 0);

            cameraMin += halfSize;
            cameraMax -= halfSize;

            // 3. Clamp targetPosition
            if (targetScreenPosition.x < cameraMin.x)
            {
                targetScreenPosition.x = cameraMin.x;
            }
            else if (targetScreenPosition.x > cameraMax.x)
            {
                targetScreenPosition.x = cameraMax.x;
            }

            if (targetScreenPosition.y < cameraMin.y)
            {
                targetScreenPosition.y = cameraMin.y;
            }
            else if (targetScreenPosition.y > cameraMax.y)
            {
                targetScreenPosition.y = cameraMax.y;
            }

            return targetScreenPosition;
        }
    }
}
