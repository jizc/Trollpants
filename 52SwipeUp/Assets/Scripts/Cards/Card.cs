// <copyright file="Card.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Cards
{
    using System;
    using System.Globalization;
    using System.Text;

    public enum Suit
    {
        Spades = 1,
        Hearts,
        Diamonds,
        Clubs
    }

    public enum CardColor
    {
        Red = 1,
        Black
    }

    public class Card
    {
        public Card(int number, Suit suit)
        {
            Suit = suit;
            if (number < 1 || number > 13)
            {
                throw new ArgumentException("Number must be between 1 and 13");
            }

            Number = number;

            if (suit == Suit.Clubs || suit == Suit.Spades)
            {
                Color = CardColor.Black;
            }
            else
            {
                Color = CardColor.Red;
            }
        }

        public Suit Suit { get; private set; }
        public CardColor Color { get; private set; }
        public int Number { get; private set; }

        public bool IsPictureCard()
        {
            return Number > 10;
        }

        public bool IsLowCard()
        {
            return Number < 6 && Number > 1;
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            switch (Number)
            {
                case 1:
                    str.Append("Ace");
                    break;
                case 11:
                    str.Append("Jack");
                    break;
                case 12:
                    str.Append("Queen");
                    break;
                case 13:
                    str.Append("King");
                    break;
                default:
                    str.Append(Number.ToString(CultureInfo.InvariantCulture));
                    break;
            }

            str.Append(" of " + Suit);

            return str.ToString();
        }
    }
}
