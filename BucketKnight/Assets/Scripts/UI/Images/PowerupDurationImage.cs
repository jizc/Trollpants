// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerupDurationImage.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class PowerupDurationImage : MonoBehaviour
    {
        private float _duration;
        public float fadeTime = 1f;
        private float _powerupElapsedTime;
        private float _faderElapsedTime;

        private Image _image;
        private Color _imageColor;
        private PowerupAssetList _powerupAssetList;

        private void OnEnable()
        {
            if (_powerupElapsedTime > 0)
            {
                StartCoroutine("PowerupTimer", _duration);
            }
            else if (_faderElapsedTime > 0)
            {
                StartCoroutine("FadeTimer", fadeTime);
            }

            Events.instance.AddListener<PowerupPickup>(OnPowerupPickedUp);
            Events.instance.AddListener<PowerupEnded>(OnPowerupEnd);
        }

        private void Start()
        {
            _image = transform.GetComponent<Image>();
            _powerupAssetList = GameObject.Find("PowerupAssetManager").GetComponent<PowerupAssetList>();
            _imageColor = _image.color;
            Deactivate();
        }

        private IEnumerator PowerupTimer(float time)
        {
            while (_powerupElapsedTime < time)
            {
                _image.fillAmount = Mathf.Lerp(1, 0, _powerupElapsedTime / time);
                _powerupElapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator FadeTimer(float time)
        {
            float alphaStart = _imageColor.a;
            while (_faderElapsedTime < time)
            {
                _imageColor.a = Mathf.Lerp(alphaStart, 0, _faderElapsedTime / time);
                _image.color = _imageColor;
                _faderElapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Events.instance.Raise(new PowerupEnded());
                //<-- powerup ended kalles her, ikke i playerstats, når det er en powerup med duration 0
        }

        private void Deactivate()
        {
            StopCoroutine("FadeTimer");
            StopCoroutine("PowerupTimer");
            _powerupElapsedTime = 0;
            _faderElapsedTime = 0;
            _imageColor.a = 0;
            _image.color = _imageColor;
            _image.fillAmount = 0;
        }

        private void Activate()
        {
            ResetAlpha();
            _image.fillAmount = 1;
        }

        private void ResetAlpha()
        {
            _imageColor.a = 1;
            _image.color = _imageColor;
        }

        #region Event handlers

        private void OnPowerupPickedUp(PowerupPickup powerupPickup)
        {
            Activate();
            _powerupElapsedTime = 0;
            _faderElapsedTime = 0;
            var activePowerup = PowerupManager.GetPowerup(powerupPickup.powerup);
            _duration = activePowerup.Duration;

            if (_duration <= 0)
            {
                StopCoroutine("PowerupTimer");
                StartCoroutine("FadeTimer", fadeTime);
            }
            else
            {
                StopCoroutine("FadeTimer");
                StartCoroutine("PowerupTimer", _duration);
            }

            _image.sprite = _powerupAssetList.GetSprite(activePowerup.powerupType);
        }

        private void OnPowerupEnd(PowerupEnded e)
        {
            if (!e.endedSoAnotherCouldBegin)
            {
                Deactivate();
            }
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<PowerupPickup>(OnPowerupPickedUp);
            Events.instance.RemoveListener<PowerupEnded>(OnPowerupEnd);
        }
    }
}
