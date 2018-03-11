// <copyright file="PanelManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.GUI
{
    using CloudOnce;
    using UnityEngine;

    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject characterSelect;
        [SerializeField] private GameObject options;
        [SerializeField] private GameObject hud;
        [SerializeField] private GameObject score;
        [SerializeField] private GameObject tutorial;
        [SerializeField] private GameObject splashScreen;

        private GameObject previousMenu;

        public GameObject CurrentMenu { get; private set; }
        public GameObject CharacterSelect => characterSelect;
        public GameObject Options => options;
        public GameObject Score => score;
        public GameObject Hud => hud;

        public void ShowMainMenu()
        {
            ShowMenu(mainMenu);
        }

        public void ShowCharacterSelect()
        {
            ShowMenu(characterSelect);
        }

        public void ShowOptions()
        {
            ShowMenu(options);
        }

        public void ShowHud()
        {
            ShowMenu(hud);
        }

        public void ShowScore()
        {
            ShowMenu(score);
        }

        public void ShowTutorial()
        {
            ShowMenu(tutorial);
        }

        public void ShowPreviousMenu()
        {
            if (previousMenu == null)
            {
#if DEBUG
                Debug.Log("No previous menu to show.");
#endif
                return;
            }

            ShowMenu(previousMenu);
        }

        public void PlayPressed()
        {
            if (!CloudVariables.HasSeenTutorial)
            {
                ShowMenu(tutorial);
                return;
            }

            ShowMenu(hud);
            gameManager.GameStart();
        }

        public void SaveOnExitPressed()
        {
            Cloud.Storage.Save();
        }

        public void GoToMainMenu()
        {
            ShowMenu(mainMenu);
        }

        public void Init()
        {
            splashScreen.SetActive(false);
            InitMenu(hud, false);
            InitMenu(characterSelect, false);
            InitMenu(options, false);
            InitMenu(score, false);
            InitMenu(tutorial, false);

            InitMenu(mainMenu, true);
            previousMenu = mainMenu;
            CurrentMenu = mainMenu;
        }

        private static void InitMenu(GameObject menu, bool setActive)
        {
            var rect = menu.GetComponent<RectTransform>();
            rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
            menu.SetActive(setActive);
        }

        private void ShowMenu(GameObject menu)
        {
            if (CurrentMenu != null)
            {
                CurrentMenu.SetActive(false);
            }

            if (menu == tutorial && !CloudVariables.HasSeenTutorial)
            {
                CloudVariables.HasSeenTutorial = true;
            }

            menu.gameObject.SetActive(true);

            previousMenu = CurrentMenu;
            CurrentMenu = menu;
        }
    }
}
