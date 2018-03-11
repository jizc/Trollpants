// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenShakeOnPlayerLost.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using DG.Tweening;
    using UnityEngine;

    /// <summary>
    ///  Shakes the camera when the player loses the game
    /// </summary>
    public static class ScreenShakeOnPlayerLost
    {
        #region Fields & properties

        private const float c_duration = 0.5f;
        private static Camera _camera;
        private static readonly Vector3 s_hitPosStrength = new Vector3(0.5f, 1f, 0f);
        private static readonly Vector3 s_hitRotStrength = new Vector3(2f, 2f, 0f);

        private static Camera MainCamera
        {
            get { return _camera ?? (_camera = Camera.main); }
        }

        #endregion /Fields & properties

        #region Public methods

        public static void ShakeCamera()
        {
            MainCamera.DOShakePosition(c_duration, s_hitPosStrength);
            MainCamera.DOShakeRotation(c_duration, s_hitRotStrength);
        }

        #endregion / Private methods
    }
}
