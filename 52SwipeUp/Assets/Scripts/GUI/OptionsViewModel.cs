// <copyright file="OptionsViewModel.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.UI;

    public class OptionsViewModel : MonoBehaviour
    {
        [SerializeField] private Text googleSignInStatus;

        public void RefreshGoogleSignInStatus()
        {
            OnSignedInChanged(Cloud.IsSignedIn);
        }

        public void SaveSettings()
        {
            PlayerSettings.Save();
        }

        private void Awake()
        {
            Cloud.OnSignedInChanged += OnSignedInChanged;
        }

        private void OnSignedInChanged(bool isSignedIn)
        {
            googleSignInStatus.text = isSignedIn
                ? "You are signed-in with Google."
                : "You are signed-out with Google.";
        }
    }
}
