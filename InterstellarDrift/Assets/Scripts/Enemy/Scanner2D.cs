// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scanner2D.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///  Detects gameobjects on the targetMask-layer, and checks if there's a clear view of the target.
    ///  Notifies listeners of the OnTargetAcquried-event when a target is found, and stops detecting.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Scanner2D : MonoBehaviour, ISpawnable
    {
        public TransformEvent OnTargetFound;

        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstacleMask;

        private bool foundTarget;

        void ISpawnable.OnSpawned(ObjectPooler pooler)
        {
            foundTarget = false;
        }

        void ISpawnable.OnRecycled()
        {
        }

        private void Awake()
        {
            // Assure that it is set to be a trigger.
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (foundTarget)
            {
                return;
            }

            // If gameobject is on targeted layer
            if ((1 << col.gameObject.layer & _targetMask) != 0)
            {
                // If no obstacles were hit when trying to "see" the target, we can see it!
                if (!Physics2D.Linecast(transform.position, col.transform.position, _obstacleMask).collider)
                {
                    foundTarget = true;
                    OnTargetFound.Invoke(col.transform);
                }
            }
        }
    }

    [Serializable]
    public class TransformEvent : UnityEvent<Transform>
    {
    }
}
