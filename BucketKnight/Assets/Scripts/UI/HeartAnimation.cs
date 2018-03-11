// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeartAnimation.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class HeartAnimation : MonoBehaviour
    {
        public Animator animator;

        public void PlayAnimation()
        {
            animator.CrossFade("HeartAnimation", 0f);
        }

        public void Reset()
        {
            animator.CrossFade("HeartIdle 0", 0f);
        }

        public void Hide()
        {
            animator.CrossFade("HeartGone", 0f);
        }
    }
}
