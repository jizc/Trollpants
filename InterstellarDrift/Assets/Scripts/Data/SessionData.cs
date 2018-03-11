// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionData.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class SessionData
    {
        public SessionData()
        {
            Init();
        }

        public int DistanceTravelled { get; set; }
        public int Score { get; set; }

        public int MillisecondsSurvived { get; set; }

        public int SecondsSurvived
        {
            get { return MillisecondsSurvived / 1000; }
        }

        public Texture2D FrontStarsTexture2D { get; set; }
        public Texture2D BackStarsTexture2D { get; set; }

        public void Init()
        {
            DistanceTravelled = 0;
            Score = 0;
            MillisecondsSurvived = 0;
        }
    }
}
