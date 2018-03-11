// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Goal.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Handles trigger-detection.
    /// </summary>
    public class Goal : MonoBehaviour
    {
        [SerializeField] private Sound _triggerSound = Sound.Checkpoint;

        public delegate void OnTriggerEnterEvent(GameObject sender, GameObject triggeringObject);

        public event OnTriggerEnterEvent OnTouchedShip;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ship"))
            {
                return;
            }

            if (OnTouchedShip != null)
            {
                if (SoundManager.Instance)
                {
                    SoundManager.Instance.PlaySound(_triggerSound);
                }

                // Increase score for reaching the goal
                if (ScoreSupervisor.Instance)
                {
                    ScoreSupervisor.Instance.IncreaseScore(300, false);
                }

                OnTouchedShip(gameObject, other.gameObject);
            }
        }
    }
}
