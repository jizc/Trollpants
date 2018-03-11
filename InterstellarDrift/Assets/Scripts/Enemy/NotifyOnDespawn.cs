// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyOnDespawn.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using UnityEngine.Events;

    public class NotifyOnDespawn : MonoBehaviour, ISpawnable
    {
        public UnityEvent OnDespawn;

        void ISpawnable.OnSpawned(ObjectPooler pooler)
        {
        }

        void ISpawnable.OnRecycled()
        {
            OnDespawn.Invoke();
        }
    }
}
