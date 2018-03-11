// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomPowerup.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using CloudOnce;
    using UnityEngine;

    public class RandomPowerup : MovingObject
    {
        public Enums.PowerupType powerupType;

        public string colliderName = "Player";

        protected void OnEnable()
        {
            powerupType = PowerupManager.GetRandomPowerupTypeAtLevel(CloudVariables.NumberOfPowerupsUnlocked);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.name == colliderName)
            {
                Events.instance.Raise(new PowerupPickup(powerupType, Enums.PowerupActivationType.Pickup));
                Despawn();
            }
        }
    }
}
