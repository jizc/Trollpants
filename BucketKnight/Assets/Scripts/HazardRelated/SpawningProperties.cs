// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpawningProperties.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class SpawningProperties : MonoBehaviour
    {
        public float maxX;
        public float minX;
        public float maxY;
        public float minY;

        public float spawningFrequency;

        public bool rotateOnSpawn;

        public bool rareSpawn;
    }
}
