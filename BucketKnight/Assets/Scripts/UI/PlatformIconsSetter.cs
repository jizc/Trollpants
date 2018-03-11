// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlatformIconsSetter.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    // <summary>
    //  Sets the correct native platform icons on the buttons in the main menu.
    // </summary>
    public class PlatformIconsSetter : MonoBehaviour
    {
        #region Fields & properties

#pragma warning disable 649
        [Header("Image Components")]
        [SerializeField] private Image _nativeImage;
        [SerializeField] private Image _leaderboardImage;
        [SerializeField] private Image _achievementsImage;

        [Header("Amazon Icons")]
        [SerializeField] private Sprite _gameCircleIcon;
        [SerializeField] private Sprite _gameCircleLeaderboardIcon;
        [SerializeField] private Sprite _gameCircleAchievementsIcon;

        [Header("Google Play Icons")]
        [SerializeField] private Sprite _gameServicesIcon;
        [SerializeField] private Sprite _gsLeaderboardIcon;
        [SerializeField] private Sprite _gsAchievementsIcon;

        [Header("iOS Icons")]
        [SerializeField] private Sprite _gameCenterIcon;
        [SerializeField] private Sprite _iosLeaderboardIcon;
        [SerializeField] private Sprite _iosAchievementsIcon;
#pragma warning restore 649

        #endregion /Fields & properties

        #region Unity methods

        private void Awake()
        {
            if (_nativeImage == null)
            {
                Debug.LogError("Native image component not set");
                return;
            }

            if (_leaderboardImage == null)
            {
                Debug.LogError("Leaderboard image component not set");
                return;
            }

            if (_achievementsImage == null)
            {
                Debug.LogError("Achievements image component not set");
                return;
            }

#if CLOUDONCE_AMAZON
            if (_gameCircleIcon != null)
            {
                _nativeImage.sprite = _gameCircleIcon;
            }

            if (_gameCircleLeaderboardIcon != null)
            {
                _leaderboardImage.sprite = _gameCircleLeaderboardIcon;
            }

            if (_gameCircleAchievementsIcon != null)
            {
                _achievementsImage.sprite = _gameCircleAchievementsIcon;
            }

#elif CLOUDONCE_GOOGLE
            if (_gameServicesIcon != null)
            {
                _nativeImage.sprite = _gameServicesIcon;
            }

            if (_gsLeaderboardIcon != null)
            {
                _leaderboardImage.sprite = _gsLeaderboardIcon;
            }

            if (_gsAchievementsIcon != null)
            {
                _achievementsImage.sprite = _gsAchievementsIcon;
            }

#elif UNITY_IOS
            if (_gameCenterIcon != null)
            {
                _nativeImage.sprite = _gameCenterIcon;
            }

            if (_iosLeaderboardIcon != null)
            {
                _leaderboardImage.sprite = _iosLeaderboardIcon;
            }

            if (_iosAchievementsIcon != null)
            {
                _achievementsImage.sprite = _iosAchievementsIcon;
            }

#endif
        }

        #endregion /Unity methods
    }
}
