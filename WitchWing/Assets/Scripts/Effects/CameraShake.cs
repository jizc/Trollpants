// <copyright file="CameraShake.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Effects
{
    using System.Collections;
    using UnityEngine;

    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float duration = 0.4f;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float magnitude = 0.5f;

        private IEnumerator coroutine;

        public void Shake()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            coroutine = PlayShake();
            StartCoroutine(coroutine);
        }

        private IEnumerator PlayShake()
        {
            var elapsed = 0f;

            var originalCamPos = transform.localPosition;
            var randomStart = Random.Range(-1000.0f, 1000.0f);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                var percentComplete = elapsed / duration;

                // We want to reduce the shake from full power to 0 starting half way through
                var damper = 1.0f - Mathf.Clamp((2.0f * percentComplete) - 1.0f, 0.0f, 1.0f);

                // Calculate the noise parameter starting randomly and going as fast as Speed allows
                var alpha = randomStart + (speed * percentComplete);

                // map noise to [-1, 1]
                var x = (Mathf.PerlinNoise(alpha, 0f) * 2f) - 1f;
                var y = (Mathf.PerlinNoise(0.0f, alpha) * 2f) - 1.0f;

                x *= magnitude * damper;
                y *= magnitude * damper;

                transform.localPosition = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

                yield return null;
            }

            transform.localPosition = originalCamPos;
        }
    }
}
