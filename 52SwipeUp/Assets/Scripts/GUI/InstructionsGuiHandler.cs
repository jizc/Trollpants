// <copyright file="InstructionsGuiHandler.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using System.Collections.Generic;
    using Cards;
    using UnityEngine;
    using UnityEngine.UI;

    public class InstructionsGuiHandler : MonoBehaviour
    {
        [SerializeField] private SpriteBucket spriteBucket;
        [SerializeField] private GameObject topGroup;
        [SerializeField] private GameObject topArrow;
        [SerializeField] private GameObject leftGroup;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightGroup;
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private GameObject bottomGroup;
        [SerializeField] private GameObject bottomArrow;
        [SerializeField] private GameObject arrows;

        public void DisableArrows()
        {
            arrows.SetActive(false);
        }

        public void DisplayInstructions(List<Direction> dirsList)
        {
            DisableAllRuleSprites();

            for (var i = 0; i <= dirsList.Count - 1; i++)
            {
                switch (dirsList[i])
                {
                    case Direction.Up:
                        AddSpriteToGroup(topGroup, i);
                        topArrow.SetActive(true);
                        break;
                    case Direction.Down:
                        AddSpriteToGroup(bottomGroup, i);
                        bottomArrow.SetActive(true);
                        break;
                    case Direction.Left:
                        AddSpriteToGroup(leftGroup, i);
                        leftArrow.SetActive(true);
                        break;
                    case Direction.Right:
                        AddSpriteToGroup(rightGroup, i);
                        rightArrow.SetActive(true);
                        break;
                }
            }
        }

        public void EnableArrows()
        {
            arrows.SetActive(true);
        }

        private static void DisableChildren(GameObject parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void AddSpriteToGroup(GameObject group, int spriteIndex)
        {
            for (var i = 0; i < group.transform.childCount; i++)
            {
                var child = group.transform.GetChild(i).gameObject;
                if (!child.activeSelf)
                {
                    child.SetActive(true);
                    child.GetComponent<Image>().sprite = spriteBucket.RuleSprites[spriteIndex];
                    break;
                }
            }
        }

        private void DisableAllRuleSprites()
        {
            DisableChildren(topGroup);
            DisableChildren(leftGroup);
            DisableChildren(rightGroup);
            DisableChildren(bottomGroup);
            topArrow.SetActive(false);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            bottomArrow.SetActive(false);
        }
    }
}
