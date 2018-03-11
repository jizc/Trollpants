// <copyright file="PanelBucket.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PanelBucket : MonoBehaviour
    {
        [SerializeField] private GameObject splashScreen;
        [SerializeField] private GameObject startMenu;
        [SerializeField] private GameObject options;
        [SerializeField] private GameObject howToPlay;
        [SerializeField] private GameObject instructions;
        [SerializeField] private GameObject hud;
        [SerializeField] private GameObject roundResults;
        [SerializeField] private GameObject gameResults;

        private List<GameObject> allPanels;

        public GameObject SplashScreen
        {
            get { return splashScreen; }
        }

        public GameObject StartMenu
        {
            get { return startMenu; }
        }

        public GameObject Options
        {
            get { return options; }
        }

        public GameObject HowToPlay
        {
            get { return howToPlay; }
        }

        public GameObject Instructions
        {
            get { return instructions; }
        }

        public GameObject Hud
        {
            get { return hud; }
        }

        public GameObject RoundResults
        {
            get { return roundResults; }
        }

        public GameObject GameResults
        {
            get { return gameResults; }
        }

        public void DisableAllPanels()
        {
            foreach (var panel in allPanels)
            {
                var canvasGroup = panel.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.interactable = false;
                }
            }
        }

        public void EnableAllPanels()
        {
            foreach (var panel in allPanels)
            {
                var canvasGroup = panel.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.interactable = true;
                }
            }
        }

        public void DeactivateAllPanels()
        {
            foreach (var panel in allPanels)
            {
                if (panel.GetComponent<PanelAnimatorHelper>() != null)
                {
                    panel.GetComponent<PanelAnimatorHelper>().HidePanelInstantly();
                }
                else
                {
                    panel.SetActive(false);
                }
            }
        }

        public void DeactivateAllPanels(GameObject exception)
        {
            foreach (var panel in allPanels)
            {
                if (panel.GetComponent<PanelAnimatorHelper>() != null)
                {
                    if (panel == exception)
                    {
                        panel.GetComponent<PanelAnimatorHelper>().ShowPanel();
                        continue;
                    }

                    panel.GetComponent<PanelAnimatorHelper>().HidePanelInstantly();
                }
                else
                {
                    if (panel == exception)
                    {
                        panel.SetActive(true);
                        continue;
                    }

                    panel.SetActive(false);
                }
            }
        }

        private void Awake()
        {
            allPanels = new List<GameObject>
            {
                splashScreen,
                startMenu,
                options,
                howToPlay,
                instructions,
                hud,
                roundResults,
                gameResults
            };
        }
    }
}
