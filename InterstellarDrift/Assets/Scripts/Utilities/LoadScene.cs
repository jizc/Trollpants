// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadScene.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LoadScene : MonoBehaviour
    {
        public void LoadSceneNumbered(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }

        public void LoadSceneNamed(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
