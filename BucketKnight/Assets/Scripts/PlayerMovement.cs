// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerMovement.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        //numbers for tweaking movement speed specific to each controlling method
        public float touchSensitivity;
        public float tiltSensitivity;
        
        public float directionIndicator;

        public float currentHorizontalSpeed;
        private const float c_maxHorizontalSpeedStart = 0.16f;
        private const float c_maxHorizontalSpeedPowerup = 0.26f;
        public float currentMaxHorizontalSpeed = c_maxHorizontalSpeedStart;

        public TurningSound turningSound;

        public float deceleration;

        //How far the players boat can turn
        public float maxTurn;

        //BarrelRoll related
        public float dodgeAnimationDuration;
        public float BarrelRollCooldown;
        private bool _isBarrelrolling;

        //Animation
        public PlayerAnimController playerAnimController;
        public float playerAnimDelay = 1f;
        public float animationBuffer;
        private float _playerAnimCountdown;

        public AnimationClip barrelRollAnimation;
        public AnimationClip defaultPositionAnimation;
        public Animation barrelRollAnimator;

        //Canvas that holds UI-blocking-elements
        public GameObject uiBlocker;

        //x-position of player when he is in middle of screen
        private float _middlePos;

        //how far the player can move from the center
        private float _maxMovementFromMiddle;

        //the plane on which the player is aligned
        private Plane _xzPlane;

        private Touch _touch;
        private float _timeSinceLastFrameWithNoTouch;
        private float _doubleTouchToBarrelRollTime = 0.3f;

        private bool _pausedThisFrame;

        private void OnEnable()
        {
            Events.instance.AddListener<PlayerLost>(OnPlayerLost);
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
            Events.instance.AddListener<TurningSpeedChanged>(OnTurningSpeedChanged);
        }

        private void Start()
        {
            touchSensitivity = 0.03f;
            tiltSensitivity = 0.5f;

            maxTurn = 50.0f;

            directionIndicator = 0;
            deceleration = 0.01f;

            currentHorizontalSpeed = 0;
            _maxMovementFromMiddle = 6.8f;

            _xzPlane = new Plane(Vector3.up, Vector3.zero);
            _middlePos = transform.position.x;

            _playerAnimCountdown = playerAnimDelay;

            Restart();
        }

        public void Restart()
        {
            _isBarrelrolling = false;
            barrelRollAnimator.Play(defaultPositionAnimation.name);
            transform.position = new Vector3(_middlePos, -0.25f, transform.position.z);
        }

        private void Update()
        {
            if (!SavedData.GameIsPaused)
            {
                // Timer that counts how long since last time no touches was registered
                if (_timeSinceLastFrameWithNoTouch <= _doubleTouchToBarrelRollTime)
                {
                    _timeSinceLastFrameWithNoTouch += Time.deltaTime;
                }
            }

            _pausedThisFrame = SavedData.GameIsPaused;
        }

        private void FixedUpdate()
        {
            if (!SavedData.GameIsPaused)
            {
                turningSound.setVolume(currentHorizontalSpeed);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
                SetSpeedPc();
#else
				SetSpeedMobile();
#endif
                SetRotation();
                SetAnimation();
                Move();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    BarrelRoll();
                }
            }
        }

        private void SetSpeedPc()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (currentHorizontalSpeed > -currentMaxHorizontalSpeed)
                {
                    currentHorizontalSpeed -= 0.04f;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (currentHorizontalSpeed < currentMaxHorizontalSpeed)
                {
                    currentHorizontalSpeed += 0.04f;
                }
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                BarrelRoll();
            }
            else
            {
                currentHorizontalSpeed = currentHorizontalSpeed / 1.3f;
            }
        }

        /// <summary>
        /// Finds what control type the player uses, and sets turning speed
        /// </summary>
        private void SetSpeedMobile()
        {
            // user is controlling with touch
            if (!SavedData.UseTilt)
            {
                if (Input.touches.Length >= 1)
                {
                    _touch = Input.touches[Input.touches.Length - 1];

                    // check if user has touched a place on the screen that is defined to block steering
                    foreach (RectTransform blockTransform in uiBlocker.transform)
                    {
                        if (blockTransform.name == "EquipmentSlotsBlocker")
                        {
                            if (SavedData.PowerupInSlotOne == Enums.PowerupType.None
                                && SavedData.PowerupInSlotTwo == Enums.PowerupType.None)
                            {
                                continue;
                            }
                        }

                        var worldCorners = new Vector3[4];
                        blockTransform.GetWorldCorners(worldCorners);

                        if (_touch.position.x > worldCorners[0].x && _touch.position.x < worldCorners[2].x
                            && _touch.position.y > worldCorners[0].y && _touch.position.y < worldCorners[2].y)
                        {
                            return;
                        }
                    }

                    if (Input.touches.Length == 2 && _timeSinceLastFrameWithNoTouch < _doubleTouchToBarrelRollTime)
                    {
                        var touchOne = Input.touches[0].position.x;
                        var touchTwo = Input.touches[1].position.x;

                        if (touchOne > touchTwo)
                        {
                            var temp = touchOne;
                            touchOne = touchTwo;
                            touchTwo = temp;
                        }

                        if (touchOne < Screen.width / 3f && touchTwo > Screen.width / 3 * 2)
                        {
                            BarrelRoll();
                            return;
                        }
                    }

                    if (_touch.position.x < Screen.width / 3f)
                    {
                        if (currentHorizontalSpeed > -currentMaxHorizontalSpeed)
                        {
                            currentHorizontalSpeed -= 0.04f;
                        }
                    }
                    if (_touch.position.x > Screen.width / 3 * 2)
                    {
                        if (currentHorizontalSpeed < currentMaxHorizontalSpeed)
                        {
                            currentHorizontalSpeed += 0.04f;
                        }
                    }
                    if (_touch.position.x > Screen.width / 3f && _touch.position.x < Screen.width / 3 * 2)
                    {
                        BarrelRoll();
                    }
                }
                else
                {
                    currentHorizontalSpeed = currentHorizontalSpeed / 1.3f;

                    // reset timer
                    _timeSinceLastFrameWithNoTouch = 0;
                }
            }
            // user is controlling by tilting
            else if (SavedData.UseTilt && Math.Abs(Input.acceleration.x) > 0.05)
            {
                if (Input.touches.Length != 0 &&
                    Input.touches[Input.touches.Length - 1].position.y < Screen.height - 200)
                {
                    BarrelRoll();
                }

                var xAcceleration = Input.acceleration.x;
                directionIndicator = xAcceleration / Math.Abs(xAcceleration);
                xAcceleration = xAcceleration - 0.05f * directionIndicator;
                currentHorizontalSpeed = xAcceleration * tiltSensitivity;
            }
            else if (Math.Abs(currentHorizontalSpeed) > 0.01)
            {
                currentHorizontalSpeed += deceleration * directionIndicator * -1;
            }

            // If over max speed, clip down. Separeted into if/else to avoid tilting bugs.
            if (currentHorizontalSpeed > currentMaxHorizontalSpeed)
            {
                currentHorizontalSpeed = currentMaxHorizontalSpeed;
            }
            else if (currentHorizontalSpeed < currentMaxHorizontalSpeed * -1)
            {
                currentHorizontalSpeed = currentMaxHorizontalSpeed * -1;
            }
        }

        private void SetRotation()
        {
            // rotates player based on current speed
            transform.rotation = Quaternion.Euler(0, 90 + currentHorizontalSpeed * maxTurn, 90);
        }

        private void SetAnimation()
        {
            _playerAnimCountdown -= Time.deltaTime;
            if (_playerAnimCountdown < 0)
            {
                if (currentHorizontalSpeed > animationBuffer)
                {
                    playerAnimController.PaddleLeft();
                }
                else if (currentHorizontalSpeed < animationBuffer * -1)
                {
                    playerAnimController.PaddleRight();
                }
                else
                {
                    playerAnimController.Idle();
                }
                _playerAnimCountdown = playerAnimDelay;
            }

        }

        /// <summary>
        /// Moves the player based on current speed
        /// </summary>
        private void Move()
        {
            if (Equals(currentHorizontalSpeed, 0f))
            {
                return;
            }

            // set new position
            transform.position = new Vector3(transform.position.x + currentHorizontalSpeed, transform.position.y,
                transform.position.z);

            // update position to be max left/right if it exceeds max left/right range. Separated into an if/else to prevent some tilting bugs
            if (transform.position.x < _middlePos - _maxMovementFromMiddle)
            {
                transform.position = new Vector3(_middlePos + _maxMovementFromMiddle * -1, transform.position.y,
                    transform.position.z);
            }
            else if (transform.position.x > _middlePos + _maxMovementFromMiddle)
            {
                transform.position = new Vector3(_middlePos + _maxMovementFromMiddle, transform.position.y,
                    transform.position.z);
            }
        }

        // http://gamedev.stackexchange.com/questions/75649/how-do-i-get-mouse-x-y-of-the-world-plane-in-unity
        /// <summary>
        /// Returns the x position on the XZ-plane corresponding to where the mouse is on the screen
        /// </summary>
        /// <returns></returns>
        private float GetXPositionOnXZPlane()
        {
            float distance;
            // Creates ray from mousePosition, finds point where ray and XZ-plane intersects
            var rayBetweenPlayerAndTouch = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_xzPlane.Raycast(rayBetweenPlayerAndTouch, out distance))
            {
                var hitPoint = rayBetweenPlayerAndTouch.GetPoint(distance).x;

                return hitPoint;
            }

            // not good return value
            return 0;
        }

        #region Event handlers

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            Restart();
        }

        private void OnPlayerLost(PlayerLost playerLostEvent)
        {
            _isBarrelrolling = false;
        }

        private void OnTurningSpeedChanged(TurningSpeedChanged turningSpeedChangedEvent)
        {
            currentMaxHorizontalSpeed = turningSpeedChangedEvent.active ? c_maxHorizontalSpeedPowerup : c_maxHorizontalSpeedStart;
        }

        public void BarrelRoll()
        {
            if (_isBarrelrolling || _pausedThisFrame)
            {
                return;
            }

            barrelRollAnimator.Play(barrelRollAnimation.name);
        }

        public void ChangeBarrelRoll()
        {
            _isBarrelrolling = !_isBarrelrolling;
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<PlayerLost>(OnPlayerLost);
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
            Events.instance.RemoveListener<TurningSpeedChanged>(OnTurningSpeedChanged);
        }
    }
}
