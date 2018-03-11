// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointerFactory.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    /// <summary>
    ///  Instantiates and defines methods for ease of setting up targets for the UI-pointers.
    /// </summary>
    public class PointerFactory : MonoBehaviour
    {
        private GameObject prefabPointer;
        private bool isInitialized;

        public void Init()
        {
            if (isInitialized)
            {
                return;
            }

            prefabPointer = Resources.Load<GameObject>("Prefabs/Pointer");
            isInitialized = true;
        }

        public void CreateNewPointer(Transform mainTarget, Transform secondaryTarget, bool clampToScreen, bool deactivateAtTarget)
        {
            // 1. Instantiate new pointer from prefab
            var pointerGo = Instantiate(prefabPointer);
            pointerGo.transform.SetParent(transform, false);

            // 2. Get follow-script and set target and initialize
            var followScript = pointerGo.GetComponent<GUIElementFollowTarget>();
            followScript.Init(mainTarget, clampToScreen, deactivateAtTarget);

            // 3. Get texttodistance-script and set targets and initialize
            var textScript = pointerGo.GetComponentInChildren<DistanceToText>();
            textScript.Init(mainTarget, secondaryTarget);

            // 4. Get lookat2d-script and set target and initialize
            var lookScript = GetComponentInChildren<LookAtTarget2D>();
            lookScript.Init(mainTarget);
        }
    }
}
