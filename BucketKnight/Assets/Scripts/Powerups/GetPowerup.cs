// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPowerup.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;

    public class GetPowerup
    {
        private List<Powerup> _powerups = new List<Powerup>();

        public GetPowerup()
        {
            _powerups.Add(new Powerup {Duration = 0, powerupType = Enums.PowerupType.None});
            _powerups.Add(new Powerup {Duration = 10, powerupType = Enums.PowerupType.Bubble}); // Works
            _powerups.Add(new Powerup {Duration = 0, powerupType = Enums.PowerupType.FillHp}); // Works
            _powerups.Add(new Powerup {Duration = 10, powerupType = Enums.PowerupType.Windmill}); // Works
            _powerups.Add(new Powerup {Duration = 5, powerupType = Enums.PowerupType.MoneyBag}); // Works
            _powerups.Add(new Powerup {Duration = 10, powerupType = Enums.PowerupType.Sword}); // Works (burde fjerne det den kræsjer med)
            _powerups.Add(new Powerup {Duration = 0, powerupType = Enums.PowerupType.TemporaryHp}); // Works
            _powerups.Add(new Powerup {Duration = 5, powerupType = Enums.PowerupType.Grail}); // Works
            //powerups.Add(new Powerup { Duration = 0, powerupType = Enums.PowerupType.Diamond });
        }
    }
}
