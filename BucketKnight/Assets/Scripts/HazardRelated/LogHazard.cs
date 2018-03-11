// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHazard.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class LogHazard : HazardScript
    {
        private float _randomnum;

        public int AnimationSlowDown = 4;
        public float minScale = 1;
        public float maxScale = 1;
        private float _origYPos;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetRandomSpeed();
        }

        protected override void Start()
        {
            base.Start();
            _randomnum = Random.Range(1, 200);

            if (Random.Range(-5, 5) > 2)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                var newSize = Random.Range(minScale, maxScale);
                transform.localScale = new Vector3(newSize, 1, 1);

                GetComponent<Rigidbody>().mass = 2 * newSize;
            }
            _origYPos = transform.position.y;
            SetRandomSpeed();
        }

        protected override void MoveToNewPosition()
        {
            base.MoveToNewPosition();
            transform.position = new Vector3(transform.position.x,
                Mathf.PingPong(Time.time / AnimationSlowDown + _randomnum, _origYPos), transform.position.z);
        }
    }
}
