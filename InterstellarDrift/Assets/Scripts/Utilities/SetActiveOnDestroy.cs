// <copyright file="SetActiveOnDestroy.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Sets the active state of target gameobjects.
    /// </summary>
    public class SetActiveOnDestroy : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private bool _activeState;

        public GameObject Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public bool ActiveState
        {
            get { return _activeState; }
            set { _activeState = value; }
        }

        private void OnDestroy()
        {
            if (!Target)
            {
                Debug.LogWarning(this + " has no Target.");
                return;
            }

            Target.SetActive(ActiveState);
        }
    }
}
