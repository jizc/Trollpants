// <copyright file="WorldSlice.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class WorldSlice : MonoBehaviour
    {
        public List<WorldTile> Tiles { get; private set; }

        public int GroundHeight { get; set; }

        private void Awake()
        {
            Tiles = new List<WorldTile>();
        }

        private void Start()
        {
            Tiles.AddRange(GetComponentsInChildren<WorldTile>().ToList().OrderBy(t => t.TileNum));
        }
    }
}
