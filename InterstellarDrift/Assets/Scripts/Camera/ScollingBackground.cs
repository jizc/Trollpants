// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScollingBackground.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class ScollingBackground : MonoBehaviour
    {
        public Camera BackgroundCamera;
        public float Speed = 1;

        private MeshRenderer background;
        private Material space;
        private Vector3 oldCameraPosistion;

        private void Awake()
        {
            background = GetComponent<MeshRenderer>();
            space = background.material;
        }

        private void Update()
        {
            var newCameraPosistion = BackgroundCamera.transform.position;
            var deltaPosistion = newCameraPosistion - oldCameraPosistion;
            space.mainTextureOffset += (Vector2)deltaPosistion.normalized * Time.deltaTime * Speed;
            oldCameraPosistion = BackgroundCamera.transform.position;
        }
    }
}
