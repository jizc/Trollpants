// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomButton.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class CustomButton : MonoBehaviour
    {
        protected virtual void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClicked);
        }

        protected abstract void OnButtonClicked();
    }
}
