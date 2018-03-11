// <copyright file="PlayerSettings.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Data
{
    using UnityEngine;

    public static class PlayerSettings
    {
        private const string isInvertedKey = "IsInverted";
        private const string muteMusicKey = "MuteMusic";
        private const string muteSfxKey = "MuteSFX";
        private const string volumeMusicKey = "VolumeMusic";
        private const string volumeSfxKey = "VolumeSFX";
        private const string hasSeenStartTutorialKey = "HasSeenTutorial1";
        private const string hasSeenWebTutorialKey = "HasSeenTutorial2";

        public static bool IsYAxisInverted { get; set; }
        public static float MusicVolume { get; set; }
        public static float SfxVolume { get; set; }
        public static bool MuteMusic { get; set; }
        public static bool MuteSfx { get; set; }
        public static bool HasSeenControlsTutorial { get; set; }
        public static bool HasSeenWebTutorial { get; set; }

        public static void Load()
        {
            IsYAxisInverted = PlayerPrefs.GetInt(isInvertedKey, 0) == 1;
            MusicVolume = PlayerPrefs.GetFloat(volumeMusicKey, 1f);
            SfxVolume = PlayerPrefs.GetFloat(volumeSfxKey, 1f);
            MuteMusic = PlayerPrefs.GetInt(muteMusicKey, 0) == 1;
            MuteSfx = PlayerPrefs.GetInt(muteSfxKey, 0) == 1;
            HasSeenControlsTutorial = PlayerPrefs.GetInt(hasSeenStartTutorialKey, 0) == 1;
            HasSeenWebTutorial = PlayerPrefs.GetInt(hasSeenWebTutorialKey, 0) == 1;
        }

        public static void Save()
        {
            PlayerPrefs.SetInt(isInvertedKey, IsYAxisInverted ? 1 : 0);
            PlayerPrefs.SetFloat(volumeMusicKey, MusicVolume);
            PlayerPrefs.SetFloat(volumeSfxKey, SfxVolume);
            PlayerPrefs.SetInt(muteMusicKey, MuteMusic ? 1 : 0);
            PlayerPrefs.SetInt(muteSfxKey, MuteSfx ? 1 : 0);
            PlayerPrefs.SetInt(hasSeenStartTutorialKey, HasSeenControlsTutorial ? 1 : 0);
            PlayerPrefs.SetInt(hasSeenWebTutorialKey, HasSeenWebTutorial ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
