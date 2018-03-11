// <copyright file="TextAnimator.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    public class TextAnimator : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 1f)] private float updateInterval = 0.5f;
        [SerializeField] private List<string> stringList = new List<string>();

        private Text textComponent;
        private float currentIntervalValue;
        private int currentIndex;

        private void Awake()
        {
            textComponent = GetComponent<Text>();
            UpdateText();
        }

        private void Update()
        {
            if (currentIntervalValue > 0f)
            {
                currentIntervalValue -= Time.deltaTime;
                return;
            }

            IncrementIndex();
            UpdateText();
            currentIntervalValue = updateInterval;
        }

        private void IncrementIndex()
        {
            if (currentIndex + 1 == stringList.Count)
            {
                currentIndex = 0;
                return;
            }

            currentIndex++;
        }

        private void UpdateText()
        {
            textComponent.text = stringList[currentIndex];
        }
    }
}
