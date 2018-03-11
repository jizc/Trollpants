// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentMovement.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class EnvironmentMovement : MovingObject
    {
        public Transform spawnTransform;
        public Vector3 origPos;

        private void OnEnable()
        {
            Events.instance.AddListener<SpeedChanged>(OnSpeedChanged);
        }

        protected override void Start()
        {
            origPos = transform.position;
            base.Start();
        }

        public void DestroySelf()
        {
            transform.position = spawnTransform.position;
        }

        public override void Reset()
        {
            transform.position = origPos;
        }

        public override void Despawn()
        {
            transform.position = spawnTransform.position;
        }

        public void OnSpeedChanged(SpeedChanged speedChanged)
        {
            if (StuckWithBackground)
            {
                return; // <-- water will not be affected by speed changes
            }

            Speed = -speedChanged.newPlayerSpeed;
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<SpeedChanged>(OnSpeedChanged);
        }
    }
}
