// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnemyController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class EnemyController : BaseController
    {
        public GameObject player;

        public static float AngleDir(Vector2 a, Vector2 b)
        {
            return (-a.x * b.y) + (a.y * b.x);
        }

        private void Start()
        {
            SetMaxVelocity(300f);
            player = GameObject.FindWithTag("Player");
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (!player)
            {
                return;
            }

            MoveForward();

            Vector2 relativePoint = transform.InverseTransformPoint(player.transform.position);

            if (relativePoint.x < -5.0)
            {
                MoveLeft();
            }
            else if (relativePoint.x > 5.0)
            {
                MoveRight();
            }

            BaseFixedUpdate();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Destroy(col.gameObject);
            }
        }
    }
}
