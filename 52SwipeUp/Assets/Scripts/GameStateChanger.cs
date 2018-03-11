// <copyright file="GameStateChanger.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp
{
    using System;
    using System.Collections;
    using Audio;
    using Cards;
    using CloudOnce;
    using GUI;
    using UnityEngine;

    public enum GameState
    {
        SplashScreen,
        StartMenu,
        HowToPlay,
        Instructions,
        Playing,
        RoundEnd,
        GameEnd
    }

    [RequireComponent(typeof(CardShepherd))]
    [RequireComponent(typeof(GameRules))]
    [RequireComponent(typeof(PanelBucket))]
    [RequireComponent(typeof(SessionData))]
    [RequireComponent(typeof(RoundCoordinator))]
    [RequireComponent(typeof(RoundEndController))]
    [RequireComponent(typeof(RoundTimer))]
    public class GameStateChanger : MonoBehaviour
    {
        [SerializeField] private InstructionsGuiHandler instructionsGuiHandler;
        [SerializeField] private AndroidUtils androidUtils;

        private CardShepherd cardShepherd;
        private PanelBucket panelBucket;
        private SessionData sessionData;
        private RoundCoordinator roundCoordinator;
        private RoundTimer roundTimer;
        private GameRules gameRules;
        private RoundEndController roundEndController;

        public GameState CurrentGameState { get; private set; }

        public void ChangeGameState(GameState gameState)
        {
#if DEBUG
            Debug.Log("Changing current game state to " + gameState);
#endif
            switch (gameState)
            {
                case GameState.SplashScreen:
                    // Activate correct panel
                    panelBucket.DeactivateAllPanels(panelBucket.SplashScreen);
                    break;
                case GameState.StartMenu:
                    // Activate correct panels
                    panelBucket.DeactivateAllPanels(panelBucket.StartMenu);
                    panelBucket.Options.GetComponent<PanelAnimatorHelper>().ShowPanelInstantly();
                    break;
                case GameState.HowToPlay:
                    // Activate correct panel
                    CurrentGameState = gameState;

                    panelBucket.DeactivateAllPanels();
                    panelBucket.HowToPlay.GetComponent<HowToPlayController>().ActivateHowTo();

                    // Set that HowToPlay has been seen
                    PlayerSettings.HowToPlayShown = true;
                    PlayerSettings.Save();
                    break;
                case GameState.Instructions:
                    // Activate correct panel
                    panelBucket.DeactivateAllPanels(panelBucket.Instructions);

                    // Reset camera
                    Camera.main.transform.position = new Vector3(0f, 1f, -11f);

                    // Set up round
                    if (CurrentGameState == GameState.RoundEnd)
                    {
#if DEBUG
                        Debug.Log("StartNextRound() called.");
#endif
                        roundCoordinator.StartNextRound();
                    }
                    else
                    {
#if DEBUG
                        Debug.Log("StartFirstRound() called.");
#endif

                        roundCoordinator.StartFirstRound();
                        AudioClipPlayer.FadeFromMenuToInGameMusic();
                    }

                    break;
                case GameState.Playing:
                    // Activate correct panel
                    panelBucket.DeactivateAllPanels(panelBucket.Hud);
                    panelBucket.Hud.GetComponent<CanvasGroup>().interactable = true;

                    // Start the timer
                    roundTimer.StartTimer();

                    // Activate swiping
                    PlayerSettings.MovementEnabled = true;
                    break;
                case GameState.RoundEnd:
#if DEBUG
                    Debug.Log("ROUND END");
#endif

                    // Report on Lucky achievement
                    if (sessionData.TimeLeft >= 23f)
                    {
                        Achievements.Lucky.Unlock();
                    }

                    // Report progress on the Steady Hand achievement
                    if (roundCoordinator.CurrentRound == 1)
                    {
                        Achievements.SteadyHand.Increment(sessionData.Score, roundCoordinator.CardsToClearLow);
                    }

                    // Report progress on the Full Deck achievement
                    if (sessionData.CurrentStreak > sessionData.HighestStreakInSession)
                    {
                        sessionData.HighestStreakInSession = sessionData.CurrentStreak;
                    }

                    Achievements.FullDeck.Increment(sessionData.HighestStreakInSession, 52.0);

                    // Clear current rules
                    gameRules.ClearRules();

                    // Prepare the round end card
                    var timeLeft = (int)sessionData.TimeLeft;
                    roundEndController.PrepareRoundEndCard(sessionData.Score, timeLeft);

                    // Increase score
                    sessionData.Score += timeLeft;

                    // Activate correct panel
                    panelBucket.DeactivateAllPanels(panelBucket.RoundResults);
#if DEBUG
                    Debug.Log("ROUND END FINISH");
#endif
                    break;

                case GameState.GameEnd:
#if DEBUG
                    Debug.Log("GAME END");
#endif

                    // Report progress on the Steady Hand achievement
                    if (roundCoordinator.CurrentRound == 1)
                    {
                        Achievements.SteadyHand.Increment(sessionData.Score, roundCoordinator.CardsToClearLow);
                    }

                    // Report progress on the Full Deck achievement
                    if (sessionData.CurrentStreak > sessionData.HighestStreakInSession)
                    {
                        sessionData.HighestStreakInSession = sessionData.CurrentStreak;
                    }

                    Achievements.FullDeck.Increment(sessionData.HighestStreakInSession, 52.0);

                    // Clear current rules
                    gameRules.ClearRules();

                    // Check if High Score
                    if (sessionData.Score > CloudVariables.HighScore)
                    {
                        sessionData.NewHighScoreSet = true;
                        CloudVariables.HighScore = sessionData.Score;
                    }

                    // Report on achievements
                    ReportOnScoreAchievements();

                    // Submit score to leaderboard
                    Leaderboards.Highscore.SubmitScore(sessionData.Score);

                    // Activate correct panel
                    panelBucket.DeactivateAllPanels();
                    panelBucket.GameResults.GetComponent<PanelAnimatorHelper>().ShowPanel();

                    // Save data
                    Cloud.Storage.Save();
#if DEBUG
                    Debug.Log("GAME END FINISH");
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException("gameState");
            }

            CurrentGameState = gameState;
        }

        public void ChangeGameState(GameState gameState, float delay)
        {
            StartCoroutine(ChangeGameStateWithDelayImpl(gameState, delay));
        }

        private void ReportOnScoreAchievements()
        {
            Achievements.Gambler.Increment(sessionData.Score, 250);
            Achievements.HighRoller.Increment(sessionData.Score, 500);
        }

        private void GameStateChangerOnShown(GameObject sender, string message)
        {
            if (Equals(sender, panelBucket.Instructions))
            {
                panelBucket.Instructions.GetComponent<InstructionsTimer>().StartTimer();
                instructionsGuiHandler.EnableArrows();
                AudioClipPlayer.PlayCountdown();
                cardShepherd.SpawnFirstCard();
            }
            else if (Equals(sender, panelBucket.RoundResults))
            {
                // Run round result animation
                roundEndController.RoundEndAnimation(2f);

                // Sometimes the last card doesn't get sent off screen,
                // so we make sure it's recycled here.
                cardShepherd.RecycleCurrentCard();
            }
            else if (Equals(sender, panelBucket.GameResults))
            {
                // Run game result animation
                panelBucket.GameResults.GetComponent<ResultsController>().BeginResultsAnimation(3f);
            }
        }

        private void Awake()
        {
            panelBucket = GetComponent<PanelBucket>();
            sessionData = GetComponent<SessionData>();
            roundCoordinator = GetComponent<RoundCoordinator>();
            roundTimer = GetComponent<RoundTimer>();
            gameRules = GetComponent<GameRules>();
            roundEndController = GetComponent<RoundEndController>();
            cardShepherd = GetComponent<CardShepherd>();

            ChangeGameState(GameState.SplashScreen);
            Cloud.OnCloudLoadComplete += DeactivateSplashScreen;

            var instructionsAnims = panelBucket.Instructions.GetComponent<PanelAnimatorHelper>();
            instructionsAnims.OnShown += GameStateChangerOnShown;
            instructionsAnims.OnHidden += GameStateChangerOnHidden;
            var resultsAnims = panelBucket.RoundResults.GetComponent<PanelAnimatorHelper>();
            resultsAnims.OnShown += GameStateChangerOnShown;
            resultsAnims.OnHidden += GameStateChangerOnHidden;
            panelBucket.GameResults.GetComponent<PanelAnimatorHelper>().OnShown += GameStateChangerOnShown;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                switch (CurrentGameState)
                {
                    case GameState.SplashScreen:
                        break;
                    case GameState.StartMenu:
#if !UNITY_EDITOR && UNITY_ANDROID
                        androidUtils.ShowSelectDialog(
                            "Exit 52 Swipe Up?",
                            "Are you sure you want to exit?",
                            exit =>
                            {
                                if (exit)
                                {
                                    Application.Quit();
                                }

                                AudioClipPlayer.Instance.PlayClick();
                            });
#elif UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                        break;
                    case GameState.HowToPlay:
                        break;
                    case GameState.Instructions:
                        break;
                    case GameState.Playing:
                        roundCoordinator.RestartGame();
                        break;
                    case GameState.RoundEnd:
                        break;
                    case GameState.GameEnd:
                        ChangeGameState(GameState.StartMenu);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void DeactivateSplashScreen(bool success)
        {
            ChangeGameState(GameState.StartMenu);
            Cloud.OnCloudLoadComplete -= DeactivateSplashScreen;
        }

        private IEnumerator ChangeGameStateWithDelayImpl(GameState gameState, float delay)
        {
            yield return new WaitForSeconds(delay);

            ChangeGameState(gameState);
        }

        private void GameStateChangerOnHidden(GameObject sender, string message)
        {
            if (Equals(sender, panelBucket.Instructions))
            {
                panelBucket.Instructions.GetComponent<InstructionsTimer>().ResetTimer();
                ChangeGameState(GameState.Playing);
            }
            else if (Equals(sender, panelBucket.RoundResults))
            {
                ChangeGameState(GameState.Instructions);
            }
        }
    }
}
