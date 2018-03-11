// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DodgeAnimationFunctions.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class DodgeAnimationFunctions : MonoBehaviour
    {
        public PlayerStats playerStats;
        public PlayerMovement playerMovement;

        private void SetOverWater()
        {
            playerStats.SetOverWater();
        }

        private void SetUnderWater()
        {
            playerStats.SetUnderWater();
        }

        private void changeBarrelRollValue()
        {
            playerMovement.ChangeBarrelRoll();
        }
    }
}
