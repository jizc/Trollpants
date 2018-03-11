// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HazardScript.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class HazardScript : MovingObject
    {
        public int damageDealt;
        public int damageDealtBack;
        public bool isDodgeable;
        protected bool isDealingDamage;
        public string frontCollisionName = "FrontHitbox";
        public string backCollisionName = "BackHitbox";
        public string colliderHitBox = "ColliderBox";

        public Enums.Enemy enemyType;

        protected virtual void OnEnable()
        {
            isDealingDamage = true;
        }

        public override void Reset()
        {
            isDealingDamage = true;
            base.Reset();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (isDealingDamage && (col.name == frontCollisionName || col.name == colliderHitBox))
            {
                GetComponent<CollisionSound>().MakeSoundWithPitch();
                Events.instance.Raise(new PlayerHit(damageDealt, isDodgeable, enemyType));
                isDealingDamage = false;
            }
            else if (damageDealtBack > 0 && isDealingDamage && col.name == backCollisionName)
            {
                Events.instance.Raise(new PlayerHit(damageDealtBack, false, enemyType));
                GetComponent<CollisionSound>().MakeSoundWithPitch();
                isDealingDamage = false;
            }
        }
    }
}
