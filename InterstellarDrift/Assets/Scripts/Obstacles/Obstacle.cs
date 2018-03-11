// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Obstacle.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    ///  A self-recycling obstacle.
    ///  Uses an IEnumerator to indefinitely check if it is within its allowed bounds. If false, calls for any listeners to recycle.
    /// </summary>
    public class Obstacle : MonoBehaviour, ISpawnable
    {
        [SerializeField] private Collider2D _obstacleCollider2D;
        [SerializeField] private float _approximateRadius = 5f;

        private ObjectPooler objectPooler;
        private IEnumerator areaCheckCoroutine;
        private bool isRecycling;
        private bool isInitialized;

        private Collider2D allowedArea;

        public Collider2D ObstacleCollider2D => _obstacleCollider2D;
        public float ApproximateRadius => _approximateRadius;

        public void SetAllowedArea(Collider2D area)
        {
            allowedArea = area;
        }

        public void RecycleSelf()
        {
            if (objectPooler == null)
            {
#if DEBUG
                Debug.LogWarning(name + " hos no reference to the object pooler. Destroying instead of recycling.");
#endif
                Destroy(gameObject);
                return;
            }

            objectPooler.Recycle(gameObject);
        }

        void ISpawnable.OnSpawned(ObjectPooler pooler)
        {
            objectPooler = pooler;
        }

        void ISpawnable.OnRecycled()
        {
            if (areaCheckCoroutine != null)
            {
                StopCoroutine(areaCheckCoroutine);
                areaCheckCoroutine = null;
            }

            allowedArea = null;
            isRecycling = false;
            isInitialized = false;
        }

        private void Awake()
        {
            if (!_obstacleCollider2D)
            {
                _obstacleCollider2D = GetComponentInChildren<Collider2D>();
            }
        }

        private void Update()
        {
            // If we don't have an allowed area, recycle yourself.
            if (!allowedArea)
            {
                RecycleSelf();
                return;
            }

            // If we have already initialized, don't do anything.
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;
            areaCheckCoroutine = CheckIfInsideArea();
            StartCoroutine(areaCheckCoroutine);
        }

        private IEnumerator CheckIfInsideArea()
        {
            while (allowedArea && !isRecycling && _obstacleCollider2D)
            {
                if (!ObstacleCollider2D.IsTouching(allowedArea))
                {
                    isRecycling = true;
                    RecycleSelf();
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
