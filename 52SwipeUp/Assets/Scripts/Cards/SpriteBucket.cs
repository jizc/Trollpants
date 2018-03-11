// <copyright file="SpriteBucket.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Cards
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SpriteBucket : MonoBehaviour
    {
        [SerializeField] private List<Sprite> spades;
        [SerializeField] private List<Sprite> hearts;
        [SerializeField] private List<Sprite> diamonds;
        [SerializeField] private List<Sprite> clubs;
        [SerializeField] private List<Sprite> ruleSprites;

        public List<Sprite> Spades
        {
            get { return spades; }
        }

        public List<Sprite> Hearts
        {
            get { return hearts; }
        }

        public List<Sprite> Diamonds
        {
            get { return diamonds; }
        }

        public List<Sprite> Clubs
        {
            get { return clubs; }
        }

        // The order of sprites MUST be: Spade, Heart, Diamond, Clover, Picture Cards, 2-5
        public List<Sprite> RuleSprites
        {
            get { return ruleSprites; }
            set { ruleSprites = value; }
        }
    }
}
