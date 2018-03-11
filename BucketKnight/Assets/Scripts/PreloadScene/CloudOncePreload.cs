// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudOncePreload.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CloudOncePreload : MonoBehaviour
    {
        private static void OnCloudLoadComplete(bool startupSucceeded)
        {
            Cloud.OnCloudLoadComplete -= OnCloudLoadComplete;
            SceneManager.LoadScene(1);
        }

        private void Awake()
        {
            Cloud.OnCloudLoadComplete += OnCloudLoadComplete;
            Cloud.Initialize();
        }
    }
}
