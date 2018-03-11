// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatedUVs.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class AnimatedUVs : MonoBehaviour
    {
        public int materialIndex;
        public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
        public string textureName = "_MainTex";
        private Vector2 _uvOffset = Vector2.zero;

        private void FixedUpdate()
        {
            _uvOffset += uvAnimationRate * Time.deltaTime;
            if (GetComponent<Renderer>().enabled)
            {
                GetComponent<Renderer>().materials[materialIndex].SetTextureOffset(textureName, _uvOffset);
            }
        }
    }
}
