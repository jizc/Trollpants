// <copyright file="ChangeGameStateOnClick.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class ChangeGameStateOnClick : MonoBehaviour
    {
        [SerializeField] private GameStateChanger gameStateChanger;
        [SerializeField] private GameState gameState;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            gameStateChanger.ChangeGameState(gameState);
        }
    }
}
