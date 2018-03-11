// <copyright file="Destroyer.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.World
{
    using UnityEngine;

    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ObjectPooler objectPooler;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                gameManager.PlayerDeath();
                return;
            }

            objectPooler.Recycle(col.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            objectPooler.Recycle(col.gameObject);
        }
    }
}
