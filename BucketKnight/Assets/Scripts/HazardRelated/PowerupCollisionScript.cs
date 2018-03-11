// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerupCollisionScript.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class PowerupCollisionScript : MovingObject
    {
        public string colliderName = "Player";
        public int value;

        private void OnTriggerEnter(Collider col)
        {
            if (col.name == colliderName)
            {
                Events.instance.Raise(new CoinsPickedUp(value));
                Despawn();
            }
            else if (col.name.ToLower() == "hazardstonewrapper(clone)")
            {

                var newpos = transform.position;

                if (newpos.x < 0)
                {
                    newpos.x += 1f;
                }
                else
                {
                    newpos.x -= 1f;
                }

                transform.position = newpos;
            }
        }
    }
}
