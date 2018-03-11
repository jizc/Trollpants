// <copyright file="AutoRotate.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.World
{
    using UnityEngine;

    public class AutoRotate : MonoBehaviour
    {
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Space space = Space.Self;

        private void LateUpdate()
        {
            transform.Rotate(rotation * Time.deltaTime, space);
        }
    }
}
