// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FishAnimController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class FishAnimController : MonoBehaviour
    {
        public FishHazard fishBehaviour;

        public void Jump()
        {
            GetComponent<Animation>().Play("Jump");
        }

        public void OnJumped()
        {
            fishBehaviour.Jumping();
        }

        public void OnLanded()
        {
            fishBehaviour.NotJumping();
        }
    }
}
