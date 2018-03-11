// <copyright file="ScoreEventHook.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Helper methods for events that need hooks to the singleton-instance.
    /// </summary>
    public class ScoreEventHook : MonoBehaviour
    {
        public void IncreaseScore(int amount)
        {
            if (ScoreSupervisor.Exists)
            {
                ScoreSupervisor.Instance.IncreaseScore(amount, false);
            }
        }

        public void DecreaseScore(int amount)
        {
            if (ScoreSupervisor.Exists)
            {
                ScoreSupervisor.Instance.DecreaseScore(amount);
            }
        }
    }
}
