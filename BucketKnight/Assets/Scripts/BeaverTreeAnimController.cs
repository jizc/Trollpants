// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeaverTreeAnimController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class BeaverTreeAnimController : MonoBehaviour
    {
        public float animDelay;
        private float _t;

        private void Start()
        {
            _t = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            _t += Time.deltaTime;
            if (_t >= animDelay && GetComponent<Animation>().IsPlaying("Idle"))
            {
                GetComponent<Animation>().Play("Falling2");
                GetComponent<Animation>().PlayQueued("Floating");
            }
        }
    }
}
