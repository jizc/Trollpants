// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Powerup.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Xml.Serialization;

    public class Powerup
    {
        [XmlAttribute("name")] public string Name;

        public Enums.PowerupType powerupType;
        public int Duration;
        public string PowerupText;
        public string imageName;
        public string countDownImageName;
        public int unlockPrice;
        public int equipPrice;
        public bool spawnAsPowerup;
        public int level;
    }
}
