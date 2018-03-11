// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoinCounterImage.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public class CoinCounterImage : MonoBehaviour
    {
        private PlayerStats _playerStats;
        private Image _coinCounterImage;

        private void Awake()
        {
            _coinCounterImage = GetComponent<Image>();
            _coinCounterImage.fillAmount = 0;
        }

        private void Start()
        {
            _playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        }

        private void Update()
        {
            if (_playerStats.multiplierData[_playerStats.CurrentMultiplier - 1].coinsUntilNext != 0)
            {
                _coinCounterImage.fillAmount = (float)_playerStats.coinCountThisMultiplier /
                                               _playerStats.multiplierData[_playerStats.CurrentMultiplier - 1]
                                                   .coinsUntilNext;
            }
            else
            {
                _coinCounterImage.fillAmount = 1;
            }
        }
    }
}
