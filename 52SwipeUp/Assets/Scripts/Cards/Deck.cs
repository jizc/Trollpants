// <copyright file="Deck.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Deck
    {
        private Stack<Card> cards;

        public Deck()
        {
            Reset();
        }

        public Card GetTopCard()
        {
            return cards.Count > 0 ? cards.Pop() : null;
        }

        /// <summary>
        /// Gets a number of cards from the deck, up to the number of remaining cards
        /// </summary>
        /// <param name="numberOfCards">Number of cards to return, must be greater than zero</param>
        /// <returns>A stack with the desired number of cards, up to the remaining number of cards in the reverse order they were drawn.</returns>
        public Stack<Card> GetTopCards(int numberOfCards)
        {
            UnityEngine.Debug.Log("Getting number of cards: " + numberOfCards);
            if (numberOfCards <= 0)
            {
                throw new ArgumentException("Number of cards must be greater than zero", "numberOfCards");
            }

            var topCards = new Stack<Card>();

            var i = 0;
            while (i < numberOfCards && cards.Count > 0)
            {
                topCards.Push(cards.Pop());
                i++;
            }

            return topCards;
        }

        public void Reset()
        {
            cards = new Stack<Card>();

            for (var i = 4; i > 0; i--)
            {
                for (var j = 13; j > 0; j--)
                {
                    cards.Push(new Card(j, (Suit)i));
                }
            }
        }

        // Shuffles deck using the Knuth-Fisher-Yates shuffle algorithm.
        public void Shuffle()
        {
            var r = new Random();
            var tempCards = cards.ToArray();
            for (var i = tempCards.Length - 1; i > 0; i--)
            {
                var n = r.Next(i + 1);
                var temp = tempCards[i];
                tempCards[i] = tempCards[n];
                tempCards[n] = temp;
            }

            cards.Clear();
            foreach (var c in tempCards)
            {
                cards.Push(c);
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            foreach (var card in cards)
            {
                str.Append(card + "\n");
            }

            return str.ToString();
        }
    }
}
