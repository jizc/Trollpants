// <copyright file="DifficultyInfo.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Data
{
    using UnityEngine;

    public static class DifficultyInfo
    {
        private const float maxLevel = 25f;

        public static bool SpawnTrees
        {
            get { return Percent(Mathf.Lerp(10f, 25f, GameState.DifficultyLevel / maxLevel)); }
        }

        public static bool SpawnGraves
        {
            get { return GameState.DifficultyLevel > 1 && SpawnTrees; }
        }

        public static bool SpawnGhosts
        {
            get
            {
                return GameState.DifficultyLevel > 1
                    && Percent(Mathf.Lerp(4f, 20f, GameState.DifficultyLevel / maxLevel));
            }
        }

        public static bool SpawnSpiders
        {
            get
            {
                return GameState.DifficultyLevel > 2
                    && Percent(Mathf.Lerp(4f, 20f, GameState.DifficultyLevel / maxLevel));
            }
        }

        public static bool SpawnWebs
        {
            get
            {
                return GameState.DifficultyLevel > 4
                    && Percent(Mathf.Lerp(5f, 20f, GameState.DifficultyLevel / maxLevel));
            }
        }

        public static int HazardBuffer
        {
            get { return GameState.DifficultyLevel < 11 ? 3 : 2; }
        }

        public static int MaxGroundHeight
        {
            get { return GameState.DifficultyLevel > 4 ? 3 : 2; }
        }

        private static bool Percent(float percent)
        {
            return Random.Range(0, 100f) >= 100f - percent;
        }
    }
}
