// <copyright file="ManaPool.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Player
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ManaPool
    {
        public static readonly Color32 ManaFillDefault;

        private const float manaDrainRate = 100f;
        private static readonly Color32 s_manaFillEmpty;
        private readonly Image manaFill;

        static ManaPool()
        {
            ManaFillDefault = new Color32(131, 0, 178, 255);
            s_manaFillEmpty = new Color32(178, 0, 0, 255);
        }

        public ManaPool(Image manaFill)
        {
            this.manaFill = manaFill;
        }

        public float CurrentManaAmount { get; private set; }

        public void ResetManaAmount()
        {
            CurrentManaAmount = Player.State.ManaAmount;
        }

        public void RegenerateMana()
        {
            CurrentManaAmount = Mathf.MoveTowards(
                CurrentManaAmount,
                Player.State.ManaAmount,
                Player.State.ManaRegenRate * Time.deltaTime);
        }

        public void DrainMana()
        {
            if (Player.State.IsWisdomActive)
            {
                return;
            }

            CurrentManaAmount = Mathf.MoveTowards(CurrentManaAmount, 0f, manaDrainRate * Time.deltaTime);
        }

        public void UpdateGUI()
        {
            manaFill.fillAmount = CurrentManaAmount / Player.State.ManaAmount;
            if (!Player.State.IsWisdomActive)
            {
                manaFill.color = s_manaFillEmpty.Slerp(ManaFillDefault, manaFill.fillAmount);
            }
        }
    }
}
