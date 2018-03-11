// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadingText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///  Animates a 'Loading...' sentence in a text-box.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LoadingText : MonoBehaviour
    {
        public bool UpperCase;

        private readonly string[] displayTexts = { "Loading.", "Loading..", "Loading..." };

        [SerializeField] private float _changeDelay = 0.75f;

        private Text textComponent;
        private float counter;
        private int currentIndex;

        private void Awake()
        {
            textComponent = GetComponent<Text>();
            textComponent.text = GetStringInArray(currentIndex, UpperCase);
        }

        private void Update()
        {
            if (!textComponent)
            {
                return;
            }

            counter += Time.deltaTime;

            if (counter >= _changeDelay)
            {
                counter = 0f;

                currentIndex++;
                if (currentIndex >= displayTexts.Length)
                {
                    currentIndex = 0;
                }

                textComponent.text = GetStringInArray(currentIndex, UpperCase);
            }
        }

        private string GetStringInArray(int nextIndex, bool upperCase)
        {
            if (nextIndex >= displayTexts.Length)
            {
                return null;
            }

            var nextString = displayTexts[nextIndex];
            if (upperCase)
            {
                nextString = nextString.ToUpper();
            }

            return nextString;
        }
    }
}
