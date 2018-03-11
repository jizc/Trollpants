// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoRotate.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Trollpants.UtilityBelt.Misc
{
    using UnityEngine;

    public class AutoRotate : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private Space _space = Space.Self;

        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Space Space
        {
            get { return _space; }
            set { _space = value; }
        }

        private void Update()
        {
            transform.Rotate(_rotation * Time.deltaTime, _space);
        }
    }
}
