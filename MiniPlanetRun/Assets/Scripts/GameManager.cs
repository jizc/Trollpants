// <copyright file="GameManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun
{
    using Character;
    using CloudOnce;
    using Data;
    using GUI;
    using UnityEngine;
    using World;

    [RequireComponent(typeof(CharacterManager))]
    [RequireComponent(typeof(SessionData))]
    public class GameManager : MonoBehaviour
    {
        [Range(0, 10)] [SerializeField] private float deathDuration = 0.5f;
        [SerializeField] private AndroidUtils androidUtils;
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private ObjectPooler objectPooler;
        [SerializeField] private GameObject theWorld;

        private CharacterManager characterManager;
        private SessionData sessionData;
        private GameObject character;
        private Vector3 characterOrigin;
        private PanelManager panelManager;
        private ScoreSupervisor scoreSupervisor;
        private Transform objectContainer;
        private RotateAndSpawn rotateAndSpawn;
        private Controller controller;
        private GameObject[] backgroundParents;

        public void GameStart()
        {
            rotateAndSpawn.SetSpawning(true);
            controller.enabled = true;
        }

        public void PlayerDeath()
        {
            rotateAndSpawn.SetSpawning(false);
            rotateAndSpawn.SetSpeed(0f);
            character.SetActive(false);
            controller.enabled = false;
            cameraShake.Shake();

            DoAchievementsAndLeaderboards();

            Invoke("ResetGame", deathDuration);
            Invoke("EnableScoreScreen", deathDuration);
        }

        public void SaveOnExit()
        {
            PlayerSettings.Save();
            Cloud.Storage.Save();
        }

        private void DoAchievementsAndLeaderboards()
        {
            var score = sessionData.Score;
            CloudVariables.TotalThingsDodged += score;

            // New highscore?
            if (score > CloudVariables.HighScore)
            {
                CloudVariables.HighScore = score;
                Achievements.Score100.Increment(score, 100);
                Achievements.Score250.Increment(score, 250);
            }

            // Cherry achievements
            var cherriesThisRun = sessionData.CherriesThisRun;
            var totalCherries = CloudVariables.TotalCollectedCherries;

            Achievements.CherryPicker.Increment(totalCherries, 250);
            Achievements.CherryHarvester.Increment(totalCherries, 1000);
            Achievements.CherryHoarder.Increment(totalCherries, 5000);
            Achievements.CherryPie.Increment(cherriesThisRun, 25);
            Achievements.CherryBomb.Increment(cherriesThisRun, 100);

            // Things dodged
            var thingsDodged = CloudVariables.TotalThingsDodged;
            Achievements.SkilledDodger.Increment(thingsDodged, 1000);
            Achievements.ExpertDodger.Increment(thingsDodged, 5000);

            // Characters unlocked achievements
            Achievements.MoreTheMerrier.Increment(SessionData.UnlockedCharacters, 3);
            Achievements.RunnersAssemble.Increment(SessionData.UnlockedCharacters, 6);

            // Update leaderboard
            Leaderboards.HighScore.SubmitScore(sessionData.Score);

            Cloud.Storage.Save();
        }

        private void Awake()
        {
            Cloud.OnCloudLoadComplete += DeactivateSplashScreen;

            cameraShake.Duration = deathDuration;

            sessionData = GetComponent<SessionData>();
            characterManager = GetComponent<CharacterManager>();
            character = GameObject.FindWithTag("Player");
            controller = character.GetComponent<Controller>();
            objectContainer = GameObject.Find("ObjectContainer").transform;
            characterOrigin = character.transform.position;
            panelManager = GameObject.FindWithTag("Canvas").GetComponent<PanelManager>();
            scoreSupervisor = transform.GetComponent<ScoreSupervisor>();
            rotateAndSpawn = theWorld.GetComponentInChildren<RotateAndSpawn>();
            backgroundParents = GameObject.FindGameObjectsWithTag("Backgrounds");
            SetNewBackground();
        }

        private void DeactivateSplashScreen(bool arg0)
        {
            characterManager.Init();
            panelManager.Init();
            Cloud.OnCloudLoadComplete -= DeactivateSplashScreen;
        }

        private void EnableScoreScreen()
        {
            SetNewBackground();
            scoreSupervisor.Init();
            panelManager.ShowScore();
            scoreSupervisor.SetHighScore();
            scoreSupervisor.SetSessionScore();
            sessionData.ResetProperties();
        }

        private void ResetGame()
        {
            var childCount = objectContainer.childCount;
            for (var i = 0; i < childCount; i++)
            {
                objectPooler.Recycle(objectContainer.GetChild(0).gameObject);
            }

            scoreSupervisor.Init();
            character.transform.position = characterOrigin;
            rotateAndSpawn.SetSpeed(0.9f);
            character.SetActive(true);
            controller.ResetCharacter();
            controller.enabled = false;
            theWorld.SetActive(true);
        }

        private void SetNewBackground()
        {
            var random = -1;

            foreach (var parent in backgroundParents)
            {
                for (var i = 0; i < parent.transform.childCount; i++)
                {
                    parent.transform.GetChild(i).gameObject.SetActive(false);
                }

                if (random <= -1)
                {
                    random = Random.Range(0, parent.transform.childCount);
                }

                parent.transform.GetChild(random).gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (panelManager.CurrentMenu == panelManager.Options
                    || panelManager.CurrentMenu == panelManager.CharacterSelect)
                {
                    Cloud.Storage.Save();
                    panelManager.ShowPreviousMenu();
                }
                else if (panelManager.CurrentMenu == panelManager.Score)
                {
                    panelManager.GoToMainMenu();
                }
                else if (panelManager.CurrentMenu == panelManager.Hud)
                {
                    rotateAndSpawn.SetSpawning(false);
                    sessionData.ResetProperties();
                    ResetGame();
                    panelManager.GoToMainMenu();
                }
                else
                {
#if UNITY_ANDROID
                    androidUtils.ShowSelectDialog(
                        "Exit Mini Planet Run?",
                        "Are you sure you want to exit?",
                        b =>
                        {
                            if (b)
                            {
                                Application.Quit();
                            }

                            Audio.AudioClipPlayer.PlayButton();
                        });
#endif
                }
            }
        }
    }
}
