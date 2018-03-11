// <copyright file="ToggleColliders.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun
{
    using UnityEngine;

    public class ToggleColliders : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;

        private void OnEnable()
        {
            _collider.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            _collider.enabled = false;
        }
    }
}
