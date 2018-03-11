// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomToggle.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class CustomToggle : MonoBehaviour
    {
        protected Toggle toggleController;

        public void OnEnable()
        {
            toggleController = GetComponent<Toggle>();
            toggleController.isOn = GetToggleValue();
            toggleController.onValueChanged.AddListener(OnToggled);
        }

        protected abstract bool GetToggleValue();
        protected abstract void OnToggled(bool toggledOn);
    }
}
