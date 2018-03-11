// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundColorTweener.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///  Tweens the alpha on the background image, to simulate shimmering water
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class BackgroundColorTweener : MonoBehaviour
    {
        #region Fields & properties

        private Image _image;

        #endregion /Fields & properties

        #region Unity methods

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            OnTweenComplete();
            /*DOTween.ToAlpha(() => _image.color, x => _image.color = x, 0.8f, 3f)
                .SetEase(Ease.InOutBounce)
                .SetLoops(-1, LoopType.Yoyo);*/
        }

        #endregion /Unity methods

        private void OnTweenComplete()
        {
            var strength = Random.Range(0.8f, 1.1f);
            var duration = Random.Range(1f, 3f);
            DOTween.ToAlpha(() => _image.color, x => _image.color = x, strength, duration)
                .SetEase(Ease.InOutBounce)
                .SetLoops(1, LoopType.Yoyo)
                .OnComplete(OnTweenComplete);
        }
    }
}
