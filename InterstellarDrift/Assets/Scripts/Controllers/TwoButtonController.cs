// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoButtonController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Provides an implementation of the BaseController that splits the screen into two buttons by width.
    /// </summary>
    public class TwoButtonController : BaseController
    {
        private static readonly int s_halfScreenWidth = Screen.width / 2;

        private static bool BoostInputDetected()
        {
            if (Input.touchCount != 2)
            {
                return false;
            }

            if (Input.touches[0].position.x < s_halfScreenWidth)
            {
                return Input.touches[1].position.x > s_halfScreenWidth;
            }

            return Input.touches[1].position.x < s_halfScreenWidth;
        }

        private void Update()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                IsInputActive = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                IsInputActive = false;
            }

            if (IsInputActive)
            {
                if (Input.mousePosition.x <= s_halfScreenWidth)
                {
                    // Left side touched
                    IsMovingRight = false;
                }
                else
                {
                    // Right side touched
                    IsMovingRight = true;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (BoostInputDetected())
            {
                Boost();
            }
            else
            {
                MoveForward();

                if (IsInputActive)
                {
                    if (IsMovingRight)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                }
            }

            BaseFixedUpdate();
        }
    }
}
