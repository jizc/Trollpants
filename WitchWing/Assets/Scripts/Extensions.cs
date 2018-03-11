// <copyright file="Extensions.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing
{
    using UnityEngine;
    using UnityEngine.Events;

    public static class Extensions
    {
        public static void SafeInvoke(this UnityAction action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        public static void SafeInvoke<T>(this UnityAction<T> action, T value)
        {
            if (action != null)
            {
                action.Invoke(value);
            }
        }

        public static Color Slerp(this Color a, Color b, float t)
        {
            return SlerpImpl(a, b, t);
        }

        public static Color32 Slerp(this Color32 a, Color32 b, float t)
        {
            return SlerpImpl(a, b, t);
        }

        private static Color SlerpImpl(Color a, Color b, float t)
        {
            float aH, aS, aV, bH, bS, bV;
            Color.RGBToHSV(a, out aH, out aS, out aV);
            Color.RGBToHSV(b, out bH, out bS, out bV);

            var angle = Mathf.LerpAngle(aH * 360f, bH * 360f, t);
            while (angle < 0f)
            {
                angle += 360f;
            }

            while (angle > 360f)
            {
                angle -= 360f;
            }

            var newRgb = Color.HSVToRGB(angle / 360f, Mathf.Lerp(aS, bS, t), Mathf.Lerp(aV, bV, t));

            return new Color(newRgb.r, newRgb.g, newRgb.b, Mathf.Lerp(a.a, b.a, t));
        }
    }
}
