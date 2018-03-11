// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopMenuManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public abstract class ShopMenuManager : MonoBehaviour
    {
        [TextArea(1, 5)] public string tabExplanationText;

        protected virtual void OnEnable()
        {
            ShowExplanationIfNeeded();
        }

        protected abstract void ShowExplanationIfNeeded();

        public void ShowExplanation()
        {
            if (gameObject.activeSelf)
            {
                Events.instance.Raise(new ShowInfoBox(tabExplanationText));
            }
        }
    }
}
