// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Avoidance.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    [RequireComponent(typeof(Collider2D))]
    public class Avoidance : MonoBehaviour
    {
        public Vector2Event OnSafePointCalculated;

        [SerializeField] private LayerMask _obstacleMask;

        private Vector2 currentDestination = Vector2.zero;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _obstacleMask) != 0)
            {
                currentDestination += (Vector2)(transform.position - other.transform.position);
            }
        }

        private void LateUpdate()
        {
            // Add the current position of the transform to offset the destination in world space.
            currentDestination = (currentDestination.normalized * 15) + (Vector2)transform.position;
            OnSafePointCalculated.Invoke(currentDestination);
            currentDestination = Vector2.zero;
        }
    }

    [Serializable]
    public class Vector2Event : UnityEvent<Vector2>
    {
    }
}
