// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HazardColliderScript.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class HazardColliderScript : HazardScript
    {
        protected override void Start()
        {
            base.Start();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (isDealingDamage && col.gameObject.name == frontCollisionName)
            {
                Events.instance.Raise(new PlayerHit(damageDealt, isDodgeable, enemyType));
                GetComponent<CollisionSound>().MakeSoundWithPitch();
                isDealingDamage = false;
            }
            else if (isDealingDamage && col.gameObject.name == backCollisionName)
            {
                GetComponent<CollisionSound>().MakeSoundWithPitch();
                isDealingDamage = false;
            }
        }
    }
}
