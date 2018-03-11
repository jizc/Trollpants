// <copyright file="NativeButtonManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SwipeUp.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class NativeButtonManager : MonoBehaviour
    {
        [Header("Image Components")]
        [SerializeField] private Image leaderboardImage;
        [SerializeField] private Image achievementsImage;

        [Header("Google Play Icons")]
        [SerializeField] private Sprite googleLeaderboardIcon;
        [SerializeField] private Sprite googleAchievementsIcon;

        [Header("iOS Icons")]
        [SerializeField] private Sprite iosLeaderboardIcon;
        [SerializeField] private Sprite iosAchievementsIcon;

        private void Awake()
        {
            if (leaderboardImage == null)
            {
                Debug.LogError("Leaderboard image component not set");
                return;
            }

            if (achievementsImage == null)
            {
                Debug.LogError("Achievements image component not set");
                return;
            }

#if CLOUDONCE_GOOGLE
            if (googleLeaderboardIcon != null)
            {
                leaderboardImage.sprite = googleLeaderboardIcon;
            }

            if (googleAchievementsIcon != null)
            {
                achievementsImage.sprite = googleAchievementsIcon;
            }
#elif UNITY_IOS
            if (iosLeaderboardIcon != null)
            {
                leaderboardImage.sprite = iosLeaderboardIcon;
            }

            if (iosAchievementsIcon != null)
            {
                achievementsImage.sprite = iosAchievementsIcon;
            }
#endif
        }
    }
}
