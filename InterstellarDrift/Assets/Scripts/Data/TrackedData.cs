// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackedData.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class TrackedData : MonoBehaviour
    {
        private static TrackedData s_instance;

        public static TrackedData Instance
        {
            get
            {
                return s_instance;
            }

            private set
            {
                if (s_instance != null)
                {
                    return;
                }

                s_instance = value;
            }
        }

        public SessionData SessionData { get; private set; }

        public void SessionEnd()
        {
            CloudVariables.DistanceTravelled += SessionData.DistanceTravelled;
            CloudVariables.PersonalBestScore = SessionData.Score;
            CloudVariables.LongestTimeSurvived = SessionData.SecondsSurvived;

            // Push the _session_ score to the leaderboard and then report achievements
            Leaderboards.HighScores.SubmitScore(SessionData.Score);
#if UNITY_IOS || CLOUDONCE_AMAZON
            // The format-setting of the leaderboard dictates the value it wants. It is set to "Elapsed Time - To The Second".
            Leaderboards.TimeAttack.SubmitScore(SessionData.SecondsSurvived);
#else
            // Google Play leaderboards that receive time-values only accept milliseconds.
            Leaderboards.TimeAttack.SubmitScore(SessionData.MillisecondsSurvived);
#endif

            ReportAchievements();

            Cloud.Storage.Save();
        }

        private static void ReportAchievements()
        {
            // Relative distances to solar system major bodies
            Achievements.EarthToMoon.Increment(CloudVariables.DistanceTravelled, 3000);
            Achievements.SunToMercury.Increment(CloudVariables.DistanceTravelled, 557280);
            Achievements.SunToVenus.Increment(CloudVariables.DistanceTravelled, 1041120);
            Achievements.SunToEarth.Increment(CloudVariables.DistanceTravelled, 1440000);
            Achievements.SunToMars.Increment(CloudVariables.DistanceTravelled, 2194560);
            Achievements.SunToJupiter.Increment(CloudVariables.DistanceTravelled, 7492320);
            Achievements.SunToSaturn.Increment(CloudVariables.DistanceTravelled, 13736160);
            Achievements.SunToUranus.Increment(CloudVariables.DistanceTravelled, 27635040);
            Achievements.SunToNeptune.Increment(CloudVariables.DistanceTravelled, 43287840);
            Achievements.SunToPluto.Increment(CloudVariables.DistanceTravelled, 56880000);

            Achievements.ItBegins.Increment(CloudVariables.PersonalBestScore, 1000);
            Achievements.GalaxyDrift.Increment(CloudVariables.PersonalBestScore, 5000);
            Achievements.Masteroid.Increment(CloudVariables.PersonalBestScore, 25000);
        }

        private void Awake()
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SessionData = new SessionData();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.buildIndex > 0)
            {
                SessionData.Init();
            }
        }
    }
}
