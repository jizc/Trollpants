// <copyright file="AndroidUtils.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AndroidUtils : MonoBehaviour
    {
        private Dictionary<int, Action<bool>> delegates;

        public int ShowSelectDialog(string msg, Action<bool> del)
        {
            var id = -1;
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.DialogManager"))
            {
                id = cls.CallStatic<int>("ShowSelectDialog", msg);
			    delegates.Add(id, del);
            }
#endif
            return id;
        }

        public int ShowSelectDialog(string title, string msg, Action<bool> del)
        {
            var id = -1;
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.DialogManager"))
            {
                id = cls.CallStatic<int>("ShowSelectTitleDialog", title, msg);
			    delegates.Add(id, del);
            }
#endif
            return id;
        }

        public int ShowSubmitDialog(string msg, Action<bool> del)
        {
            var id = -1;
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.DialogManager"))
            {
                id = cls.CallStatic<int>("ShowSubmitDialog", msg);
			    delegates.Add(id, del);
            }
#endif
            return id;
        }

        public int ShowSubmitDialog(string title, string msg, Action<bool> del)
        {
            var id = -1;
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.DialogManager"))
            {
                id = cls.CallStatic<int>("ShowSubmitTitleDialog", title, msg);
			    delegates.Add(id, del);
            }
#endif
            return id;
        }

        public void DismissDialog(int id)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.DialogManager"))
            {
                cls.CallStatic("DissmissDialog", id);
            }
#endif

            if (delegates.ContainsKey(id))
            {
                delegates[id](false);
                delegates.Remove(id);
            }
            else
            {
                Debug.LogWarning("undefined id:" + id);
            }
        }

        public void SetLabel(string decide, string cancel, string close)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.DialogManager"))
            {
                cls.CallStatic("SetLabel", decide, cancel, close);
            }
#endif
        }

        public void ShowToast(string text)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.ToastManager"))
            {
                cls.CallStatic("ShowToast", text);
            }
#endif
        }

        public void ShowToast(string text, int duration)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.ToastManager"))
            {
                cls.CallStatic("ShowToastForDuration", text, duration);
            }
#endif
        }

        public void ShowToast(string text, int duration, int x, int y)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var cls = new AndroidJavaClass("com.trollpantsandroidutils.ToastManager"))
            {
                cls.CallStatic("ShowPositionedToast", text, duration, x, y);
            }
#endif
        }

        public void OnSubmit(string idStr)
        {
            var id = int.Parse(idStr);
            if (delegates.ContainsKey(id))
            {
                delegates[id](true);
                delegates.Remove(id);
            }
            else
            {
                Debug.LogWarning("undefined id:" + idStr);
            }
        }

        public void OnCancel(string idStr)
        {
            var id = int.Parse(idStr);
            if (delegates.ContainsKey(id))
            {
                delegates[id](false);
                delegates.Remove(id);
            }
            else
            {
                Debug.LogWarning("undefined id:" + idStr);
            }
        }

        private void OnDestroy()
        {
            if (delegates != null)
            {
                delegates.Clear();
                delegates = null;
            }
        }

        private void Awake()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Make sure the GameObject is called "TrollpantsAndroidUtils" so the Java callbacks will work
            name = "TrollpantsAndroidUtils";

            delegates = new Dictionary<int, Action<bool>>();

            // set default labels
            SetLabel("Yes", "Cancel", "Close");
#endif
        }

        private void CallStaticToast(string methodName, params object[] args)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var ajc = new AndroidJavaClass("com.trollpantsandroidutils.ToastManager"))
            {
                ajc.CallStatic(methodName, args);
            }
#endif
        }
    }
}
