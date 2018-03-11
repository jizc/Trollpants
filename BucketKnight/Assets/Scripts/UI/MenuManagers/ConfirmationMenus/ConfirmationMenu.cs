// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfirmationMenu.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class ConfirmationMenu : MonoBehaviour
    {
        public Text infoText;

        public virtual void SetText(string text)
        {
            infoText.text = text;
        }
    }
}
