// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyOnCollide.cs" company="Jan Ivar Z. Carlsen">
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
    ///  Sends an event out to listeners when collisions with objects on the chosen layers occur.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class NotifyOnCollide : MonoBehaviour
    {
        public UnityEvent OnCollisionEnter;
        public UnityEvent OnCollisionStay;
        public UnityEvent OnCollisionExit;

        [SerializeField] private LayerMask _collisionLayers;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((1 << other.gameObject.layer & _collisionLayers) != 0)
            {
                OnCollisionEnter.Invoke();
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if ((1 << other.gameObject.layer & _collisionLayers) != 0)
            {
                OnCollisionStay.Invoke();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if ((1 << other.gameObject.layer & _collisionLayers) != 0)
            {
                OnCollisionExit.Invoke();
            }
        }
    }

    [Serializable]
    public class Collision2DEvent : UnityEvent<Collider2D>
    {
    }
}
