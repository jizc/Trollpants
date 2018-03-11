// <copyright file="SwipeDetector.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Input
{
    using System;
    using Cards;
    using UnityEngine;

    public class SwipeDetector : MonoBehaviour
    {
        private const float minSwipeDist = 50.0f;
        private const float maxSwipeTime = 0.5f;

        private float fingerStartTime;
        private Vector2 fingerStartPos = Vector2.zero;
        private bool isSwipe;

        public delegate void SwipeHandler(Direction dir);

        public event SwipeHandler Swiped;

        private void Update()
        {
            if (UnityEngine.Input.touchCount < 1)
            {
                return;
            }

            foreach (var touch in UnityEngine.Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        /* this is a new touch */
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
                        /* The touch is being canceled */
                        isSwipe = false;
                        break;

                    case TouchPhase.Ended:

                        var gestureTime = Time.time - fingerStartTime;
                        var gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            var direction = touch.position - fingerStartPos;
                            Vector2 swipeType;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (Math.Abs(swipeType.x) > 0.001f)
                            {
                                Swiped?.Invoke(swipeType.x > 0.0f ? Direction.Right : Direction.Left);
                            }

                            if (Math.Abs(swipeType.y) > 0.001f)
                            {
                                Swiped?.Invoke(swipeType.y > 0.0f ? Direction.Up : Direction.Down);
                            }
                        }

                        break;
                }
            }
        }
    }
}
