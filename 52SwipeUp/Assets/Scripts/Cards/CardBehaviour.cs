// <copyright file="CardBehaviour.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Cards
{
    using System;
    using UnityEngine;

    public class CardBehaviour : MonoBehaviour
    {
        private SpriteBucket spriteBucket;
        private ObjectPooler objectPooler;
        private bool isQuitting;
        private Card card;

        public Card Card
        {
            get { return card; }
            set
            {
                card = value;
                SetSprite();
            }
        }

        public void Initialize(SpriteBucket sprites, ObjectPooler pooler)
        {
            spriteBucket = sprites;
            objectPooler = pooler;
        }

        public void StartSelfRecycleTimer(float seconds)
        {
            Invoke("RecycleSelf", seconds);
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

        private void RecycleSelf()
        {
            if (!isQuitting)
            {
                objectPooler.Recycle(gameObject);
            }
        }

        private void SetSprite()
        {
            Sprite sprite;
            switch (Card.Suit)
            {
                case Suit.Spades:
                    sprite = spriteBucket.Spades[Card.Number - 1];
                    break;
                case Suit.Hearts:
                    sprite = spriteBucket.Hearts[Card.Number - 1];
                    break;
                case Suit.Diamonds:
                    sprite = spriteBucket.Diamonds[Card.Number - 1];
                    break;
                case Suit.Clubs:
                    sprite = spriteBucket.Clubs[Card.Number - 1];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
