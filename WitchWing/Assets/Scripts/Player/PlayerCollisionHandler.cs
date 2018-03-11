// <copyright file="PlayerCollisionHandler.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Player
{
    using System.Collections;
    using Data;
    using Environment;
    using GUI;
    using UnityEngine;
    using Utils;

    public class PlayerCollisionHandler : MonoBehaviour
    {
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private TimeBonusViewModel timeBonusViewModel;
        [SerializeField] private PopUpSpawner popUpSpawner;

        private Transform playerTransform;
        private bool inCheckpoint;

        private void Start()
        {
            playerTransform = Player.Transform;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (TutorialCoordinator.IsControlTutorialActive || Player.State.IsDead)
            {
                return;
            }

            switch (collision.gameObject.layer)
            {
                // Trees and graves
                case 8:
                // Enemies
                case 10:
                case 18:
                // Ground
                case 14:
                    Player.Kill();
                    FlightCoordinator.OnPlayerDeath();
                    break;

                // Pickup
                case 9:
                    worldGenerator.RemoveContent(collision.GetComponentInParent<WorldTile>());
                    switch (collision.name)
                    {
                        case "Coin":
                            Player.State.Coins += GameState.CoinValue;
                            Player.State.CoinsCollected += GameState.CoinValue;
                            popUpSpawner.SpawnCoin(GameState.CoinValue);
                            AudioClipPlayer.PlayCoin();
                            break;

                        case "Gem":
                            Player.State.Coins += GameState.CoinValue * 10;
                            popUpSpawner.SpawnCoin(GameState.CoinValue * 10);
                            AudioClipPlayer.PlayGem();
                            break;
                    }

                    break;

                // Web
                case 12:
                    if (Player.IsBoosting)
                    {
                        worldGenerator.RemoveContent(collision.GetComponentInParent<WorldTile>());
                    }
                    else
                    {
                        Player.Kill();
                        FlightCoordinator.OnPlayerDeath();
                    }

                    break;

                // Checkpoint
                case 13:
                    if (inCheckpoint)
                    {
                        // Player sometimes collides with the same checkpoint twice
                        return;
                    }

                    inCheckpoint = true;
                    StartCoroutine(WorldScroller.ActiveCheckpointSpeed());
                    StartCoroutine(DelayedSpeedReset());
                    worldGenerator.RemoveContent(collision.GetComponentInParent<WorldTile>());
                    GameState.DifficultyLevel++;
                    GameState.CoinValue += 2;
                    var timeBonus = Player.State.RewardTimeBonus();
                    Player.StartNewTimeBonusDrain();
                    timeBonusViewModel.ShowTimeBonus(timeBonus);
                    AudioClipPlayer.PlayCheckpoint();
                    break;
            }
        }

        private IEnumerator DelayedSpeedReset()
        {
            var startPosition = playerTransform.position;
            var currentPosition = startPosition;

            while ((currentPosition - startPosition).magnitude < 3f)
            {
                currentPosition = playerTransform.position;
                yield return null;
            }

            inCheckpoint = false;
            StartCoroutine(WorldScroller.ResetDefaultScrollSpeed());
        }
    }
}
