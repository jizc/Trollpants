// <copyright file="WorldScroller.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System.Collections;
    using Data;
    using Player;
    using UnityEngine;

    public class WorldScroller : MonoBehaviour
    {
        private const float boostAcceleration = 8f; // How quickly boosting will get the scroll speed up to maximum
        private const float defaultBaseScrollSpeed = 0.005f;

        private static WorldScroller s_instance;

        [SerializeField] private Transform playerCanvasTransform;
        [SerializeField] private Transform activeCloudsTransform;
        [SerializeField] private CanvasGroup vignetteCanvasGroup;
        [SerializeField] private Camera mainCamera;

        private Transform cameraTransform;
        private Transform playerTransform;
        private CameraZoomer cameraZoomer;
        private float baseScrollSpeed = 5f;
        private float scrollSpeed;
        private bool isResettingDefaultScrollSpeed;

        private Vector3 cameraStartPosition;
        private Vector3 playerStartPosition;
        private Vector3 playerCanvasStartPosition;
        private Vector3 activeCloudsStartPosition;

        public static float StarsScrollSpeed
        {
            get { return defaultBaseScrollSpeed; }
        }

        public static float LandscapeScrollSpeed
        {
            get { return defaultBaseScrollSpeed * 2f; }
        }

        public static float CloudScrollSpeedBack
        {
            get { return ScrollSpeed * defaultBaseScrollSpeed * 60f; }
        }

        public static float CloudScrollSpeedMid
        {
            get { return ScrollSpeed * defaultBaseScrollSpeed * 70f; }
        }

        public static float CloudScrollSpeedFront
        {
            get { return ScrollSpeed * defaultBaseScrollSpeed * 80f; }
        }

        public static float ScrollSpeed
        {
            get { return GameState.IsPaused ? 0f : s_instance.scrollSpeed; }
            private set { s_instance.scrollSpeed = value; }
        }

        public static float BaseScrollSpeed
        {
            get { return s_instance.baseScrollSpeed; }
            private set { s_instance.baseScrollSpeed = value; }
        }

        public static float CameraZBias
        {
            get { return s_instance.cameraZoomer.CameraZBias; }
        }

        public static void ResetPositions()
        {
            ScrollSpeed = 0f;
            s_instance.cameraTransform.position = s_instance.cameraStartPosition;
            s_instance.playerTransform.position = s_instance.playerStartPosition;
            s_instance.activeCloudsTransform.position = s_instance.activeCloudsStartPosition;
            s_instance.playerCanvasTransform.position = s_instance.playerCanvasStartPosition;
            s_instance.cameraZoomer.Reset();
        }

        public static IEnumerator ActiveCheckpointSpeed()
        {
            const float targetSpeed = 2.5f;
            var delta = BaseScrollSpeed - targetSpeed;

            while (BaseScrollSpeed > targetSpeed)
            {
                if (s_instance.isResettingDefaultScrollSpeed)
                {
                    break;
                }

                BaseScrollSpeed = Mathf.MoveTowards(BaseScrollSpeed, targetSpeed, delta * Time.deltaTime * 2f);
                yield return null;
            }
        }

        public static IEnumerator ResetDefaultScrollSpeed()
        {
            const float targetSpeed = 5f;
            var delta = Mathf.Abs(BaseScrollSpeed - targetSpeed);
            s_instance.isResettingDefaultScrollSpeed = true;

            while (BaseScrollSpeed < targetSpeed)
            {
                BaseScrollSpeed = Mathf.MoveTowards(BaseScrollSpeed, targetSpeed, delta * Time.deltaTime * 2f);
                yield return null;
            }

            s_instance.isResettingDefaultScrollSpeed = false;
        }

        private void Awake()
        {
            s_instance = this;
            cameraTransform = mainCamera.transform;
            cameraStartPosition = cameraTransform.position;
            activeCloudsStartPosition = activeCloudsTransform.position;
            playerCanvasStartPosition = playerCanvasTransform.position;
        }

        private void Start()
        {
            playerTransform = Player.Transform;
            playerStartPosition = playerTransform.position;
            cameraZoomer = new CameraZoomer(cameraTransform);
        }

        private void Update()
        {
            if (GameState.IsPaused || Player.State.IsDead)
            {
                return;
            }

            var currentBoostSpeed = Player.IsBoosting ? Player.State.BoostSpeed : 0f;
            ScrollSpeed = Mathf.MoveTowards(ScrollSpeed, baseScrollSpeed + currentBoostSpeed, boostAcceleration * Time.deltaTime);

            var xDelta = ScrollSpeed * Time.deltaTime;

            ScrollCamera(xDelta);
            ScrollPlayerCanvas(xDelta);
            ScrollActiveClouds(xDelta);

            cameraZoomer.AdjustCameraZoom(xDelta, Player.IsBoosting);
            vignetteCanvasGroup.alpha = -cameraZoomer.CameraZBias;
        }

        private void ScrollCamera(float xDelta)
        {
            cameraTransform.Translate(xDelta, 0f, 0f);
        }

        private void ScrollPlayerCanvas(float xDelta)
        {
            playerCanvasTransform.position =
                new Vector3(playerCanvasTransform.position.x + xDelta, playerTransform.position.y + 0.5f, 0f);
        }

        private void ScrollActiveClouds(float xDelta)
        {
            activeCloudsTransform.Translate(xDelta, 0f, 0f);
        }
    }
}
