// <copyright file="ButtonClickAnimationTriggerEditor.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(ButtonClickAnimationTrigger))]
    public class ButtonClickAnimationTriggerEditor : Editor
    {
        private ButtonClickAnimationTrigger animationTrigger;

        private ButtonClickAnimationTrigger AnimationTrigger
        {
            get { return animationTrigger ?? (animationTrigger = (ButtonClickAnimationTrigger)target); }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            AnimationTrigger.SelectedAnimationTrigger = (AnimationTrigger)EditorGUILayout.EnumPopup("Selected Trigger:", AnimationTrigger.SelectedAnimationTrigger);
            if (AnimationTrigger.Toggleable)
            {
                AnimationTrigger.ToggleAnimationTrigger = (AnimationTrigger)EditorGUILayout.EnumPopup("Toggles to:", AnimationTrigger.ToggleAnimationTrigger);
            }

            AnimationTrigger.Toggleable = EditorGUILayout.Toggle("Toggleable:", AnimationTrigger.Toggleable);
            AnimationTrigger.TargetAnimator = (Animator)EditorGUILayout.ObjectField("Target Animator:", AnimationTrigger.TargetAnimator, typeof(Animator), true);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(AnimationTrigger.gameObject);
            }
        }
    }
}
