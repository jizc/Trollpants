// <copyright file="PanelAnimatorHelper.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEngine;

    public class PanelAnimatorHelper : MonoBehaviour
    {
        [SerializeField] private string showInsantStateName;
        [SerializeField] private string hideInstantStateName;
        [SerializeField] private string showStateName;
        [SerializeField] private string hideStateName;

        private Animator cachedAnimator;

        public delegate void AnimationEventHandler(GameObject sender, string message);

        public event AnimationEventHandler OnHidden;
        public event AnimationEventHandler OnShowStart;
        public event AnimationEventHandler OnShown;

        private Animator CachedAnimator
        {
            get { return cachedAnimator ?? (cachedAnimator = GetComponent<Animator>()); }
        }

        public void ShowPanelInstantly()
        {
            OnShowStart?.Invoke(gameObject, "OnShow");
            gameObject.SetActive(true);

            if (CheckIfCurrentState(showInsantStateName))
            {
                return;
            }

            gameObject.SetActive(true);
            CachedAnimator.Play(showInsantStateName);
        }

        public void HidePanelInstantly()
        {
            if (CheckIfCurrentState(hideInstantStateName))
            {
                return;
            }

            CachedAnimator.Play(hideInstantStateName);
            gameObject.SetActive(false);
        }

        // Part of workaround for Rules card flashing before animating on to the screen
        public void HidePanelInstantly(bool leavePanelActive)
        {
            if (CheckIfCurrentState(hideInstantStateName))
            {
                return;
            }

            CachedAnimator.Play(hideInstantStateName);
            gameObject.SetActive(leavePanelActive);
        }

        public void ShowPanel()
        {
            OnShowStart?.Invoke(gameObject, "OnShow");
            gameObject.SetActive(true);

            if (!CheckIfCurrentState(showStateName))
            {
                CachedAnimator.Play(showStateName);
            }
        }

        public void HidePanel()
        {
            if (!CheckIfCurrentState(hideStateName))
            {
                CachedAnimator.Play(hideStateName);
            }
        }

        public void OnShowComplete()
        {
#if DEBUG
            Debug.Log("OnShowComplete called on " + name + ".");
#endif

            if (OnShown != null)
            {
                OnShown(gameObject, "OnShowComplete");
            }
        }

        public void OnHideComplete()
        {
#if DEBUG
            Debug.Log("OnHideComplete called on " + name + ".");
#endif
            gameObject.SetActive(false);

            if (OnHidden != null)
            {
                OnHidden(gameObject, "OnHideComplete");
            }
        }

        private bool CheckIfCurrentState(string stateName)
        {
            if (!gameObject.activeSelf)
            {
                return true;
            }

            var current = CachedAnimator.GetCurrentAnimatorStateInfo(0);
            return current.IsName(stateName);
        }
    }
}
