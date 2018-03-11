// <copyright file="InputSupervisor.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Input
{
    using Cards;
    using UnityEngine;

    [RequireComponent(typeof(CardShepherd))]
    [RequireComponent(typeof(SwipeDetector))]
    public class InputSupervisor : MonoBehaviour
    {
        private CardShepherd cardShepherd;
        private SwipeDetector swipeDetector;

        private void OnSwiped(Direction direction)
        {
            if (PlayerSettings.MovementEnabled)
            {
                cardShepherd.MoveCard(DirectionUtils.DirectionToVector2(direction));
            }
        }

        private void Awake()
        {
            cardShepherd = GetComponent<CardShepherd>();
            swipeDetector = GetComponent<SwipeDetector>();

            swipeDetector.Swiped += OnSwiped;
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        private void Update()
        {
            if (PlayerSettings.MovementEnabled)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cardShepherd.MoveCard(Vector2.right);
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    cardShepherd.MoveCard(-Vector2.right);
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    cardShepherd.MoveCard(Vector2.up);
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    cardShepherd.MoveCard(-Vector2.up);
                }
            }
        }
#endif
    }
}
