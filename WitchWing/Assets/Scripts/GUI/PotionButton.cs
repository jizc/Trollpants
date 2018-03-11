// <copyright file="PotionButton.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System;
    using Player;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class PotionButton : MonoBehaviour
    {
        [SerializeField] private PotionType potionType;

        private Button button;

        private enum PotionType
        {
            Wisdom,
            Midas
        }

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnEnable()
        {
            switch (potionType)
            {
                case PotionType.Wisdom:
                    button.interactable = Player.State.WisdomPotions > 0;
                    break;
                case PotionType.Midas:
                    button.interactable = Player.State.MidasPotions > 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnClick()
        {
            switch (potionType)
            {
                case PotionType.Wisdom:
                    if (Player.State.IsWisdomActive || Player.State.WisdomPotions == 0)
                    {
                        return;
                    }

                    Player.State.WisdomPotions--;

                    break;
                case PotionType.Midas:
                    if (Player.State.IsMidasActive || Player.State.MidasPotions == 0)
                    {
                        return;
                    }

                    Player.State.MidasPotions--;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
