// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiplierFillScript.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public struct MultiplierFillData
    {
        public Texture texture;
        public float minMaskOffset;
        public float maxMaskOffset;
    }

    public class MultiplierFillScript : MonoBehaviour
    {
        private const string maskName = "_Mask";

        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private Image image;

        public MultiplierFillData[] multiplierFillData;

        private Vector2 offsetPos = Vector2.zero;

        private void OnEnable()
        {
            Events.instance.AddListener<MultiplierChanged>(OnMultiplierChanged);
        }

        private void Awake()
        {
            image.material.SetTexture("_MainTex", multiplierFillData[0].texture);
        }

        private void LateUpdate()
        {
            offsetPos.x += Time.deltaTime * 0.4f;
            if (offsetPos.x >= 1)
            {
                offsetPos.x = 0;
            }

            offsetPos.y = CalculateYOffset();

            image.material.SetTextureOffset(maskName, offsetPos);
        }

        private float CalculateYOffset()
        {
            // maps from one number range to another
            // from: http://forum.unity3d.com/threads/re-map-a-number-from-one-range-to-another.119437/
            return multiplierFillData[_playerStats.CurrentMultiplier - 1].minMaskOffset
                   + ((_playerStats.multiplierTimer - _playerStats.multiplierData[_playerStats.CurrentMultiplier - 1].timeLimit)
                   * (multiplierFillData[_playerStats.CurrentMultiplier - 1].maxMaskOffset - multiplierFillData[_playerStats.CurrentMultiplier - 1].minMaskOffset)
                   / -_playerStats.multiplierData[_playerStats.CurrentMultiplier - 1].timeLimit);
        }

        private void OnMultiplierChanged(MultiplierChanged multiplierChangedEvent)
        {
            if (_playerStats == null)
            {
                Debug.LogError("_playerStats is null");
                return;
            }

            var index = _playerStats.CurrentMultiplier - 1;
            image.material.SetTexture("_MainTex", multiplierFillData[index].texture);

            // Force material to update
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<MultiplierChanged>(OnMultiplierChanged);
        }
    }
}
