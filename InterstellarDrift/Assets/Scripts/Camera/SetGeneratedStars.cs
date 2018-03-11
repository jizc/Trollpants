// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetGeneratedStars.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(MeshRenderer))]
    public class SetGeneratedStars : MonoBehaviour
    {
        [SerializeField] private BackgroundType _backgroundType;

        private enum BackgroundType
        {
            Front,
            Back
        }

        private void Awake()
        {
            var meshRenderer = GetComponent<MeshRenderer>();

            switch (_backgroundType)
            {
                case BackgroundType.Front:
                    meshRenderer.material.mainTexture = TrackedData.Instance.SessionData.FrontStarsTexture2D;
                    break;
                case BackgroundType.Back:
                    meshRenderer.material.mainTexture = TrackedData.Instance.SessionData.BackStarsTexture2D;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
