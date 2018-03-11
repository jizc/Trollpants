// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TutorialManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using CloudOnce;
    using UnityEngine;
    using UnityEngine.UI;

    public class TutorialManager : MonoBehaviour
    {
        private static readonly int s_halfScreenWidth = Screen.width / 2;

        [SerializeField] private Text _left;
        [SerializeField] private Text _right;

        private bool pressedLeft;
        private bool pressedRight;
        private bool setNewTextLeft;
        private bool setNewTextRight;
        private GameObject player;
        private float time = 2;
        private Rigidbody2D playerRigidbody2D;
        private BaseController playerBaseController;

        private static bool BoostInputDetected()
        {
            if (Input.touchCount != 2)
            {
                return false;
            }

            if (Input.touches[0].position.x < s_halfScreenWidth)
            {
                return Input.touches[1].position.x > s_halfScreenWidth;
            }

            return Input.touches[1].position.x < s_halfScreenWidth;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            playerBaseController = player.GetComponent<ShipSupervisor>().ControllerInstance;

            playerRigidbody2D = player.GetComponent<Rigidbody2D>();

            if (!CloudVariables.HasFinishedTutorial)
            {
                playerBaseController.IsEngineOn = false;
            }
        }

        private void Update()
        {
            if (!pressedLeft && playerRigidbody2D.angularVelocity > 0)
            {
                pressedLeft = true;
                _left.enabled = false;
            }
            else if (!pressedRight && playerRigidbody2D.angularVelocity < 0)
            {
                pressedRight = true;
                _right.enabled = false;
            }

            if (pressedLeft && pressedRight && !setNewTextLeft)
            {
                _left.text = "Follow the Arrow.\nDrift around Asteroids for points.";
                _left.enabled = true;
                setNewTextLeft = true;
            }

            if (setNewTextLeft)
            {
                if (time <= 0f)
                {
                    if (!setNewTextRight)
                    {
                        _right.text = "Pressing both sides activates boost.\n\nBoost to begin the game!";
                        _right.enabled = true;
                        setNewTextRight = true;
                    }

                    if (BoostInputDetected() || Input.GetKeyDown(KeyCode.Space))
                    {
                        if (TrackedData.Instance)
                        {
                            CloudVariables.HasFinishedTutorial = true;
                        }

                        playerBaseController.IsEngineOn = true;
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
        }
    }
}
