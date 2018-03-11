// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerupContainer.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("PowerupCollection")]
    public class PowerupContainer
    {
        [XmlArray("Powerups")] [XmlArrayItem("Powerup")] public List<Powerup> Powerups = new List<Powerup>();
    }
}
