// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeaverController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class BeaverController : MonoBehaviour
    {
        private Vector3 _pos;

        private void Start()
        {
            _pos = transform.position;

            if (_pos.x < 0)
            {
                _pos.z -= 6;

                transform.rotation = Quaternion.Euler(0, 270, 0);

                transform.position = _pos;
            }
        }
    }
}
