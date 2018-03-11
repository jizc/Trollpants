// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FishWaterSplash.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class FishWaterSplash : ParticleBehaviour
    {
        private Vector3 prevPos;
        private Vector3 origPos;

        public bool freezePos;

        private void Awake()
        {
            origPos = gameObject.transform.localPosition;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            gameObject.transform.localPosition = origPos;
            freezePos = false;
        }

        private void LateUpdate()
        {
            if (freezePos)
            {
                gameObject.transform.position = prevPos;
            }
            prevPos = gameObject.transform.position;
        }

        public void ActivateParticles()
        {
            freezePos = true;
            PlayParticles();
        }
    }
}
