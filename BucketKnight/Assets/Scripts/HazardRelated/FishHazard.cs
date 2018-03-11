// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FishHazard.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class FishHazard : HazardScript
    {
        public int MinScale = 1;
        public int MaxScale = 4;

        public bool RotateOnSpawn;
        private bool _jumped;

        private float timer;

        public AudioClip splashSound;
        public AudioSource audioSource;

        public FishWaterSplash splashParticles;
        public FishAnimController fishAnimController;

        protected override void OnEnable()
        {
            base.OnEnable();
            _jumped = false;
            SetRandomSpeed();
        }

        protected override void Start()
        {
            base.Start();
            timer = Random.Range(0f, 0.5f);
            SetRandomSpeed();
        }

        protected override void MoveToNewPosition()
        {
            base.MoveToNewPosition();

            if (transform.position.y < -1)
            {
                transform.position = new Vector3(transform.position.x, -1.3f, transform.position.z);
            }

            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }

            if (!_jumped && timer <= 0)
            {
                Jump();
            }
        }

        public void Jumping()
        {
            if (!isDealingDamage)
            {
                isDealingDamage = true;
            }

            splashParticles.ActivateParticles();
        }

        public void NotJumping()
        {
            if (isDealingDamage)
            {
                isDealingDamage = false;
            }
        }

        private void Jump()
        {
            fishAnimController.Jump();
            _jumped = true;
            audioSource.clip = splashSound;
            audioSource.pitch = Random.Range(0.7f, 1f);
            audioSource.Play();
        }

        private void ToTheSurface(float height)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(0, height, 0);
        }
    }
}
