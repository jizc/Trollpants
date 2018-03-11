// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovingObject.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class MovingObject : MonoBehaviour
    {
        public bool StuckWithBackground;
        public bool MoveReverse;
        public float MinSpeed;
        public float MaxSpeed;
        public float Speed;
        public float RandomSpeed;

        protected Transform objectPooler;

        public virtual void Despawn()
        {
            gameObject.transform.SetParent(objectPooler);
            gameObject.SetActive(false);
        }

        public virtual void Reset()
        {
            Despawn();
        }

        public void SetSpeed()
        {
            Speed = GameObject.FindWithTag("Player").GetComponent<PlayerStats>().Speed;
            if (!MoveReverse)
            {
                Speed *= -1;
            }
        }

        protected virtual void MoveToNewPosition()
        {
            var pos = transform.position;
            pos.z += (Speed + RandomSpeed) * Time.deltaTime;
            transform.position = pos;
        }

        protected void SetRandomSpeed()
        {
            RandomSpeed = Random.Range(MinSpeed, MaxSpeed);
        }

        protected virtual void Start()
        {
            RandomSpeed = 0;
            SetSpeed();

            objectPooler = GameObject.Find("ObjectPooler").transform;
        }

        private void FixedUpdate()
        {
            if (!SavedData.GameIsPaused)
            {
                MoveToNewPosition();
            }
        }
    }
}
