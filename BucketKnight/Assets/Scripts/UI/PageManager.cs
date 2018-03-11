// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class PageManager : MonoBehaviour
    {
        public GameObject startingPage;
        protected GameObject currentPage;

        public void Awake()
        {
            foreach (Transform child in transform.Find("Pages"))
            {
                child.gameObject.SetActive(false);
            }
        }

        public void OnEnable()
        {
            if (currentPage != null)
            {
                currentPage.SetActive(false);
            }

            startingPage.SetActive(true);
            currentPage = startingPage;
        }

        public void GoToPage(GameObject pageToGoTo)
        {
            currentPage.SetActive(false);
            pageToGoTo.SetActive(true);
            currentPage = pageToGoTo;
        }

        public void GoToPage(string pageToGoTo)
        {
            foreach (Transform child in transform.Find("Pages"))
            {
                if (child.name == pageToGoTo)
                {
                    GoToPage(child.gameObject);
                    break;
                }
            }
        }
    }
}
