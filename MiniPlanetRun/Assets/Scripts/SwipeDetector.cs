// <copyright file="SwipeDetector.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun
{
    using UnityEngine;

    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    public class SwipeDetector : MonoBehaviour
    {
        private const float maxSwipeTime = 0.5f;
        private const float minSwipeDist = 50.0f;

        private float fingerStartTime;
        private Vector2 fingerStartPos = Vector2.zero;
        private bool isSwipe;

        public delegate void SwipeHandler(Direction dir);

        public event SwipeHandler Swiped;

        private void Update()
        {
            if (Input.touchCount <= 0)
            {
                return;
            }

            foreach (var touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
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

                            if (!Equals(swipeType.x, 0.0f))
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    if (Swiped != null)
                                    {
                                        Swiped(Direction.Right);
                                    }
                                }
                                else
                                {
                                    if (Swiped != null)
                                    {
                                        Swiped(Direction.Left);
                                    }
                                }
                            }

                            if (!Equals(swipeType.y, 0.0f))
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    if (Swiped != null)
                                    {
                                        Swiped(Direction.Up);
                                    }
                                }
                                else
                                {
                                    if (Swiped != null)
                                    {
                                        Swiped(Direction.Down);
                                    }
                                }
                            }
                        }

                        break;
                }
            }
        }
    }
}
