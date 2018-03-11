// <copyright file="CardShepherd.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.Cards
{
    using System.Collections.Generic;
    using Audio;
    using GUI;
    using UnityEngine;

    [RequireComponent(typeof(GameRules))]
    [RequireComponent(typeof(GameStateChanger))]
    [RequireComponent(typeof(ObjectPooler))]
    [RequireComponent(typeof(SpriteBucket))]
    [RequireComponent(typeof(PanelBucket))]
    [RequireComponent(typeof(SessionData))]
    [RequireComponent(typeof(RoundCoordinator))]
    [RequireComponent(typeof(RoundTimer))]
    public class CardShepherd : MonoBehaviour
    {
        private const float forceMod = 60;
        private readonly Deck deck = new Deck();

        [SerializeField] private CameraShake cameraShake;

        private GameRules gameRules;
        private GameStateChanger gameStateChanger;
        private ObjectPooler objectPooler;
        private SpriteBucket spriteBucket;
        private PanelBucket panelBucket;
        private SessionData sessionData;
        private RoundCoordinator roundCoordinator;
        private RoundTimer roundTimer;

        private GameObject currentCard;
        private int cardsCleared;
        private int cardsToClear;
        private Stack<Card> activeCards = new Stack<Card>();

        public void ResetCardsForNewRound(int cardCount)
        {
            if (currentCard != null && currentCard.activeSelf)
            {
                objectPooler.Recycle(currentCard);
            }

            deck.Reset();
            deck.Shuffle();
            cardsCleared = 0;
            cardsToClear = cardCount;
            activeCards = deck.GetTopCards(cardsToClear);
        }

        public void SpawnFirstCard()
        {
            currentCard = SetNewCard();
        }

        public void MoveCard(Vector2 direction)
        {
            if (currentCard == null)
            {
                Debug.LogWarning("s_currentCard is null");
                return;
            }

            cardsCleared++;
            var tempcard = SetNewCard();

            // Send the current card of the screen in the direction the player swiped
#if DEBUG
            Debug.Log("Sending card in direction: " + direction);
#endif
            currentCard.GetComponent<Renderer>().sortingOrder = 2;
            currentCard.GetComponent<Rigidbody2D>().AddForce(direction * forceMod, ForceMode2D.Impulse);
            var cb = currentCard.GetComponent<CardBehaviour>();
            cb.StartSelfRecycleTimer(0.5f);

            if (gameRules.CheckRules(DirectionUtils.Vector2ToDirection(direction), cb.Card))
            {
                // Correct swipe
                AudioClipPlayer.PlaySwipe();
                sessionData.Score++;

                // Continue streak or start a new one
                if (sessionData.IsOnStreak)
                {
                    sessionData.CurrentStreak++;
                }
                else
                {
                    sessionData.IsOnStreak = true;
                    sessionData.CurrentStreak = 1;
                }
            }
            else
            {
                // Wrong swipe
                AudioClipPlayer.PlaySwipe();
                AudioClipPlayer.PlaySwipeError();
                cameraShake.Shake();
                sessionData.Lives--;

                // Streak broken
                sessionData.IsOnStreak = false;
                if (sessionData.CurrentStreak > sessionData.HighestStreakInSession)
                {
                    sessionData.HighestStreakInSession = sessionData.CurrentStreak;
                }
            }

            // Is player is dead?
            if (sessionData.Lives <= 0)
            {
                if (tempcard != null)
                {
                    tempcard.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * forceMod, ForceMode2D.Impulse);
                    tempcard.GetComponent<CardBehaviour>().StartSelfRecycleTimer(0.4f);
                }

                EndRound();
                gameStateChanger.ChangeGameState(GameState.GameEnd, 0.4f);
                return;
            }

            currentCard = tempcard;

            // Has the player cleared enough cards to end the round?
            if (cardsCleared >= cardsToClear)
            {
                EndRound();
                gameStateChanger.ChangeGameState(GameState.RoundEnd, 0.5f);
            }
        }

        public void RecycleCurrentCard()
        {
            if (currentCard != null)
            {
                objectPooler.Recycle(currentCard);
            }
        }

        private void EndRound()
        {
            roundTimer.StopTimer();
            PlayerSettings.MovementEnabled = false;
            panelBucket.Hud.GetComponent<CanvasGroup>().interactable = false;
            panelBucket.Hud.GetComponent<PanelAnimatorHelper>().HidePanel();
        }

        private void Awake()
        {
            gameRules = GetComponent<GameRules>();
            gameStateChanger = GetComponent<GameStateChanger>();
            objectPooler = GetComponent<ObjectPooler>();
            spriteBucket = GetComponent<SpriteBucket>();
            panelBucket = GetComponent<PanelBucket>();
            sessionData = GetComponent<SessionData>();
            roundCoordinator = GetComponent<RoundCoordinator>();
            roundTimer = GetComponent<RoundTimer>();
        }

        private GameObject SetNewCard()
        {
#if DEBUG
            Debug.Log("Active cards: " + activeCards.Count);
#endif
            if (activeCards.Count > 0)
            {
                var tempCard = activeCards.Pop();
                var temp = objectPooler.Spawn("Prefabs/", "Card", roundCoordinator.transform, Vector3.zero);
                var cb = temp.GetComponent<CardBehaviour>();
                cb.Initialize(spriteBucket, objectPooler);
                cb.Card = tempCard;
                temp.GetComponent<Renderer>().sortingOrder = 1;
                return temp;
            }

            return null;
        }
    }
}
