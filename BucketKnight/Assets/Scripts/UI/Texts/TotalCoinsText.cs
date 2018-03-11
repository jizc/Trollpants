// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TotalCoinsText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public class TotalCoinsText : MonoBehaviour
    {
        private Text _coinText;

        private void Awake()
        {
            _coinText = gameObject.GetComponent<Text>();
        }

        private void OnEnable()
        {
            _coinText.text = SavedData.TotalCoins.ToString();

            Events.instance.AddListener<TotalCoinsChanged>(OnTotalCoinsChanged);
        }

        #region Event handlers

        private void OnTotalCoinsChanged(TotalCoinsChanged totalCoinsChangedEvent)
        {
            _coinText.text = totalCoinsChangedEvent.totalCoins.ToString();
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<TotalCoinsChanged>(OnTotalCoinsChanged);
        }
    }
}
