// <copyright file="Scorer.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.World
{
    using Data;
    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    public class Scorer : MonoBehaviour
    {
        [SerializeField] private SessionData sessionData;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.transform.parent.CompareTag("Currency") && col.transform.parent.gameObject.layer != 8)
            {
                sessionData.Score++;
            }

            col.transform.parent.gameObject.layer = 8; // Mark it for death
        }
    }
}
