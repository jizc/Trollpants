// <copyright file="WorldGenerator.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Effects;
    using GUI;
    using Player;
    using UnityEngine;

    public class WorldGenerator : MonoBehaviour
    {
        private const int mailFreeSpaces = 6;
        private const int playerOffset = 12;
        private const int mailIntervalDistance = 175;

        private const float coinChance = 6f;
        private const float gemChance = 0.5f;

        [SerializeField] private MidasEffectSpawner effectSpawner;

        private Transform playerTransform;
        private WorldTilePool worldTilePool;
        private Queue<WorldSlice> sliceQueue;
        private Transform slice0Transform;
        private WorldSlice lastWorldSlice;

        private bool timeForMail;
        private int mailTimeCount;
        private int nextMailTimeDistance;
        private int ghostBuffer;
        private int spiderBuffer;
        private int webBuffer;

        public void ResetWorld()
        {
            timeForMail = false;
            mailTimeCount = 0;
            nextMailTimeDistance = mailIntervalDistance;
            ghostBuffer = 0;
            spiderBuffer = 0;
            webBuffer = 0;

            var slices = sliceQueue.ToArray();
            slice0Transform = slices[0].transform;
            lastWorldSlice = slices.Last();
            sliceQueue.Clear();
            var xPosition = -7;
            foreach (var slice in slices)
            {
                foreach (var tile in slice.Tiles)
                {
                    worldTilePool.RemoveContent(tile);
                }

                var bottomTile = slice.Tiles.FirstOrDefault(t => t.TileNum == 0);
                if (bottomTile != null)
                {
                    worldTilePool.Spawn(bottomTile, "Flat" + Random.Range(0, 3));
                }

                slice.GroundHeight = 0;
                slice.transform.localPosition = new Vector3(xPosition++, 0f, 0f);
                sliceQueue.Enqueue(slice);
            }
        }

        public void RemoveContent(WorldTile worldTile)
        {
            worldTilePool.RemoveContent(worldTile);
        }

        public void MidasEffect()
        {
            var slices = sliceQueue.ToArray();
            foreach (var slice in slices)
            {
                foreach (var tile in slice.Tiles)
                {
                    // Skip ghost dummies, spiders and spiderwebs
                    if (MidasSkipReplace(tile))
                    {
                        continue;
                    }

                    effectSpawner.Spawn(tile.transform.position);

                    worldTilePool.RemoveContent(tile);
                    worldTilePool.Spawn(tile, "Coin");
                }
            }
        }

        private static bool MidasSkipReplace(WorldTile t)
        {
            return t.WorldTileType != WorldTileType.Obstacle
                || t.Content.name == "GhostDummy"
                || t.Content.name == "Spider"
                || t.Content.layer == 12
                || t.Web != null;
        }

        private static bool MidasSkipSpawn(WorldTile t)
        {
            return t.WorldTileType != WorldTileType.Obstacle
                || t.Content.name == "GhostDummy"
                || t.Content.name == "Spider"
                || t.Content.layer == 12;
        }

        private static bool Percent(float chance)
        {
            return Random.Range(0, 100f) >= 100f - chance;
        }

        private void Awake()
        {
            worldTilePool = new WorldTilePool(transform);
            sliceQueue = new Queue<WorldSlice>();
            var sliceChildren = GetComponentsInChildren<WorldSlice>()
                .OrderBy(s => s.transform.localPosition.x)
                .ToList();
            lastWorldSlice = sliceChildren.Last();
            foreach (var slice in sliceChildren)
            {
                sliceQueue.Enqueue(slice);
            }

            slice0Transform = sliceQueue.Peek().transform;
        }

        private void Start()
        {
            // Find all bottom tiles and spawn them as flat ground
            foreach (var tile in FindObjectsOfType<WorldTile>().Where(t => t.TileNum == 0))
            {
                worldTilePool.Spawn(tile, "Flat" + Random.Range(0, 3));
            }

            nextMailTimeDistance = mailIntervalDistance;
            playerTransform = Player.Transform;
        }

        private void Update()
        {
            if (GameState.IsPaused || Player.State.IsDead)
            {
                return;
            }

            // When the last trailing slice is more than 4 units away from the player, generate a new slice
            if (slice0Transform.position.x + 4f < playerTransform.localPosition.x)
            {
                GenerateNewSlice();
            }
        }

        private void GenerateNewSlice()
        {
            var newWorldSlice = sliceQueue.Dequeue();
            newWorldSlice.transform.localPosition = new Vector3(lastWorldSlice.transform.localPosition.x + 1f, 0f, 0f);

            GenerateGround(newWorldSlice);

            if (!timeForMail)
            {
                GenerateObstacles(newWorldSlice);
            }
            else
            {
                mailTimeCount--;

                if (mailTimeCount == mailFreeSpaces)
                {
                    worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight + 1], "MailBox");
                    worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight + 2], "MailLazer");
                }

                if (mailTimeCount <= 0)
                {
                    timeForMail = false;
                }
            }

            // Make sure there is a way through the slice
            var passable = false;
            for (var i = 0; i < 7; i++)
            {
                if ((lastWorldSlice.Tiles[i].WorldTileType != WorldTileType.Empty
                     && lastWorldSlice.Tiles[i].WorldTileType != WorldTileType.Pickup
                     && lastWorldSlice.Tiles[i].Content.layer != 12)
                    ||
                    (newWorldSlice.Tiles[i].WorldTileType != WorldTileType.Empty
                     && newWorldSlice.Tiles[i].WorldTileType != WorldTileType.Pickup
                     && newWorldSlice.Tiles[i].Content.layer != 12))
                {
                    continue;
                }

                passable = true;
                break;
            }

            // If not passable, remove something to clear the way
            if (!passable)
            {
                ClearTheWay(newWorldSlice);
            }

            // Midas Potion effect
            if (Player.State.IsMidasActive)
            {
                foreach (var tile in newWorldSlice.Tiles)
                {
                    if (MidasSkipSpawn(tile))
                    {
                        continue;
                    }

                    // If obstacle with webfill, replace with proper web, and continue
                    if (tile.Web != null)
                    {
                        worldTilePool.RemoveContent(tile);
                        worldTilePool.Spawn(tile, "Web");
                        continue;
                    }

                    worldTilePool.RemoveContent(tile);
                    worldTilePool.Spawn(tile, "Coin");
                }
            }

            // Mail time?
            if (Player.State.DistanceTraveled + playerOffset + mailFreeSpaces >= nextMailTimeDistance)
            {
                nextMailTimeDistance += mailIntervalDistance;
                mailTimeCount = mailFreeSpaces * 2;
                timeForMail = true;
            }

            // Add the slice to the queue
            sliceQueue.Enqueue(newWorldSlice);

            // Reset the "first slice" since the queue has changed
            slice0Transform = sliceQueue.Peek().transform;
            lastWorldSlice = newWorldSlice;
        }

        private void GenerateGround(WorldSlice newWorldSlice)
        {
            var prevGroundHeight = lastWorldSlice.GroundHeight;

            newWorldSlice.GroundHeight = timeForMail
                ? prevGroundHeight
                : Mathf.Clamp(
                    Random.Range(prevGroundHeight - 1, prevGroundHeight + 2),
                    0,
                    DifficultyInfo.MaxGroundHeight);

            if (newWorldSlice.GroundHeight > prevGroundHeight &&
                lastWorldSlice.Tiles[newWorldSlice.GroundHeight].WorldTileType == WorldTileType.Tilted)
            {
                newWorldSlice.GroundHeight--;
            }
            else if (newWorldSlice.GroundHeight < prevGroundHeight &&
                     lastWorldSlice.Tiles[newWorldSlice.GroundHeight + 1].WorldTileType == WorldTileType.Tilted)
            {
                newWorldSlice.GroundHeight++;
            }

            // Reset each tile
            foreach (var tile in newWorldSlice.Tiles)
            {
                worldTilePool.RemoveContent(tile);
            }

            // Fill in the "below ground" tiles
            for (var i = 0; i < newWorldSlice.GroundHeight; i++)
            {
                worldTilePool.Spawn(newWorldSlice.Tiles[i], "Ground" + Random.Range(0, 2));
            }

            // Find out which hilltop tile to use
            if (newWorldSlice.GroundHeight < prevGroundHeight)
            {
                worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight + 1], "RampDown" + Random.Range(0, 3));
                worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight], "Ground" + Random.Range(0, 2));
            }
            else if (newWorldSlice.GroundHeight > prevGroundHeight)
            {
                worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight], "RampUp" + Random.Range(0, 3));
            }
            else
            {
                worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight], "Flat" + Random.Range(0, 3));
            }
        }

        private void GenerateObstacles(WorldSlice newWorldSlice)
        {
            // Spawn obstacles on flat ground
            var content = newWorldSlice.Tiles[newWorldSlice.GroundHeight].Content;

            if (content != null &&
                (content.name.StartsWith("Flat") && newWorldSlice.Tiles[newWorldSlice.GroundHeight + 1].Content == null))
            {
                // Trees
                if (DifficultyInfo.SpawnTrees)
                {
                    worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight + 1], "Tree" + Random.Range(0, 4));
                    newWorldSlice.Tiles[newWorldSlice.GroundHeight + 1].Content.transform.Rotate(
                        Random.Range(-4f, 4f),
                        Random.Range(0f, 359f),
                        0f);
                    worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight + 2], "TreeDummy");
                }
                else if (DifficultyInfo.SpawnGraves)
                {
                    // Graves
                    worldTilePool.Spawn(newWorldSlice.Tiles[newWorldSlice.GroundHeight + 1], "Grave" + Random.Range(0, 3));
                    newWorldSlice.Tiles[newWorldSlice.GroundHeight + 1].Content.transform.Rotate(
                        Random.Range(-7f, 7f),
                        Random.Range(0f, 359f),
                        0);
                }
            }

            DecreaseHazardBuffers();

            // Spawn Spiderwebs
            if (DifficultyInfo.SpawnWebs && webBuffer == 0)
            {
                webBuffer = DifficultyInfo.HazardBuffer;

                if (!PlayerSettings.HasSeenWebTutorial)
                {
                    TutorialCoordinator.ActivateWebTutorial();
                }

                foreach (var tile in newWorldSlice.Tiles.Where(t => t.Content == null))
                {
                    worldTilePool.Spawn(tile, tile.TileNum == 6 ? "WebTop" : "Web");
                }

                foreach (var tile in newWorldSlice.Tiles.Where(
                    t => t.Content.name.StartsWith("Ramp")
                    || t.Content.name.StartsWith("Tree")
                    || t.Content.name.StartsWith("Grave")
                    || t.Content.name.StartsWith("Flat")))
                {
                    worldTilePool.FillWeb(tile);
                }
            }

            // Spawn coins
            GenerateCoins(newWorldSlice);

            // Spawn Ghosts
            if (DifficultyInfo.SpawnGhosts && ghostBuffer == 0)
            {
                ghostBuffer = DifficultyInfo.HazardBuffer;
                var extraHeight = (newWorldSlice.Tiles[newWorldSlice.GroundHeight].Content == null) ? 1 : 2;
                var tileIndex = Random.Range(newWorldSlice.GroundHeight + extraHeight, 6);

                if (newWorldSlice.Tiles[tileIndex].Content == null && newWorldSlice.Tiles[tileIndex + 1].Content == null)
                {
                    worldTilePool.Spawn(newWorldSlice.Tiles[tileIndex], "Ghost");
                    worldTilePool.Spawn(newWorldSlice.Tiles[tileIndex + 1], "GhostDummy");
                }
            }

            // Spawn spiders
            if (DifficultyInfo.SpawnSpiders
                && (newWorldSlice.Tiles[6].Content == null)
                && spiderBuffer == 0)
            {
                spiderBuffer = DifficultyInfo.HazardBuffer;
                worldTilePool.Spawn(newWorldSlice.Tiles[6], "Spider");
                if (newWorldSlice.Tiles[6].Content != null)
                {
                    newWorldSlice.Tiles[6].Content.transform.Rotate(0f, Random.Range(-25f, 25f), 0f);
                }
                else
                {
                    Debug.LogError("Tile content was null!");
                }
            }
        }

        private void DecreaseHazardBuffers()
        {
            if (webBuffer > 0)
            {
                webBuffer--;
            }

            if (spiderBuffer > 0)
            {
                spiderBuffer--;
            }

            if (ghostBuffer > 0)
            {
                ghostBuffer--;
            }
        }

        private void ClearTheWay(WorldSlice newWorldSlice)
        {
            // Get the first tile that in the previous slice that is passable
            var tileNum = lastWorldSlice.Tiles.First(
                tile => tile.WorldTileType == WorldTileType.Empty
                    || (tile.Content.layer == 9
                    || tile.Content.layer == 12)).TileNum;

            // Check if there are any webs above the tile to be removed
            var isWebs = tileNum < 6
                && newWorldSlice.Tiles[tileNum + 1].Content != null
                && newWorldSlice.Tiles[tileNum + 1].Content.name.StartsWith("Web");

            // Dummy tiles are used for things that move vertically,
            // or occupy more than two tiles, to mark them as being in
            // use. They are always placed above the actual tile.
            //
            // If dummy tile detected remove the actual tile below
            // because that is the tile that contains the actual
            // obstacle object
            if (newWorldSlice.Tiles[tileNum].Content.name.EndsWith("Dummy"))
            {
                worldTilePool.RemoveContent(newWorldSlice.Tiles[tileNum - 1]);
                if (isWebs)
                {
                    worldTilePool.Spawn(newWorldSlice.Tiles[tileNum - 1], "Web");
                }
            }

            // If a ramp is blocking a path
            // lower the ground height and flatten the ground there... EXPLOIN LAYT0R
            if (newWorldSlice.Tiles[tileNum].Content.name.StartsWith("RampUp"))
            {
                worldTilePool.RemoveContent(newWorldSlice.Tiles[tileNum]);
                worldTilePool.RemoveContent(newWorldSlice.Tiles[tileNum - 1]);
                worldTilePool.Spawn(newWorldSlice.Tiles[tileNum - 1], "Flat" + Random.Range(0, 3));
                newWorldSlice.GroundHeight--;
            }
            else
            {
                // If a tree or ghost is blocking the path remove its
                // dummy tile
                if ((newWorldSlice.Tiles[tileNum].Content.name.StartsWith("Tree")
                    || newWorldSlice.Tiles[tileNum].Content.name == "Ghost")
                    && !newWorldSlice.Tiles[tileNum].Content.name.EndsWith("Dummy"))
                {
                    worldTilePool.RemoveContent(newWorldSlice.Tiles[tileNum + 1]);

                    // If there was a web above fix it so that it reaches all the way down
                    if (
                        tileNum < 5 &&
                        newWorldSlice.Tiles[tileNum + 2].Content != null &&
                        newWorldSlice.Tiles[tileNum + 2].Content.name.StartsWith("Web"))
                    {
                        worldTilePool.Spawn(newWorldSlice.Tiles[tileNum + 1], "Web");
                        isWebs = true;
                    }
                }

                // Remove the tile
                worldTilePool.RemoveContent(newWorldSlice.Tiles[tileNum]);
            }

            if (isWebs)
            {
                worldTilePool.Spawn(newWorldSlice.Tiles[tileNum], "Web");
            }
        }

        private void GenerateCoins(WorldSlice newWorldSlice)
        {
            var skipping = false;
            var skipCount = 2;
            foreach (var tile in newWorldSlice.Tiles.Where(t => t.Content == null))
            {
                if (skipping)
                {
                    ++skipCount;
                    skipping = skipCount < 2;
                    continue;
                }

                if (!Percent(coinChance))
                {
                    continue;
                }

                worldTilePool.Spawn(tile, Percent(gemChance) ? "Gem" : "Coin");

                skipping = true;
                skipCount = 0;
            }
        }
    }
}
