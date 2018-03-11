// <copyright file="GameServicesToggle.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Toggle))]
    public class GameServicesToggle : MonoBehaviour
    {
        [SerializeField] private GameObject leaderboardsButton;
        [SerializeField] private GameObject achievementsButton;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            leaderboardsButton.SetActive(isOn);
            achievementsButton.SetActive(isOn);
        }
    }
}
