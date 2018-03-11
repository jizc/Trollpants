// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneClickController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Provides an implementation of the BaseController that uses a single mouse-click.
    /// </summary>
    public class OneClickController : BaseController
    {
        private void Update()
        {
            if (!IsInitialized)
            {
                return;
            }

#if DEBUG
            Debug.Log("Mousebutton(0) is " + (Input.GetMouseButton(0) ? "pressed" : "not pressed"));
#endif

            if (Input.GetMouseButton(0))
            {
                IsInputActive = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // Set input to false and flip IsMovingRight so the next movement is opposite the previous.
                IsInputActive = false;
                IsMovingRight = !IsMovingRight;
            }
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
            {
                return;
            }

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

            BaseFixedUpdate();
        }
    }
}
