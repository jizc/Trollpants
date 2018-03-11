// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimedMode.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using UnityEngine.UI;

    public class TimedMode : MonoBehaviour
    {
        public Image FillableImage;
        public float CurrentTime;
        public float TimeLimit = 30f;
        public float TotalSecondsAlive;
        public float CurrentTimePercentage;

        private GameObject player;

        public void ResetTime()
        {
            CurrentTime = TimeLimit;
            TotalSecondsAlive = 0;
        }

        public void RefreshDuration()
        {
            CurrentTime = TimeLimit;
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            CurrentTime = TimeLimit;
        }

        private void Update()
        {
            if (!FillableImage)
            {
                Debug.LogWarning(name + " is missing fillable Image-component on child-object.");
                return;
            }

            if (FillableImage.type != Image.Type.Filled)
            {
                return;
            }

            if (CurrentTime > 0)
            {
                TotalSecondsAlive += Time.deltaTime;

            }

            TrackedData.Instance.SessionData.MillisecondsSurvived = (int)(TotalSecondsAlive * 1000);

            CurrentTime -= Time.deltaTime;

            CurrentTimePercentage = CurrentTime / TimeLimit;
            FillableImage.fillAmount = CurrentTimePercentage;

            if (CurrentTime <= 0)
            {
                Destroy(player);
            }
        }
    }
}
