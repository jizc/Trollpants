// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeaverHazard.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class BeaverHazard : HazardScript
    {
        public float xRightPos;
        public float xLeftPos;
        public float yPos;
        public AudioClip fallSound;
        public AudioSource audioSource;

        protected override void OnEnable()
        {
            base.OnEnable();
            var newpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (transform.position.x >= 0)
            {
                newpos.x = xRightPos;
            }
            else
            {
                newpos.x = xLeftPos;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            newpos.y = yPos;

            transform.position = newpos;

            audioSource.clip = fallSound;
            audioSource.Play();
        }
    }
}
