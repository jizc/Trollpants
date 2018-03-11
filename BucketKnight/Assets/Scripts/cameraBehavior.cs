// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cameraBehavior.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class cameraBehavior : MonoBehaviour
    {
        private Transform _playerTransform;
        private Vector3 _cameraOrientationVector = new Vector3(0, 4, -3);
        private bool gameOver;

        private void Awake()
        {
            Events.instance.AddListener<PlayerLost>(OnPlayerLost);
            Events.instance.AddListener<GameResumed>(OnGameResumed);
        }

        private void Start()
        {
            _playerTransform = GameObject.Find("Player").transform;
        }

        private void LateUpdate()
        {
            if (gameOver)
            {
                return;
            }

            transform.position = _playerTransform.position + _cameraOrientationVector;
        }

        private void OnDestroy()
        {
            Events.instance.RemoveListener<PlayerLost>(OnPlayerLost);
            Events.instance.RemoveListener<GameResumed>(OnGameResumed);
        }

        private void OnPlayerLost(PlayerLost playerLostEvent)
        {
            gameOver = true;
        }

        private void OnGameResumed(GameResumed gameResumedEvent)
        {
            gameOver = false;
        }
    }
}
