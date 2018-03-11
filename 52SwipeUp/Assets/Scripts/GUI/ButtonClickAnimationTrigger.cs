// <copyright file="ButtonClickAnimationTrigger.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public enum AnimationTrigger
    {
        Show,
        Hide,
        ToggleIn,
        ToggleOut
    }

    [RequireComponent(typeof(Button))]
    public class ButtonClickAnimationTrigger : MonoBehaviour
    {
        [SerializeField] private AnimationTrigger selectedAnimationTrigger;
        [SerializeField] private AnimationTrigger toggleAnimationTrigger;
        [SerializeField] private bool toggleable;
        [SerializeField] private bool wasToggled;
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private Button button;

        public AnimationTrigger SelectedAnimationTrigger
        {
            get { return selectedAnimationTrigger; }
            set { selectedAnimationTrigger = value; }
        }

        public AnimationTrigger ToggleAnimationTrigger
        {
            get { return toggleAnimationTrigger; }
            set { toggleAnimationTrigger = value; }
        }

        public bool Toggleable
        {
            get { return toggleable; }
            set { toggleable = value; }
        }

        public Animator TargetAnimator
        {
            get
            {
                if (targetAnimator == null)
                {
                    Debug.LogWarning("No Animator-component to trigger animations in.");
                }

                return targetAnimator;
            }
            set { targetAnimator = value; }
        }

        public Button CachedButton
        {
            get
            {
                if (button == null)
                {
                    button = GetComponent<Button>();
                }

                return button;
            }
        }

        public void OnClick()
        {
            if (!TargetAnimator)
            {
#if DEBUG
                Debug.LogError("No Animator-component found.");
#endif
                return;
            }

            AnimationTrigger trigger = SelectedAnimationTrigger;
            if (Toggleable && wasToggled)
            {
                trigger = ToggleAnimationTrigger;
            }

            switch (trigger)
            {
                case AnimationTrigger.Show:
                    TargetAnimator.SetTrigger("Show");
                    break;
                case AnimationTrigger.Hide:
                    TargetAnimator.SetTrigger("Hide");
                    break;
                case AnimationTrigger.ToggleIn:
                    TargetAnimator.SetTrigger("ToggleIn");
                    break;
                case AnimationTrigger.ToggleOut:
                    TargetAnimator.SetTrigger("ToggleOut");
                    break;
                default:
#if DEBUG
                    Debug.LogWarning("No Trigger selected.");
#endif
                    break;
            }

            wasToggled = !wasToggled;
        }

        private void Awake()
        {
            CachedButton.onClick.AddListener(OnClick);
        }
    }
}
