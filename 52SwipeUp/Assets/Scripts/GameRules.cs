// <copyright file="GameRules.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp
{
    using System;
    using System.Collections.Generic;
    using Cards;
    using UnityEngine;
    using Random = System.Random;

    [RequireComponent(typeof(RoundCoordinator))]
    public class GameRules : MonoBehaviour
    {
        private readonly List<Func<Direction, Card, bool>> rules = new List<Func<Direction, Card, bool>>();
        private readonly List<Func<Direction, Card, bool>> extraRules = new List<Func<Direction, Card, bool>>();
        private readonly Random random = new Random();

        private RoundCoordinator roundCoordinator;

        public List<Direction> AddRandomColorRules()
        {
            var dir1 = (Direction)random.Next(1, 5);
            Direction dir2;
            do
            {
                dir2 = (Direction)random.Next(1, 5);
            }
            while (dir2 == dir1);

            AddRule(dir1, CardColor.Black);
            AddRule(dir2, CardColor.Red);

            var dirlist = new List<Direction> { dir1, dir2, dir2, dir1 };
            return dirlist;
        }

        public List<Direction> AddRandomSuitRules()
        {
            var dirlist = new List<Direction>();
            var dirs = new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
            for (var i = dirs.Length - 1; i > 0; i--)
            {
                var n = random.Next(i + 1);
                var temp = dirs[i];
                dirs[i] = dirs[n];
                dirs[n] = temp;
            }

            for (var i = 1; i < 5; i++)
            {
                AddRule(dirs[i - 1], (Suit)i);
                dirlist.Add(dirs[i - 1]);
            }

            return dirlist;
        }

        public void AddRule(Direction direction, int min, int max)
        {
            Func<Direction, Card, bool> f = (dir, card) => dir == direction && card.Number >= min && card.Number <= max;
            extraRules.Add(f);
        }

        public bool CheckRules(Direction dir, Card card)
        {
            var result = false;

            if ((roundCoordinator.CurrentRound >= 5 && card.IsPictureCard())
                || (roundCoordinator.CurrentRound >= 7 && card.IsLowCard()))
            {
                foreach (var r in extraRules)
                {
                    if (r.Invoke(dir, card))
                    {
                        result = true;
                    }
                }
            }
            else
            {
                foreach (var r in rules)
                {
                    if (r.Invoke(dir, card))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public void ClearRules()
        {
            rules.Clear();
            extraRules.Clear();
        }

        public Direction GetRandomDirection()
        {
            return (Direction)random.Next(1, 5);
        }

        private void AddRule(Direction direction, Suit suit)
        {
            var temp = new Card(1, suit);
            Func<Direction, Card, bool> f = (dir, card) => dir == direction && card.Suit == temp.Suit;
            rules.Add(f);
        }

        private void AddRule(Direction direction, CardColor color)
        {
            var temp = color == CardColor.Black ? new Card(1, Suit.Spades) : new Card(1, Suit.Hearts);

            Func<Direction, Card, bool> f = (dir, card) => dir == direction && card.Color == temp.Color;
            rules.Add(f);
        }

        private void Awake()
        {
            roundCoordinator = GetComponent<RoundCoordinator>();
        }
    }
}
