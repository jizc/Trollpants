// <copyright file="CharacterManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.Character
{
    using System.Collections.Generic;
    using System.Linq;
    using Audio;
    using CloudOnce;
    using Data;
    using GUI;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(SessionData))]
    public class CharacterManager : MonoBehaviour
    {
        private const int char2Cost = 50;
        private const int char3Cost = 100;
        private const int char4Cost = 200;
        private const int char5Cost = 400;
        private const int char6Cost = 800;

        [SerializeField] private AndroidUtils androidUtils;
        [SerializeField] private Text cherriesText;

        private SessionData sessionData;
        private Controller controller;
        private List<GameObject> characters;
        private GameObject currentCharacter;
        private PanelManager panelManager;
        private List<CharacterBox> characterBoxes;

        public void Ch1Selected()
        {
            if (currentCharacter == characters[0])
            {
                panelManager.GoToMainMenu();
                return;
            }

            PlayerSettings.CurrentCharacterId = 0;
            SetCurrentCharacter(characters[0]);
        }

        public void Ch2Selected()
        {
            BuyableCharacter(1, char2Cost, CloudVariables.Char2Unlocked);
        }

        public void Ch3Selected()
        {
            BuyableCharacter(2, char3Cost, CloudVariables.Char3Unlocked);
        }

        public void Ch4Selected()
        {
            BuyableCharacter(3, char4Cost, CloudVariables.Char4Unlocked);
        }

        public void Ch5Selected()
        {
            BuyableCharacter(4, char5Cost, CloudVariables.Char5Unlocked);
        }

        public void Ch6Selected()
        {
            BuyableCharacter(5, char6Cost, CloudVariables.Char6Unlocked);
        }

        public void Ch7Selected()
        {
            if (currentCharacter == characters[6])
            {
                panelManager.GoToMainMenu();
                return;
            }

            PlayerSettings.CurrentCharacterId = 6;
            SetCurrentCharacter(characters[6]);
        }

        public void Ch8Selected()
        {
            if (currentCharacter == characters[7])
            {
                panelManager.GoToMainMenu();
                return;
            }

            PlayerSettings.CurrentCharacterId = 7;
            SetCurrentCharacter(characters[7]);
        }

        public void Init()
        {
            sessionData = GetComponent<SessionData>();
            sessionData.CherriesChanged += OnCherriesChanged;
            OnCherriesChanged(sessionData.Cherries);

            controller = GameObject.FindWithTag("Player").GetComponent<Controller>();
            panelManager = GameObject.FindWithTag("Canvas").GetComponent<PanelManager>();

            var c = GameObject.FindWithTag("Player").transform;
            characters = new List<GameObject>();
            for (var i = 0; i < c.childCount; i++)
            {
                var child = c.GetChild(i);
                if (child.CompareTag("Character"))
                {
                    characters.Add(child.gameObject);
                }
            }

            characters = characters.OrderBy(g => g.name).ToList();

            foreach (var ch in characters)
            {
                ch.SetActive(false);
            }

            // Fill the characterBox-list
            characterBoxes = new List<CharacterBox>();

            var boxes = GameObject.FindGameObjectsWithTag("CharacterBox");
            foreach (var box in boxes)
            {
                var boxScript = box.GetComponent<CharacterBox>();

                characterBoxes.Add(boxScript);
                boxScript.Init();
            }

            characterBoxes = characterBoxes.OrderBy(j => j.CharacterNumber).ToList();

            RefreshCharacterMenu();

            SetCurrentCharacter(characters[PlayerSettings.CurrentCharacterId]);
        }

        private static bool CheckUnlocked(int characterIndex)
        {
            switch (characterIndex)
            {
                case 0:
                    return true;
                case 1:
                    return CloudVariables.Char2Unlocked;
                case 2:
                    return CloudVariables.Char3Unlocked;
                case 3:
                    return CloudVariables.Char4Unlocked;
                case 4:
                    return CloudVariables.Char5Unlocked;
                case 5:
                    return CloudVariables.Char6Unlocked;
                case 6:
                case 7:
                    return true;
                default:
#if DEBUG
                    Debug.LogWarning("Checking Unlocked: Character index out of bounds.");
#endif
                    return false;
            }
        }

        private static int GetCharacterCost(int characterIndex)
        {
            switch (characterIndex)
            {
                case 0:
                    return 0;
                case 1:
                    return char2Cost;
                case 2:
                    return char3Cost;
                case 3:
                    return char4Cost;
                case 4:
                    return char5Cost;
                case 5:
                    return char6Cost;
                default:
#if DEBUG
                    Debug.LogWarning("Checking Character Cost: Character index out of bounds.");
#endif
                    return -1;
            }
        }

        private static void UnlockCharacter(int characterIndex)
        {
            AudioClipPlayer.PlayNewCharacter();

            switch (characterIndex)
            {
                case 1:
                    CloudVariables.Char2Unlocked = true;
                    return;
                case 2:
                    CloudVariables.Char3Unlocked = true;
                    return;
                case 3:
                    CloudVariables.Char4Unlocked = true;
                    return;
                case 4:
                    CloudVariables.Char5Unlocked = true;
                    return;
                case 5:
                    CloudVariables.Char6Unlocked = true;
                    return;
            }
        }

        private static void UpdateCharacterBoxGraphics(CharacterBox targetCharacterBox)
        {
            if (!targetCharacterBox.gameObject.activeInHierarchy)
            {
                return;
            }

            targetCharacterBox.IsUnlocked = CheckUnlocked(targetCharacterBox.CharacterNumber);

            if (targetCharacterBox.IsBuyable)
            {
                if (!targetCharacterBox.IsUnlocked)
                {
                    targetCharacterBox.CherryCost = GetCharacterCost(targetCharacterBox.CharacterNumber);
                }
            }

            targetCharacterBox.IsSelected = targetCharacterBox.CharacterNumber == PlayerSettings.CurrentCharacterId;
        }

        private void BuyableCharacter(int charIndex, int cost, bool isUnlocked)
        {
            if (isUnlocked)
            {
                if (currentCharacter == characters[charIndex])
                {
                    panelManager.GoToMainMenu();
                    return;
                }

                PlayerSettings.CurrentCharacterId = charIndex;
                SetCurrentCharacter(characters[charIndex]);
            }
            else if (CloudVariables.Cherries >= cost)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                androidUtils.ShowSelectDialog(
                    "Are you sure?",
                    "Unlock this character with " + cost + " cherries?",
                    confirm =>
                    {
                        if (confirm)
                        {
                            BuyCharacterImpl(charIndex, cost);
                        }
                    });
#else
                BuyCharacterImpl(charIndex, cost);
#endif
            }
            else
            {
#if DEBUG
                Debug.Log("Not enough cherries");
#endif
            }
        }

        private void BuyCharacterImpl(int charIndex, int cost)
        {
            sessionData.Cherries -= cost;
            UnlockCharacter(charIndex);
            PlayerSettings.CurrentCharacterId = charIndex;
            SetCurrentCharacter(characters[charIndex]);
            Achievements.MoreTheMerrier.Increment(SessionData.UnlockedCharacters, 3);
            Achievements.RunnersAssemble.Increment(SessionData.UnlockedCharacters, 6);

            Cloud.Storage.Save();
#if DEBUG
            Debug.Log("Unlocked character");
#endif
        }

        private void RefreshCharacterMenu()
        {
            // Goes through all the GUI-boxes and sets the currently selected one
            // This also shows the appropriate graphic
            foreach (var character in characterBoxes)
            {
                UpdateCharacterBoxGraphics(character);
            }
        }

        private void SetCurrentCharacter(GameObject character)
        {
            if (currentCharacter != null)
            {
                currentCharacter.SetActive(false);
            }

            currentCharacter = character;
            currentCharacter.SetActive(true);

            controller.SetAnimator(currentCharacter.GetComponent<Animator>());

            RefreshCharacterMenu();
        }

        private void OnCherriesChanged(int count)
        {
            cherriesText.text = count.ToString();
        }
    }
}
