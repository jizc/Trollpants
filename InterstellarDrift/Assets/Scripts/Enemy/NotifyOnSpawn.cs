// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyOnSpawn.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using UnityEngine.Events;

    public class NotifyOnSpawn : MonoBehaviour, ISpawnable
    {
        public UnityEvent OnSpawn;

        void ISpawnable.OnSpawned(ObjectPooler pooler)
        {
            OnSpawn.Invoke();
        }

        void ISpawnable.OnRecycled()
        {
        }
    }
}
