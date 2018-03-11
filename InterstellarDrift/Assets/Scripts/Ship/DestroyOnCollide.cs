// <copyright file="DestroyOnCollide.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Reloads the scene when a collision occurs.
    /// </summary>
    public class DestroyOnCollide : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionLayers;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((1 << other.gameObject.layer & _collisionLayers) != 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
