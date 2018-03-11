// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaterAnimator.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class WaterAnimator : MonoBehaviour
    {
        public float scrollSpeed = 0.5f;

        private void Update()
        {
            var offset = Time.time * scrollSpeed;
            GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
        }
    }
}
