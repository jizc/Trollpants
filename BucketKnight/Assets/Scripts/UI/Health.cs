// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Health.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class Health : MonoBehaviour
    {
        private int _currentlyDisplayedHealth;
        private int _currentlyDisplayedTempHealth;

        public HeartAnimation[] hearts;
        public GameObject[] tempHearts;

        private void OnEnable()
        {
            Events.instance.AddListener<PlayerHealthChanged>(OnHealthChanged);

            for (var i = 3; i > _currentlyDisplayedHealth; i--)
            {
                hearts[i - 1].Hide();
            }
        }

        private void Awake()
        {
            ResetHearts();
        }

        private void ResetHearts()
        {
            foreach (var heart in hearts)
            {
                heart.Reset();
                heart.gameObject.SetActive(true);
            }
            foreach (var tempHeart in tempHearts)
            {
                tempHeart.SetActive(false);
            }
            _currentlyDisplayedHealth = 3;
            _currentlyDisplayedTempHealth = 0;
        }

        private void OnHealthChanged(PlayerHealthChanged playerHealthChangedEvent)
        {
            if (_currentlyDisplayedHealth > playerHealthChangedEvent.health)
            {
                for (var i = _currentlyDisplayedHealth - 1; i >= playerHealthChangedEvent.health; i--)
                {
                    hearts[playerHealthChangedEvent.health].PlayAnimation();
                }
            }
            else if (_currentlyDisplayedHealth < playerHealthChangedEvent.health)
            {
                for (var i = Mathf.Max(_currentlyDisplayedHealth - 1, 0); i < playerHealthChangedEvent.health; i++)
                {
                    hearts[i].Reset();
                }
            }
            _currentlyDisplayedHealth = playerHealthChangedEvent.health;

            if (_currentlyDisplayedTempHealth > playerHealthChangedEvent.tempHealth)
            {
                for (var i = _currentlyDisplayedTempHealth - 1; i >= playerHealthChangedEvent.tempHealth; i--)
                {
                    tempHearts[i].SetActive(false);
                }
            }
            else if (_currentlyDisplayedTempHealth < playerHealthChangedEvent.tempHealth)
            {
                for (var i = Mathf.Max(_currentlyDisplayedTempHealth - 1, 0);
                    i < playerHealthChangedEvent.tempHealth;
                    i++)
                {
                    tempHearts[i].SetActive(true);
                }
            }
            _currentlyDisplayedTempHealth = playerHealthChangedEvent.tempHealth;

        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<PlayerHealthChanged>(OnHealthChanged);
        }
    }
}
