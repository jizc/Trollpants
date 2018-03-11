// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnemyShipSetup.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class EnemyShipSetup : MonoBehaviour, ISpawnable
    {
        public GameObject Scanner;
        public TargetTransform Target;

        private Rigidbody2D cachedRigidbody2D;
        private EnemyDriver cachedDriver;

        void ISpawnable.OnSpawned(ObjectPooler pooler)
        {
            if (Target)
            {
                Target.Target = null;
            }
#if DEBUG
            else
            {
                Debug.LogWarning(name + " is missing a reference to " + Target);
            }
#endif

            if (Scanner)
            {
                Scanner.SetActive(true);
            }
#if DEBUG
            else
            {
                Debug.LogWarning(name + " is missing a reference to " + Scanner);
            }
#endif

            if (cachedRigidbody2D)
            {
                cachedRigidbody2D.AddTorque(Random.Range(-5, 5f));
            }
#if DEBUG
            else
            {
                Debug.LogWarning(name + " is missing a reference to " + cachedRigidbody2D);
            }
#endif

            if (cachedDriver)
            {
                cachedDriver.TargetPoint = cachedDriver.transform.up; // Up is forward in 2d
            }
#if DEBUG
            else
            {
                Debug.LogWarning(name + " is missing a reference to " + cachedDriver);
            }
#endif
        }

        void ISpawnable.OnRecycled()
        {
        }

        private void Awake()
        {
            cachedRigidbody2D = GetComponent<Rigidbody2D>();
            cachedDriver = GetComponent<EnemyDriver>();

            if (!Target)
            {
                Target = GetComponent<TargetTransform>();
            }
        }
    }
}
