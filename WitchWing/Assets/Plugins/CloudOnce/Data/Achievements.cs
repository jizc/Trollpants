// <copyright file="Achievements.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace CloudOnce
{
    using Internal;

    /// <summary>
    /// Provides access to achievements registered via the CloudOnce Editor.
    /// This file was automatically generated by CloudOnce. Do not edit.
    /// </summary>
    public static class Achievements
    {
        private static readonly UnifiedAchievement s_offToAFlyingStart = new UnifiedAchievement("OffToAFlyingStart",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "OffToAFlyingStart"
#endif
            );

        public static UnifiedAchievement OffToAFlyingStart
        {
            get { return s_offToAFlyingStart; }
        }

        private static readonly UnifiedAchievement s_getYourWings = new UnifiedAchievement("GetYourWings",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "GetYourWings"
#endif
            );

        public static UnifiedAchievement GetYourWings
        {
            get { return s_getYourWings; }
        }

        private static readonly UnifiedAchievement s_flyingHigh = new UnifiedAchievement("FlyingHigh",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "FlyingHigh"
#endif
            );

        public static UnifiedAchievement FlyingHigh
        {
            get { return s_flyingHigh; }
        }

        private static readonly UnifiedAchievement s_withFlyingColors = new UnifiedAchievement("WithFlyingColors",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "WithFlyingColors"
#endif
            );

        public static UnifiedAchievement WithFlyingColors
        {
            get { return s_withFlyingColors; }
        }

        private static readonly UnifiedAchievement s_broomAce = new UnifiedAchievement("BroomAce",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "BroomAce"
#endif
            );

        public static UnifiedAchievement BroomAce
        {
            get { return s_broomAce; }
        }

        private static readonly UnifiedAchievement s_frequentFlyer = new UnifiedAchievement("FrequentFlyer",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "FrequentFlyer"
#endif
            );

        public static UnifiedAchievement FrequentFlyer
        {
            get { return s_frequentFlyer; }
        }

        private static readonly UnifiedAchievement s_masterOfWisdom = new UnifiedAchievement("MasterOfWisdom",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "MasterOfWisdom"
#endif
            );

        public static UnifiedAchievement MasterOfWisdom
        {
            get { return s_masterOfWisdom; }
        }

        private static readonly UnifiedAchievement s_broomAcrobat = new UnifiedAchievement("BroomAcrobat",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "BroomAcrobat"
#endif
            );

        public static UnifiedAchievement BroomAcrobat
        {
            get { return s_broomAcrobat; }
        }

        private static readonly UnifiedAchievement s_aerodynamicWonder = new UnifiedAchievement("AerodynamicWonder",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "AerodynamicWonder"
#endif
            );

        public static UnifiedAchievement AerodynamicWonder
        {
            get { return s_aerodynamicWonder; }
        }

        private static readonly UnifiedAchievement s_vigorousWillpower = new UnifiedAchievement("VigorousWillpower",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "VigorousWillpower"
#endif
            );

        public static UnifiedAchievement VigorousWillpower
        {
            get { return s_vigorousWillpower; }
        }

        private static readonly UnifiedAchievement s_veteranWitch = new UnifiedAchievement("VeteranWitch",
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_GOOGLE
            ""
#elif !UNITY_EDITOR && UNITY_ANDROID && CLOUDONCE_AMAZON
            ""
#else
            "VeteranWitch"
#endif
            );

        public static UnifiedAchievement VeteranWitch
        {
            get { return s_veteranWitch; }
        }

        public static readonly UnifiedAchievement[] All =
        {
            s_offToAFlyingStart,
            s_getYourWings,
            s_flyingHigh,
            s_withFlyingColors,
            s_broomAce,
            s_frequentFlyer,
            s_masterOfWisdom,
            s_broomAcrobat,
            s_aerodynamicWonder,
            s_vigorousWillpower,
            s_veteranWitch,
        };
    }
}
