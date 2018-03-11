// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEndSupervisor.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public class GameEndSupervisor : MonoBehaviour
    {
        public AnimateData ScoreAnimateData;
        public AnimateData TimeAnimateData;

        private bool isInitialized;

        public void Init()
        {
            if (isInitialized)
            {
                return;
            }

            gameObject.SetActive(false);
            isInitialized = true;
        }

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            if (!isInitialized)
            {
                return;
            }

            // Deactivate all other Canvases
            foreach (var canvas in FindObjectsOfType<Canvas>())
            {
                if (canvas == GetComponent<Canvas>())
                {
                    continue;
                }

                canvas.gameObject.SetActive(false);
            }

            if (GameMode.Instance.CurrentMode == GameMode.Mode.Standard)
            {
                ScoreAnimateData.PlayAnimation();
                TimeAnimateData.gameObject.SetActive(false);
            }
            else if (GameMode.Instance.CurrentMode == GameMode.Mode.Time)
            {
                ScoreAnimateData.PlayAnimation();
                TimeAnimateData.PlayAnimation();
            }

            // Tell the data tracker that the session has ended
            // Updates the stats
            TrackedData.Instance.SessionEnd();
        }
    }
}
