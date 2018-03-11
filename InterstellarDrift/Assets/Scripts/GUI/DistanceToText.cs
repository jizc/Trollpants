// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistanceToText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///  Gets the world-coordinate distance between two transforms and outputs it to the Text-component.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class DistanceToText : MonoBehaviour
    {
        public Transform TargetA;
        public Transform TargetB;

        private Text cachedText;

        public void Init(Transform targetA, Transform targetB)
        {
            cachedText = GetComponent<Text>();
            TargetA = targetA;
            TargetB = targetB;
        }

        private void Awake()
        {
            Init(null, null);
        }

        private void Update()
        {
            if (TargetA == null || TargetB == null)
            {
#if DEBUG
                Debug.LogWarning(name + " lacks a target transform.");
#endif
                return;
            }

            cachedText.text = string.Empty + (int)Vector3.Distance(TargetA.position, TargetB.position);
        }
    }
}
