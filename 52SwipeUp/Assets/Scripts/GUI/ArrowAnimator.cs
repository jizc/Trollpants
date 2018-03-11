// <copyright file="ArrowAnimator.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class ArrowAnimator : MonoBehaviour
    {
        [SerializeField] private Axis2D axis;
        [SerializeField] private float distance;
        [SerializeField] [Range(0.1f, 10f)] private float speed;
        [SerializeField] private bool teleportToStart;
        [SerializeField] private bool halfSpeedOnWayBack;
        [SerializeField] private bool fadeImageAlpha;

        private RectTransform rectTransform;
        private Image image;

        private Vector2 startPosition;
        private Vector2 targetPosition;
        private bool isMovingBack;

        private Color startColor;
        private Color targetColor;
        private float timeElapsed;

        private bool isInitialized;

        private enum Axis2D
        {
            X,
            Y
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();

            startPosition = rectTransform.anchoredPosition;
            startColor = image.color;
            targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            switch (axis)
            {
                case Axis2D.X:
                    targetPosition = new Vector2(startPosition.x + distance, startPosition.y);
                    break;
                case Axis2D.Y:
                    targetPosition = new Vector2(startPosition.x, startPosition.y + distance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            isInitialized = true;
        }

        private void OnEnable()
        {
            if (isInitialized)
            {
                rectTransform.anchoredPosition = startPosition;
                isMovingBack = false;
                timeElapsed = 0f;
            }
        }

        private void Update()
        {
            if (distance < 0f)
            {
                distance = -distance;
            }

            timeElapsed += Time.deltaTime;

            if (!isMovingBack)
            {
                if (fadeImageAlpha)
                {
                    image.color = Color.Lerp(startColor, targetColor, timeElapsed * speed);
                }

                rectTransform.anchoredPosition = Vector2.MoveTowards(
                    rectTransform.anchoredPosition,
                    targetPosition,
                    distance * Time.deltaTime * speed);
                if (rectTransform.anchoredPosition == targetPosition)
                {
                    if (fadeImageAlpha)
                    {
                        image.color = targetColor;
                        timeElapsed = 0f;
                    }

                    isMovingBack = true;
                }
            }
            else if (teleportToStart && isMovingBack)
            {
                rectTransform.anchoredPosition = startPosition;
                if (fadeImageAlpha)
                {
                    image.color = startColor;
                    timeElapsed = 0f;
                }

                isMovingBack = false;
            }
            else
            {
                if (fadeImageAlpha)
                {
                    image.color = Color.Lerp(
                        targetColor,
                        startColor,
                        timeElapsed * (speed * (halfSpeedOnWayBack ? 0.5f : 1f)));
                }

                rectTransform.anchoredPosition = Vector2.MoveTowards(
                    rectTransform.anchoredPosition,
                    startPosition,
                    distance * Time.deltaTime * (speed * (halfSpeedOnWayBack ? 0.5f : 1f)));

                if (rectTransform.anchoredPosition == startPosition)
                {
                    if (fadeImageAlpha)
                    {
                        image.color = startColor;
                        timeElapsed = 0f;
                    }

                    isMovingBack = false;
                }
            }
        }
    }
}
