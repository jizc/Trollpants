// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PauseButton.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    public class PauseButton : CustomButton
    {
        protected override void OnButtonClicked()
        {
            Events.instance.Raise(new GamePaused());
            Events.instance.Raise(new EnterPauseMenu());
        }
    }
}
