// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Planetoid.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Handles particle system activation/deactivation for the planetoids.
    /// </summary>
    public class Planetoid : MonoBehaviour, ISpawnable
    {
        [SerializeField] private float _planetoidMaxForce = 2f;
        [SerializeField] private float _planetoidMaxTorque = 5f;

        private Rigidbody2D cachedRigidBody2D;

        void ISpawnable.OnSpawned(ObjectPooler pooler)
        {
            if (!cachedRigidBody2D)
            {
#if DEBUG
                Debug.LogWarning("No cached rigidbody2D. Is this script on a gameobject with a rigidbody2D on it?");
#endif

                return;
            }

            Invoke("Init", 0.2f);
        }

        void ISpawnable.OnRecycled()
        {
        }

        private void Awake()
        {
            cachedRigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Init()
        {
            cachedRigidBody2D.AddForce(new Vector2(Random.Range(-_planetoidMaxForce, _planetoidMaxForce), Random.Range(-_planetoidMaxForce, _planetoidMaxForce)), ForceMode2D.Impulse);
            cachedRigidBody2D.AddTorque(Random.Range(-_planetoidMaxTorque, _planetoidMaxTorque), ForceMode2D.Impulse);
        }
    }
}
