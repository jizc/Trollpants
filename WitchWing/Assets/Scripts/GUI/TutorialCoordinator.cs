// <copyright file="TutorialCoordinator.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System.Collections;
    using Data;
    using Player;
    using Player.Controls;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class TutorialCoordinator : MonoBehaviour
    {
        private static TutorialCoordinator s_instance;

        [SerializeField] private GameObject webTutorialPanel;
        [SerializeField] private GameObject tiltTutorialPanel;
        [SerializeField] private GameObject upArrow;
        [SerializeField] private GameObject downArrow;
        [SerializeField] private GameObject boostArrow;
        [SerializeField] private Text topTiltText;
        [SerializeField] private Text midTiltText;
        [SerializeField] private Text botTiltText;
        [SerializeField] private Image topOutlineBox;
        [SerializeField] private Image midOutlineBox;
        [SerializeField] private Image botOutlineBox;
        [SerializeField] private BoostInput boostInput;

        private bool hasMovedUp;
        private bool hasMovedDown;

        private bool isWebTutorialActive;
        private bool isTiltTutorialActive;

        public static bool IsActive
        {
            get
            {
                return IsControlTutorialActive
                       || s_instance.isTiltTutorialActive
                       || s_instance.isWebTutorialActive;
            }
        }

        public static bool IsControlTutorialActive { get; private set; }

        public static void ResetTutorials()
        {
            PlayerSettings.HasSeenControlsTutorial = false;
            PlayerSettings.HasSeenWebTutorial = false;
            PlayerSettings.Save();

            s_instance.hasMovedUp = false;
            s_instance.hasMovedDown = false;
        }

        public static void ActivateTiltTutorial()
        {
            s_instance.isTiltTutorialActive = true;
            s_instance.tiltTutorialPanel.SetActive(true);
        }

        public static void ActivateWebTutorial()
        {
            GameState.IsPaused = true;
            s_instance.webTutorialPanel.SetActive(true);
            s_instance.isWebTutorialActive = true;
        }

        public void DeactivateTiltTutorial()
        {
            tiltTutorialPanel.SetActive(false);
            isTiltTutorialActive = false;

            ActivateControlsTutorial();
        }

        public void DeactivateWebTutorial()
        {
            // Refill manabar so player has a chance of using the boost if
            // it's empty when tutorial is shown
            Player.RefillMana();

            webTutorialPanel.SetActive(false);
            isWebTutorialActive = false;

            GameState.IsPaused = false;
            PlayerSettings.HasSeenWebTutorial = true;
        }

        private static IEnumerator DelayedActions(float seconds, params UnityAction[] actions)
        {
            yield return new WaitForSeconds(seconds);

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action.SafeInvoke();
                }
            }
        }

        private void ActivateControlsTutorial()
        {
            topTiltText.transform.parent.gameObject.SetActive(true);
            topOutlineBox.gameObject.SetActive(true);
            upArrow.SetActive(true);
            IsControlTutorialActive = true;
        }

        private void Awake()
        {
            s_instance = this;
        }

        private void Update()
        {
            if (!IsControlTutorialActive)
            {
                return;
            }

            if (!hasMovedUp)
            {
                if (Player.Transform.position.y > 2f)
                {
                    var initialText = topTiltText.text;
                    topTiltText.text = "Good job!";
                    topOutlineBox.color = Color.green;
                    upArrow.SetActive(false);
                    hasMovedUp = true;

                    UnityAction turnOffTop = () =>
                    {
                        topTiltText.text = initialText;
                        topOutlineBox.color = Color.white;

                        topTiltText.transform.parent.gameObject.SetActive(false);
                        topOutlineBox.gameObject.SetActive(false);
                    };
                    UnityAction turnOnBottom = () =>
                    {
                        downArrow.SetActive(true);
                        botTiltText.transform.parent.gameObject.SetActive(true);
                        botOutlineBox.gameObject.SetActive(true);
                    };
                    StartCoroutine(DelayedActions(1f, turnOffTop, turnOnBottom));
                }

                return;
            }

            if (!hasMovedDown)
            {
                if (Player.Transform.position.y < -1.8f)
                {
                    var initialText = botTiltText.text;
                    botTiltText.text = "Nice!";
                    botOutlineBox.color = Color.green;
                    downArrow.SetActive(false);
                    hasMovedDown = true;

                    UnityAction turnOffBottom = () =>
                    {
                        botTiltText.text = initialText;
                        botOutlineBox.color = Color.white;

                        botTiltText.transform.parent.gameObject.SetActive(false);
                        botOutlineBox.gameObject.SetActive(false);
                    };
                    UnityAction turnOnMid = () =>
                    {
                        boostArrow.SetActive(true);
                        midTiltText.transform.parent.gameObject.SetActive(true);
                        midOutlineBox.gameObject.SetActive(true);
                    };
                    StartCoroutine(DelayedActions(1f, turnOffBottom, turnOnMid));
                }

                return;
            }

            var yPos = Player.Transform.position.y;
            if (yPos > -0.2f && yPos < 0.6f)
            {
                midOutlineBox.color = Color.green;

                if (boostInput.IsPressingBoost)
                {
                    boostArrow.SetActive(false);
                    midTiltText.transform.parent.gameObject.SetActive(false);
                    midOutlineBox.gameObject.SetActive(false);
                    IsControlTutorialActive = false;
                    GameState.IsPaused = false;
                    PlayerSettings.HasSeenControlsTutorial = true;
                    PlayerSettings.Save();
                }
            }
            else
            {
                midOutlineBox.color = Color.white;
            }
        }
    }
}
