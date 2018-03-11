// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class KeyboardController : BaseController
    {
        private void Update()
        {
            if (!IsInitialized)
            {
                return;
            }

            var horizontal = Input.GetAxis("Horizontal");

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                IsInputActive = true;

                if (horizontal > 0f)
                {
                    IsMovingRight = true;
                }
                else if (horizontal < 0f)
                {
                    IsMovingRight = false;
                }

                return;
            }

            IsInputActive = false;
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (Input.GetKey(KeyCode.Space))
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
