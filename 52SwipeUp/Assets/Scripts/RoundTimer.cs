// <copyright file="RoundTimer.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp
{
    using System;
    using GUI;
    using UnityEngine;

    [RequireComponent(typeof(GameStateChanger))]
    [RequireComponent(typeof(PanelBucket))]
    [RequireComponent(typeof(SessionData))]
    public class RoundTimer : MonoBehaviour
    {
        [SerializeField] private float interval = 0.1f;

        private GameStateChanger gameStateChanger;
        private PanelBucket panelBucket;
        private SessionData sessionData;

        private float currentInterval;
        private bool timerActive;

        public float Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public void StartTimer()
        {
            timerActive = true;
        }

        public void StopTimer()
        {
            timerActive = false;
        }

        public void ResetTimer()
        {
            sessionData.TimeLeft = 30f;
            timerActive = false;
        }

        private void Awake()
        {
            gameStateChanger = GetComponent<GameStateChanger>();
            panelBucket = GetComponent<PanelBucket>();
            sessionData = GetComponent<SessionData>();

            currentInterval = interval;
        }

        private void Update()
        {
            if (!timerActive)
            {
                return;
            }

            if (currentInterval > 0)
            {
                currentInterval -= Time.deltaTime;
                return;
            }

            currentInterval += interval;

            sessionData.TimeLeft -= interval;

            if (Math.Abs(sessionData.TimeLeft) < 0.001f)
            {
                StopTimer();
                panelBucket.Hud.GetComponent<CanvasGroup>().interactable = false;
                panelBucket.Hud.GetComponent<PanelAnimatorHelper>().HidePanel();
                PlayerSettings.MovementEnabled = false;
                gameStateChanger.ChangeGameState(GameState.GameEnd, 0.4f);
            }
        }
    }
}
