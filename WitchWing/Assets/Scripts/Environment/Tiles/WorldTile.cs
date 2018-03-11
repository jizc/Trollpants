// <copyright file="WorldTile.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System.Linq;
    using UnityEngine;

    public enum WorldTileType
    {
        Empty,
        Ground,
        Flat,
        Tilted,
        Obstacle,
        Pickup
    }

    public class WorldTile : MonoBehaviour
    {
        #region Fields & properties

#if UNITY_EDITOR
        [SerializeField]
#endif
        private GameObject content;

#if UNITY_EDITOR
        [SerializeField]
#endif
        private WorldTileType worldTileType = WorldTileType.Empty;

        private Transform cachedTransform;
        private GameObject web;

        public GameObject Web
        {
            get { return web; }
            set
            {
                web = value;
                if (web != null)
                {
                    web.transform.parent = cachedTransform;
                }
            }
        }

        public GameObject Content
        {
            get { return content; }
            set
            {
                content = value;
                if (content != null)
                {
                    WorldTileType = ParseTileType(content.name);
                }
            }
        }

        // Vertical position in slice
        public int TileNum { get; private set; }

        public WorldTileType WorldTileType
        {
            get { return worldTileType; }
            set { worldTileType = value; }
        }

        #endregion /Fields & properties

        private static WorldTileType ParseTileType(string tileTypeString)
        {
            var x = new string(tileTypeString.Reverse().SkipWhile(char.IsDigit).Reverse().ToArray());
            switch (x)
            {
                case "Ground":
                    return WorldTileType.Ground;

                case "Flat":
                    return WorldTileType.Flat;

                case "RampDown":
                case "RampUp":
                    return WorldTileType.Tilted;

                case "Coin":
                case "Gem":
                case "MailBox":
                case "MailLazer":
                    return WorldTileType.Pickup;
                case "Tree":
                case "TreeDummy":
                case "Grave":
                case "Ghost":
                case "GhostDummy":
                case "Spider":
                case "Web":
                case "WebTop":
                    return WorldTileType.Obstacle;
                default:
                    Debug.LogWarning(string.Format("Parsing {0} to {1} failed returning default", x, typeof(WorldTileType).Name));
                    return WorldTileType.Obstacle;
            }
        }

        private void Awake()
        {
            TileNum = int.Parse(name.Substring(name.Length - 1));
            cachedTransform = transform;
        }
    }
}
