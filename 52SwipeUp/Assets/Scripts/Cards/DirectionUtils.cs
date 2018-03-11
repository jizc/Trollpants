// <copyright file="DirectionUtils.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Cards
{
    using System;
    using UnityEngine;

    public enum Direction
    {
        None = 0,
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionUtils
    {
        public static Direction Vector2ToDirection(Vector2 vector)
        {
            var dir = Direction.None;

            if (vector.Equals(Vector2.right))
            {
                dir = Direction.Right;
            }
            else if (vector.Equals(-Vector2.right))
            {
                dir = Direction.Left;
            }
            else if (vector.Equals(Vector2.up))
            {
                dir = Direction.Up;
            }
            else if (vector.Equals(-Vector2.up))
            {
                dir = Direction.Down;
            }

            return dir;
        }

        public static Vector2 DirectionToVector2(Direction dir)
        {
            Vector2 vector;
            switch (dir)
            {
                case Direction.Right:
                    vector = Vector2.right;
                    break;
                case Direction.Left:
                    vector = -Vector2.right;
                    break;
                case Direction.Up:
                    vector = Vector2.up;
                    break;
                case Direction.Down:
                    vector = -Vector2.up;
                    break;
                case Direction.None:
                    vector = Vector2.zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }

            return vector;
        }
    }
}
