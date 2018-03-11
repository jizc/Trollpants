// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HazardCollisionScript.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    namespace Assets.Scripts.HazardRelated
    {
        using UnityEngine;

        public class HazardCollisionScript : MovingObject
        {
            public int damageDealt;
            public bool isDodgeable;
            protected bool isDealingDamage;
            public string frontCollisionName = "FrontHitbox";
            public string backCollisionName = "BackHitbox";
            public string colliderHitBox = "ColliderBox";

            public Enums.Enemy EnemyType;

            protected override void Start()
            {
                isDealingDamage = true;
                base.Start();
            }

            private void OnTriggerEnter(Collider col)
            {

                if (isDealingDamage && (col.name == frontCollisionName || col.name == colliderHitBox))
                {
                    GetComponent<CollisionSound>().MakeSoundWithPitch();
                    Events.instance.Raise(new PlayerHit(damageDealt, isDodgeable, EnemyType));
                    isDealingDamage = false;
                }
                else if (isDealingDamage && col.name == backCollisionName && damageDealt > 1 && !isDodgeable)
                {
                    Events.instance.Raise(new PlayerHit(1, false, EnemyType));
                    GetComponent<CollisionSound>().MakeSoundWithPitch();
                    isDealingDamage = false;
                }
            }

            private void OnCollisionEnter(Collision col)
            {

                if (isDealingDamage && col.gameObject.name == frontCollisionName)
                {
                    Events.instance.Raise(new PlayerHit(damageDealt, isDodgeable, EnemyType));
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
}
