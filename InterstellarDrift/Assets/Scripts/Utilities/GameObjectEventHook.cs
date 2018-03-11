// <copyright file="GameObjectEventHook.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using UnityEngine;

    public class GameObjectEventHook : MonoBehaviour
    {
        public GameObject Target;

        public void Destroy()
        {
            if (Target)
            {
                Destroy(Target);
            }
        }
    }
}
