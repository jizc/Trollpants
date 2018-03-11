// <copyright file="HowToPlayController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEngine;

    [RequireComponent(typeof(PanelAnimatorHelper))]
    public class HowToPlayController : MonoBehaviour
    {
        [SerializeField] private GameStateChanger gameStateChanger;
        [SerializeField] private GameObject[] contents;
        [SerializeField] private GameObject doneButton;
        [SerializeField] private GameObject playButton;

        public void ActivateHowTo()
        {
            GetComponent<PanelAnimatorHelper>().ShowPanel();
            DeactivateAllContents(contents[0]);

            switch (gameStateChanger.CurrentGameState)
            {
                case GameState.HowToPlay:
                    doneButton.SetActive(false);
                    playButton.SetActive(true);
                    break;
                default:
                    doneButton.SetActive(true);
                    playButton.SetActive(false);
                    break;
            }
        }

        private void Awake()
        {
            DeactivateAllContents(contents[0]);
        }

        private void DeactivateAllContents(GameObject exception)
        {
            foreach (var c in contents)
            {
                if (Equals(c, exception))
                {
                    c.SetActive(true);
                    continue;
                }

                c.SetActive(false);
            }
        }
    }
}
