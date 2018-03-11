// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enums.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    public class Enums
    {
        public enum PowerupType
        {
            None,
            Bubble,
            FillHp,
            Windmill,
            MoneyBag,
            Sword,
            TemporaryHp,
            Diamond,
            Grail
        }

        public enum PowerupActivationType
        {
            Pickup,
            Equipment
        }

        public enum Enemy
        {
            Fish,
            Log,
            Stone,
            Beaver,
            Bridge
        }
    }
}
