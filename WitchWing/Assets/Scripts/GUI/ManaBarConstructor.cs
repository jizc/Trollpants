// <copyright file="ManaBarConstructor.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System;
    using System.Collections.Generic;
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.UI;

    public class ManaBarConstructor : MonoBehaviour
    {
        [Header("Frame")]
        [SerializeField] private Transform frameContainer;
        [SerializeField] private GameObject framePrefab;

        [Header("Sprites")]
        [SerializeField] private Sprite soloSegment;
        [SerializeField] private Sprite bottomSegment;
        [SerializeField] private Sprite middleSegment;
        [SerializeField] private Sprite topSegment;

        [Header("Transforms")]
        [SerializeField] private RectTransform manaFillRectTransform;
        [SerializeField] private RectTransform manaBackgroundRectTransform;

        public void UpdateManaBarSize()
        {
            // Make sure there are the right amount of frame pieces
            var framePieces = InstantiateFramePieces(CloudVariables.UnlockedManaAmountLevel);

            // Go through the list and set correct name, position and sprite
            for (var index = 0; index < CloudVariables.UnlockedManaAmountLevel; index++)
            {
                framePieces[index].name = "FramePiece" + string.Format("{0:00}", index + 1);
                var rectTransform = framePieces[index].GetComponent<RectTransform>();
                rectTransform.SetSiblingIndex(index + 2);
                if (index > 0)
                {
                    rectTransform.anchoredPosition = new Vector2(0, 35f * (index + 1));
                }

                if (CloudVariables.UnlockedManaAmountLevel == 1)
                {
                    framePieces[index].GetComponent<Image>().sprite = soloSegment;
                }
                else
                {
                    if (index == 0)
                    {
                        framePieces[index].GetComponent<Image>().sprite = bottomSegment;
                    }
                    else if (index > 0 && index != CloudVariables.UnlockedManaAmountLevel - 1)
                    {
                        framePieces[index].GetComponent<Image>().sprite = middleSegment;
                    }
                    else
                    {
                        framePieces[index].GetComponent<Image>().sprite = topSegment;
                    }
                }
            }

            if (manaBackgroundRectTransform == null || manaFillRectTransform == null)
            {
                return;
            }

            manaBackgroundRectTransform.sizeDelta = new Vector2(
                manaBackgroundRectTransform.sizeDelta.x, CloudVariables.UnlockedManaAmountLevel * 35f);
            manaFillRectTransform.sizeDelta = new Vector2(
                manaFillRectTransform.sizeDelta.x, CloudVariables.UnlockedManaAmountLevel * 35f);
        }

        private List<GameObject> InstantiateFramePieces(int desiredFramePieces)
        {
            // Find existing frame pieces
            var returnList = new List<GameObject>();
            for (var i = 0; i < frameContainer.childCount; i++)
            {
                returnList.Add(frameContainer.GetChild(i).gameObject);
            }

            // If already right amount of frame pieces, return
            if (returnList.Count == desiredFramePieces)
            {
                return returnList;
            }

            if (returnList.Count < desiredFramePieces)
            {
                // We need more frame pieces
                var neededFramePieces = desiredFramePieces - returnList.Count;
                for (var i = 0; i < neededFramePieces; i++)
                {
                    var listItem = Instantiate(framePrefab, frameContainer);
                    var rectTransform = listItem.GetComponent<RectTransform>();
                    rectTransform.localScale = Vector3.one;
                    returnList.Add(listItem);
                }
            }
            else
            {
                // We have too many frame pieces
                var beforeReturnListCount = returnList.Count;
                var removeCount = returnList.Count - desiredFramePieces;

                try
                {
                    var removeList = new List<GameObject>();
                    for (var i = 0; i < removeCount; i++)
                    {
                        removeList.Add(returnList[desiredFramePieces + i]);
                    }

                    foreach (var item in removeList)
                    {
                        returnList.Remove(item);
                        Destroy(item);
                    }
                }
                catch (ArgumentOutOfRangeException exception)
                {
                    Debug.LogError("Index: " + (desiredFramePieces + 1) + "\nDesiredListCount: " + desiredFramePieces +
                                   "\nPanelsToRemoveCount: " + removeCount + "\nReturnListCount: " + returnList.Count +
                                   " (" + beforeReturnListCount + ")");
                    Debug.LogException(exception);
                    throw;
                }
            }

            return returnList;
        }
    }
}
