// <copyright file="RoundCoordinator.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp
{
    using System.Collections.Generic;
    using Cards;
    using GUI;
    using UnityEngine;
    using UnityEngine.Events;

    [RequireComponent(typeof(CardShepherd))]
    [RequireComponent(typeof(GameRules))]
    [RequireComponent(typeof(GameStateChanger))]
    [RequireComponent(typeof(PanelBucket))]
    [RequireComponent(typeof(SessionData))]
    [RequireComponent(typeof(RoundTimer))]
    public class RoundCoordinator : MonoBehaviour
    {
        private const int cardsToClearLow = 15;
        private const int cardsToClearHigh = 25;

        [SerializeField] private InstructionsGuiHandler instructionsGuiHandler;
        [SerializeField] private AndroidUtils androidUtils;

        private int currentRound;
        private CardShepherd cardShepherd;
        private GameRules gameRules;
        private GameStateChanger gameStateChanger;
        private PanelBucket panelBucket;
        private SessionData sessionData;
        private RoundTimer roundTimer;

        private Direction pictureDir;

        public delegate bool AssertMethod(Direction dir, Card card);

        public event UnityAction<int> CurrentRoundChanged;

        public int CurrentRound
        {
            get { return currentRound; }
            private set
            {
                if (currentRound != value)
                {
                    currentRound = value;
                    CurrentRoundChanged?.Invoke(value);
                }
            }
        }

        public int CardsToClearLow
        {
            get { return cardsToClearLow; }
        }

        public int CardsToClearHigh
        {
            get { return cardsToClearHigh; }
        }

        public void StartFirstRound()
        {
            sessionData.ResetProperties();
            CurrentRound = 1;
            PrepareNextRound();
        }

        public void StartNextRound()
        {
            CurrentRound++;
            PrepareNextRound();
        }

        public void RestartGame()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            androidUtils.ShowSelectDialog(
                "Restart game?",
                "Are you sure you want to restart? All the current progress will be lost!",
                restart =>
                {
                    if (restart)
                    {
                        RestartGameImpl();
                    }

                    AudioClipPlayer.Instance.PlayClick();
                });
#else
            RestartGameImpl();
#endif
        }

        private void Awake()
        {
            cardShepherd = GetComponent<CardShepherd>();
            gameRules = GetComponent<GameRules>();
            panelBucket = GetComponent<PanelBucket>();
            sessionData = GetComponent<SessionData>();
            gameStateChanger = GetComponent<GameStateChanger>();
            roundTimer = GetComponent<RoundTimer>();
        }

        private void PrepareNextRound()
        {
            var dirlist = new List<Direction>();
            cardShepherd.ResetCardsForNewRound((CurrentRound % 2 == 0) ? cardsToClearHigh : cardsToClearLow);
            roundTimer.ResetTimer();

            // Display correct instructions
            if (CurrentRound <= 2)
            {
                var rules = gameRules.AddRandomColorRules();
                instructionsGuiHandler.DisplayInstructions(rules);
                return;
            }

            if (CurrentRound >= 3)
            {
                dirlist = gameRules.AddRandomSuitRules();
            }

            if (CurrentRound >= 5)
            {
                pictureDir = gameRules.GetRandomDirection();
                dirlist.Add(pictureDir);
                gameRules.AddRule(pictureDir, 11, 13);
            }

            if (CurrentRound >= 7)
            {
                Direction lowCardsDir;
                do
                {
                    lowCardsDir = gameRules.GetRandomDirection();
                }
                while (lowCardsDir == pictureDir);

                dirlist.Add(lowCardsDir);
                gameRules.AddRule(lowCardsDir, 2, 5);
            }

            instructionsGuiHandler.DisplayInstructions(dirlist);
        }

        private void RestartGameImpl()
        {
            roundTimer.StopTimer();
            PlayerSettings.MovementEnabled = false;
            panelBucket.Hud.GetComponent<CanvasGroup>().interactable = false;
            panelBucket.Hud.GetComponent<PanelAnimatorHelper>().HidePanel();
            gameRules.ClearRules();
            gameStateChanger.ChangeGameState(GameState.Instructions, 0.4f);
        }
    }
}
