// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TutorialTouchButton.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class TutorialTouchButton : CustomButton
    {
        protected override void OnButtonClicked()
        {
            Events.instance.Raise(new TiltToggled(false));
            GameObject.Find("TutorialMenu").GetComponent<MainTutorialManager>().GoToPage("TouchControlPage");
        }
    }
}
