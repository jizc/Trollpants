// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerAnimController.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class PlayerAnimController : MonoBehaviour
    {
        public string paddleLeftName,
            paddleRightName,
            switchToRightName,
            switchToLeftName,
            idleLeftName,
            idleRightName,
            damageLBackName,
            damageLForwardName,
            damageRBackName,
            damageRForwardName;

        public float animFadeTime;

        private bool _paddlingLeft;

        private void OnEnable()
        {
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
        }

        private void Start()
        {
            Restart();
        }

        private void Restart()
        {
            GetComponent<Animation>().Play(idleLeftName);
            _paddlingLeft = true;
        }

        public void PaddleLeft()
        {
            if (GetComponent<Animation>().IsPlaying(damageLBackName) ||
                GetComponent<Animation>().IsPlaying(damageLBackName))
            {
                return;
            }
            if (!_paddlingLeft)
            {
                GetComponent<Animation>().CrossFade(switchToLeftName, animFadeTime);
                GetComponent<Animation>().CrossFadeQueued(paddleLeftName, animFadeTime);
                _paddlingLeft = true;
            }
            else if (_paddlingLeft && !GetComponent<Animation>().IsPlaying(paddleLeftName))
            {
                GetComponent<Animation>().CrossFade(paddleLeftName, animFadeTime);
                _paddlingLeft = true;
            }
        }

        public void PaddleRight()
        {
            if (GetComponent<Animation>().IsPlaying(damageLBackName) ||
                GetComponent<Animation>().IsPlaying(damageLBackName))
            {
                return;
            }
            if (_paddlingLeft)
            {
                GetComponent<Animation>().CrossFade(switchToRightName, animFadeTime);
                GetComponent<Animation>().CrossFadeQueued(paddleRightName, animFadeTime);
                _paddlingLeft = false;
            }
            else if (!_paddlingLeft && !GetComponent<Animation>().IsPlaying(paddleRightName))
            {
                GetComponent<Animation>().CrossFade(paddleRightName, animFadeTime);
                _paddlingLeft = false;
            }
        }

        public void Idle()
        {
            if (!_paddlingLeft && !GetComponent<Animation>().IsPlaying(idleRightName))
            {
                GetComponent<Animation>().CrossFade(idleRightName, animFadeTime);
            }
            else if (_paddlingLeft && !GetComponent<Animation>().IsPlaying(idleLeftName))
            {
                GetComponent<Animation>().CrossFade(idleLeftName, animFadeTime);
            }
        }

        public void PlayerDamaged()
        {
            if (!_paddlingLeft)
            {
                GetComponent<Animation>().Play(damageRBackName);
                GetComponent<Animation>().CrossFadeQueued(idleRightName, animFadeTime);
            }
            else if (_paddlingLeft)
            {
                GetComponent<Animation>().Play(damageLBackName);
                GetComponent<Animation>().CrossFadeQueued(idleLeftName, animFadeTime);
            }
        }

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            Restart();
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
        }
    }
}
