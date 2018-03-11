// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayGameButton.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    public class PlayGameButton : CustomButton
    {
        protected override void OnButtonClicked()
        {
            if (!SavedData.TutorialHasBeenLaunchedOnce)
            {
                Events.instance.Raise(new EnterTutorialMenu(false));
            }
            else
            {
                Events.instance.Raise(new GoInGame());
            }
        }
    }
}
