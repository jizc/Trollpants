// <copyright file="WorldTilePool.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using UnityEngine;

    public class WorldTilePool
    {
        private const string worldTilesPath = "WorldTiles/";
        private readonly Transform transform;

        public WorldTilePool(Transform transform)
        {
            this.transform = transform;
            Initialize();
        }

        public void Spawn(WorldTile worldTile, string prefabName)
        {
            var poolGameObject = transform.Find(prefabName + "s");
            GameObject gameObject;

            if (poolGameObject != null && poolGameObject.childCount > 0)
            {
                gameObject = poolGameObject.GetChild(0).gameObject;
            }
            else
            {
                gameObject = Object.Instantiate(Resources.Load<GameObject>(worldTilesPath + prefabName));

                if (gameObject != null)
                {
                    gameObject.name = prefabName;
                }
            }

            if (gameObject == null)
            {
                return;
            }

            var rb2D = gameObject.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                rb2D.WakeUp();
            }

            gameObject.transform.parent = worldTile.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);

            worldTile.Content = gameObject;

            if (gameObject.name == "Ghost")
            {
                gameObject.GetComponent<GhostPatrolBehaviour>().ResetDistance();
            }
        }

        public void FillWeb(WorldTile worldTile)
        {
            if (worldTile.Web != null)
            {
                return;
            }

            const string prefabName = "WebFill";
            var poolGameObject = transform.Find(prefabName + "s");
            GameObject gameObject;

            if (poolGameObject != null && poolGameObject.childCount > 0)
            {
                gameObject = poolGameObject.GetChild(0).gameObject;
            }
            else
            {
                gameObject = Object.Instantiate(Resources.Load<GameObject>(worldTilesPath + prefabName));

                if (gameObject != null)
                {
                    gameObject.name = prefabName;
                }
            }

            if (gameObject == null)
            {
                return;
            }

            gameObject.transform.parent = worldTile.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);

            worldTile.Web = gameObject;
        }

        public void RemoveContent(WorldTile worldTile)
        {
            if (worldTile == null)
            {
                return;
            }

            if (worldTile.Content != null)
            {
                Recycle(worldTile.Content);
            }

            worldTile.Content = null;
            worldTile.WorldTileType = WorldTileType.Empty;
            if (worldTile.Web != null)
            {
                Recycle(worldTile.Web);
                worldTile.Web = null;
            }
        }

        private void Recycle(GameObject gameObject)
        {
            if (gameObject == null)
            {
                Debug.LogError("The object you want to recycle is null.");
                return;
            }

            var rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.Sleep();
            }

            var gameObjectTypePool = transform.Find(gameObject.name + "s");
            var t = gameObject.transform;

            gameObject.SetActive(false);

            if (gameObjectTypePool != null)
            {
                t.parent = gameObjectTypePool;
                t.localPosition = Vector3.zero;
            }
            else
            {
                var newGameObject = new GameObject { name = gameObject.name + "s" };
                var ngot = newGameObject.transform;
                ngot.parent = transform;
                ngot.localPosition = Vector3.zero;

                gameObjectTypePool = ngot;
                t.parent = gameObjectTypePool;
                t.localPosition = Vector3.zero;
            }
        }

        private void Initialize()
        {
            PreLoadTiles("Flat0", 5);
            PreLoadTiles("Flat1", 5);
            PreLoadTiles("Flat2", 5);
            PreLoadTiles("Coin", 10);
            PreLoadTiles("Gem", 2);
            PreLoadTiles("RampUp0", 4);
            PreLoadTiles("RampUp1", 4);
            PreLoadTiles("RampUp2", 4);
            PreLoadTiles("RampDown0", 4);
            PreLoadTiles("RampDown1", 4);
            PreLoadTiles("RampDown2", 4);
            PreLoadTiles("Ghost", 5);
            PreLoadTiles("GhostDummy", 5);
            PreLoadTiles("Grave0", 5);
            PreLoadTiles("Grave1", 5);
            PreLoadTiles("Grave2", 5);
            PreLoadTiles("Spider", 5);
            PreLoadTiles("Tree0", 4);
            PreLoadTiles("Tree1", 4);
            PreLoadTiles("Tree2", 4);
            PreLoadTiles("Tree3", 4);
            PreLoadTiles("TreeDummy", 6);
            PreLoadTiles("Web", 20);
            PreLoadTiles("WebFill", 6);
            PreLoadTiles("WebTop", 6);
        }

        private void PreLoadTiles(string tileName, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var gameObject = Object.Instantiate(Resources.Load<GameObject>(worldTilesPath + tileName));
                if (gameObject != null)
                {
                    gameObject.transform.SetParent(transform);
                    gameObject.name = tileName;
                    Recycle(gameObject);
                }
            }
        }
    }
}
