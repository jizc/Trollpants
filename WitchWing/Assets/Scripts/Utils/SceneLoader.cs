// <copyright file="SceneLoader.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Utils
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLoader : MonoBehaviour
    {
        private static void OnCloudLoadComplete(bool success)
        {
            SceneManager.LoadSceneAsync(1);
        }

        private void Start()
        {
            Cloud.OnCloudLoadComplete += OnCloudLoadComplete;
            Cloud.Initialize();
        }
    }
}
