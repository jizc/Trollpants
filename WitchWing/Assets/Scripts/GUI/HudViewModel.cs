// <copyright file="HudViewModel.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using CloudOnce;
    using Player;
    using UnityEngine;
    using UnityEngine.UI;

    public class HudViewModel : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private Text coinsText;
        [SerializeField] private Text timeBonusText;
        [SerializeField] private Text distanceRecordText;
        [SerializeField] private Text currentDistanceText;
        [SerializeField] private Text wisdomPotionsText;
        [SerializeField] private Text midasPotionsText;

        public void Init()
        {
            currentDistanceText.text = "0m";
            coinsText.text = Player.State.Coins.ToString();
            distanceRecordText.text = CloudVariables.DistanceRecord.ToString();
            wisdomPotionsText.text = CloudVariables.WisdomPotions.ToString();
            midasPotionsText.text = CloudVariables.MidasPotions.ToString();

            Player.State.CoinsChanged += OnCoinsChanged;
            Player.State.TimeBonusChanged += OnTimeBonusChanged;

            Player.State.DistanceRecordChanged += OnDistanceRecordChanged;
            Player.State.DistanceTraveledChanged += OnDistanceTraveledChanged;

            Player.State.WisdomPotionsChanged += OnWisdomPotionsChanged;
            Player.State.MidasPotionsChanged += OnMidasPotionsChanged;
        }

        private void OnCoinsChanged(int value)
        {
            coinsText.text = value.ToString();
        }

        private void OnTimeBonusChanged(int value)
        {
            timeBonusText.text = value.ToString();
        }

        private void OnDistanceRecordChanged(int value)
        {
            distanceRecordText.text = value.ToString();
        }

        private void OnDistanceTraveledChanged(int value)
        {
            currentDistanceText.text = value + "m";
        }

        private void OnWisdomPotionsChanged(int value)
        {
            wisdomPotionsText.text = value.ToString();
        }

        private void OnMidasPotionsChanged(int value)
        {
            midasPotionsText.text = value.ToString();
        }
    }
}
