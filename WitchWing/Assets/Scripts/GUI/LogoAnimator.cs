// <copyright file="LogoAnimator.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing
{
    using DG.Tweening;
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class LogoAnimator : MonoBehaviour
    {
        [SerializeField] private float distance = 40f;
        [SerializeField] [Range(0.3f, 5f)] private float duration = 2f;
        [SerializeField] private Ease easing = Ease.InOutQuad;

        private RectTransform rectTransform;
        private Tweener tweener;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            enabled = false;
        }

        private void OnEnable()
        {
            if (tweener == null)
            {
                tweener = rectTransform.DOAnchorPosY(-distance, duration).SetRelative(true).SetLoops(-1, LoopType.Yoyo).SetEase(easing);
            }

            tweener.Play();
        }

        private void OnDisable()
        {
            tweener.Pause();
        }
    }
}
