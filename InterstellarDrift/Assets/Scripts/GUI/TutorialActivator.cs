// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TutorialActivator.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using CloudOnce;
    using UnityEngine;

    public class TutorialActivator : MonoBehaviour
    {
        private GameObject tutorialCanvas;

        private void Awake()
        {
            tutorialCanvas = GameObject.FindWithTag("TutorialCanvas");
        }

        private void Start()
        {
            tutorialCanvas.SetActive(!CloudVariables.HasFinishedTutorial);
        }
    }
}
