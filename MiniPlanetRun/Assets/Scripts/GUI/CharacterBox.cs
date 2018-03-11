// <copyright file="CharacterBox.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterBox : MonoBehaviour
    {
        [SerializeField] private bool isBuyable;
        [SerializeField] private int characterNumber;

        private Text cherryCostText;
        private GameObject unlockedGfx;
        private GameObject lockedGfx;
        private GameObject selectedGfx;

        private bool isUnlocked;
        private bool isSelected;
        private int cherryCost;

        public bool IsBuyable
        {
            get { return isBuyable; }
        }

        public int CharacterNumber
        {
            get { return characterNumber; }
            private set { characterNumber = value; }
        }

        public bool IsUnlocked
        {
            get { return isUnlocked; }
            set
            {
                isUnlocked = value;

                lockedGfx.SetActive(!isUnlocked);
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;

                selectedGfx.SetActive(isSelected);
            }
        }

        public int CherryCost
        {
            get { return cherryCost; }
            set
            {
                cherryCost = value;
                cherryCostText.text = string.Empty + cherryCost;
            }
        }

        public void Init()
        {
            unlockedGfx = transform.GetChild(0).gameObject;
            lockedGfx = transform.GetChild(1).gameObject;
            selectedGfx = transform.GetChild(2).gameObject;

            unlockedGfx.SetActive(true);
            lockedGfx.SetActive(true);
            selectedGfx.SetActive(true);

            if (isBuyable)
            {
                cherryCostText = lockedGfx.GetComponentInChildren<Text>();
            }
        }
    }
}
