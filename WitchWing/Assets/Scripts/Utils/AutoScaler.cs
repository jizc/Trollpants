// <copyright file="AutoScaler.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Utils
{
    using DG.Tweening;
    using UnityEngine;

    public class AutoScaler : MonoBehaviour
    {
        [SerializeField] private Vector3 target = new Vector3(1.03f, 1.03f, 1f);
        [SerializeField] private float duration = 10f;
        [SerializeField] private Ease easing = Ease.InOutQuad;

        private Tweener tweener;

        private void OnEnable()
        {
            if (tweener == null)
            {
                tweener = transform.DOScale(target, duration).SetLoops(-1, LoopType.Yoyo).SetEase(easing);
            }

            tweener.Play();
        }

        private void OnDisable()
        {
            tweener.Pause();
        }
    }
}
