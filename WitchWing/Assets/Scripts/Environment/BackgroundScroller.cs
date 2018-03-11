// <copyright file="BackgroundScroller.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using System;
    using Data;
    using Player;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(RawImage))]
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private Background backgroundType;

        private RawImage backgroundImage;
        private RectTransform backgroundTransform;
        private Vector3 startPosition;

        private float textureOffset;
        private float speed;

        private enum Background
        {
            Stars,
            Landscape
        }

        private void Awake()
        {
            backgroundTransform = (RectTransform)transform;
            backgroundImage = GetComponent<RawImage>();
            startPosition = backgroundTransform.anchoredPosition3D;
        }

        private void Start()
        {
            switch (backgroundType)
            {
                case Background.Stars:
                    speed = WorldScroller.StarsScrollSpeed;
                    break;
                case Background.Landscape:
                    speed = WorldScroller.LandscapeScrollSpeed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            if (GameState.IsPaused || Player.State.IsDead)
            {
                return;
            }

            textureOffset += speed * WorldScroller.ScrollSpeed * Time.deltaTime;
            if (textureOffset > 1f)
            {
                textureOffset -= 1f;
            }

            backgroundImage.uvRect = new Rect(textureOffset, 0f, 1f, 1f);

            var targetPosition = startPosition + new Vector3(
                0f,
                WorldScroller.CameraZBias * -50f,
                WorldScroller.CameraZBias * (backgroundType == Background.Stars ? -100f : -80f));

            backgroundTransform.anchoredPosition3D = Vector3.MoveTowards(
                backgroundTransform.anchoredPosition3D, targetPosition, 500f * Time.deltaTime);
        }
    }
}
