// <copyright file="TransformRotator.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using Data;
    using UnityEngine;

    public class TransformRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 rotation;

        private void Update()
        {
            if (GameState.IsPaused)
            {
                return;
            }

            transform.Rotate(rotation * Time.deltaTime, Space.Self);
        }
    }
}
