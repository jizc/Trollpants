// <copyright file="AddToScoreOnDestroy.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Provides a simple way to increase the score of the player when an object is destroyed or despawned.
    /// </summary>
    public class AddToScoreOnDestroy : MonoBehaviour
    {
        [SerializeField] private int _addAmount = 100;
        [SerializeField] private bool _playSound;

        /// <summary>
        /// For objects that are despawned instead of destroyed.
        /// </summary>
        public void OnDespawned()
        {
            Increase();
        }

        private void OnDestroy()
        {
            Increase();
        }

        private void Increase()
        {
            if (ScoreSupervisor.Exists)
            {
                ScoreSupervisor.Instance.IncreaseScore(_addAmount, _playSound);
            }
        }
    }
}
