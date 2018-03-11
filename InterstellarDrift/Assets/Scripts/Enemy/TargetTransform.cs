// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetTransform.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using UnityEngine.Events;

    public class TargetTransform : MonoBehaviour
    {
        public UnityEvent OnTargetAcquired;
        public UnityEvent OnTargetLost;

        [SerializeField] private Transform _target;

        public Transform Target
        {
            get { return _target; }
            set
            {
                _target = value;

                if (_target)
                {
                    OnTargetAcquired.Invoke();
                }
            }
        }

        public void ClearTarget()
        {
            Target = null;
            OnTargetLost.Invoke();
        }
    }
}
