namespace UnityStandardAssets.CrossPlatformInput.Inspector
{
    using System.Collections.Generic;
    using UnityEditor;

    [InitializeOnLoad]
    public class CrossPlatformInputInitialize
    {
        private const string mobileInput = "MOBILE_INPUT";

        private static readonly BuildTargetGroup[] s_buildTargetGroups =
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.WSA
        };

        private static readonly BuildTargetGroup[] s_mobileBuildTargetGroups =
        {
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.PSM,
            BuildTargetGroup.Tizen,
            BuildTargetGroup.WSA
        };

        static CrossPlatformInputInitialize()
        {
            var defines = GetDefinesList(s_buildTargetGroups[0]);
            if (!defines.Contains(mobileInput))
            {
                SetEnabled(mobileInput, true, true);
            }
        }

        [MenuItem("Mobile Input/Enable")]
        private static void Enable()
        {
            SetEnabled(mobileInput, true, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                case BuildTarget.iOS:
                case BuildTarget.PSM:
                case BuildTarget.Tizen:
                case BuildTarget.WSAPlayer:
                    EditorUtility.DisplayDialog(
                        "Mobile Input",
                        "You have enabled Mobile Input. You'll need to use the Unity Remote app on a connected device to control your game in the Editor.",
                        "OK");
                    break;

                default:
                    const string message = "You have enabled Mobile Input, but you have a non-mobile build target selected in your build settings."
                                        + " The mobile control rigs won't be active or visible on-screen until you switch the build target to a mobile platform.";
                    EditorUtility.DisplayDialog("Mobile Input", message, "OK");
                    break;
            }
        }

        [MenuItem("Mobile Input/Enable", true)]
        private static bool EnableValidate()
        {
            var defines = GetDefinesList(s_mobileBuildTargetGroups[0]);
            return !defines.Contains(mobileInput);
        }

        [MenuItem("Mobile Input/Disable")]
        private static void Disable()
        {
            SetEnabled("MOBILE_INPUT", false, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                case BuildTarget.iOS:
                    EditorUtility.DisplayDialog(
                        "Mobile Input",
                        "You have disabled Mobile Input. Mobile control rigs won't be visible, and the Cross Platform Input functions will always return standalone controls.",
                        "OK");
                    break;
            }
        }

        [MenuItem("Mobile Input/Disable", true)]
        private static bool DisableValidate()
        {
            var defines = GetDefinesList(s_mobileBuildTargetGroups[0]);
            return defines.Contains(mobileInput);
        }

        private static void SetEnabled(string defineName, bool enable, bool mobile)
        {
            foreach (var group in mobile ? s_mobileBuildTargetGroups : s_buildTargetGroups)
            {
                var defines = GetDefinesList(group);
                if (enable)
                {
                    if (defines.Contains(defineName))
                    {
                        return;
                    }

                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                    {
                        return;
                    }

                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }

                var definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }

        private static List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
    }
}
