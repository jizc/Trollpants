// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToggleMute.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.UI;

    public class ToggleMute : MonoBehaviour
    {
        private static void ToggleMuteOnEvent(bool soundOn)
        {
            if (SoundManager.Instance)
            {
                SoundManager.Instance.MuteSound(!soundOn);
            }
        }

        private void Awake()
        {
            var toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(ToggleMuteOnEvent);

            if (CloudVariables.IsMuted)
            {
                toggle.isOn = false;
            }
        }
    }
}
