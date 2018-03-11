// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomSpritesheet.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class CustomSpritesheet : MonoBehaviour
    {
        public int numberOfMissingSpritesInLastRow;

        protected List<RectTransform> spritesheetRectTransforms = new List<RectTransform>();

        protected int spriteWidth;
        protected int spriteHeight;
        protected int spritesPerRow;
        protected int numberOfColumns;
        protected int currentSpriteNum;

        public void Awake()
        {
            var compList = GetComponentsInChildren<RectTransform>();

            // første element er fra parent, så det hoppes over
            for (var i = 1; i < compList.Length; i++)
            {
                spritesheetRectTransforms.Add(compList[i]);
            }

            var thisRect = GetComponent<RectTransform>();

            spriteWidth = (int)thisRect.rect.width;
            spriteHeight = (int)thisRect.rect.height;
            spritesPerRow = (int)spritesheetRectTransforms[0].rect.width / spriteWidth;
            numberOfColumns = (int)spritesheetRectTransforms[0].rect.height / spriteHeight;

            currentSpriteNum = 0;
            GoToSprite(currentSpriteNum);
        }

        protected void GoToNextSprite()
        {
            if (currentSpriteNum > (spritesPerRow * numberOfColumns) - numberOfMissingSpritesInLastRow)
            {
                currentSpriteNum = 1;
            }

            GoToSprite(currentSpriteNum);
        }

        protected void GoToPreviousSprite()
        {
            if (currentSpriteNum < 1)
            {
                currentSpriteNum = (spritesPerRow * numberOfColumns) - numberOfMissingSpritesInLastRow;
            }

            GoToSprite(currentSpriteNum);
        }

        /// <summary>
        /// Changes position of spritesheet so that desired frame is shown. Will not do anything on too high/low input number
        /// </summary>
        /// <param name="spriteNum">Zero based index number</param>
        protected void GoToSprite(int spriteNum)
        {
            if (spriteNum < 0 || spriteNum > spritesPerRow * numberOfColumns)
            {
                return;
            }

            var rowCount = 1;
            var columnCount = 1;
            while (rowCount * spritesPerRow <= spriteNum)
            {
                rowCount++;
            }

            columnCount += spritesPerRow - ((rowCount * spritesPerRow) - spriteNum);

            foreach (var rectTransform in spritesheetRectTransforms)
            {
                rectTransform.anchoredPosition = new Vector2(-1 * spriteWidth * (columnCount - 1), spriteHeight * (rowCount - 1));
            }

            currentSpriteNum = spriteNum;
        }
    }
}
