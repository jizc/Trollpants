// <copyright file="ButtonClickSoundTrigger.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.Audio
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class ButtonClickSoundTrigger : MonoBehaviour
    {
        private Button button;

        private static void OnClick()
        {
            AudioClipPlayer.PlayButton();
        }

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
    }
}
