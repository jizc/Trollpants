// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameOverCoinText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public class GameOverCoinText : MonoBehaviour
    {
        private float countDownDelay;
        private int coins;
        private int totalCoins;
        private int transferRate;

        private Text coinText;
        private Text totalCoinsText;

        private void OnEnable()
        {
            transferRate = 1;
            coinText = transform.Find("CoinsCollectedText").GetComponent<Text>();
            totalCoinsText = transform.Find("TotalCoinsText").GetComponent<Text>();
            coins = GameObject.Find("Player").GetComponent<PlayerStats>().Coins;
            GameObject.Find("Player").GetComponent<PlayerStats>().Coins = 0;

            transform.Find("CoinsCollectedText").GetComponent<Text>().text = coins.ToString();

            if (coins == 0)
            {
                totalCoins = SavedData.TotalCoins;
                transform.Find("TotalCoinsText").GetComponent<Text>().text = totalCoins.ToString();
                return;
            }

            countDownDelay = 3;
            totalCoins = 0;
            totalCoins = SavedData.TotalCoins - coins;
            transform.Find("CoinsCollectedText").GetComponent<Text>().text = coins.ToString();
            transform.Find("TotalCoinsText").GetComponent<Text>().text = totalCoins.ToString();
        }

        private void Update()
        {
            if (coins == 0)
            {
                return;
            }

            if (countDownDelay >= 0)
            {
                countDownDelay -= 0.05f;
                return;
            }

            if (coins > 0)
            {
                if (transferRate > coins)
                {
                    transferRate = coins;
                }

                coins -= transferRate;
                totalCoins += transferRate;

                coinText.text = coins.ToString();
                totalCoinsText.text = totalCoins.ToString();
            }
        }
    }
}
