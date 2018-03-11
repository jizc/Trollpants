// <copyright file="OptionsViewModel.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using CloudOnce;
    using Data;
    using UnityEngine;
    using UnityEngine.UI;

    public class OptionsViewModel : MonoBehaviour
    {
        [SerializeField] private Text googleSignInStatus;
        [SerializeField] private Toggle invertControlsToggle;

        public void ResetTutorials()
        {
            TutorialCoordinator.ResetTutorials();
        }

        private static void OnInvertControlsChanged(bool isEnabled)
        {
            PlayerSettings.IsYAxisInverted = isEnabled;
            PlayerSettings.Save();
        }

        private void Awake()
        {
            Cloud.OnSignedInChanged += OnSignedInChanged;
            invertControlsToggle.onValueChanged.AddListener(OnInvertControlsChanged);
        }

        private void Start()
        {
            invertControlsToggle.isOn = PlayerSettings.IsYAxisInverted;
        }

        private void OnEnable()
        {
            OnSignedInChanged(Cloud.IsSignedIn);
        }

        private void OnSignedInChanged(bool isSignedIn)
        {
            googleSignInStatus.text = isSignedIn
                ? "You are signed-in with Google."
                : "You are signed-out with Google.";
        }
    }
}
