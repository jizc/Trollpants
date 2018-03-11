// <copyright file="CloudOnceUpgrader.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2017 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

#if UNITY_EDITOR
namespace CloudOnce.Internal.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Data;
    using UnityEditor;
    using UnityEngine;
    using Utils;

    [InitializeOnLoad]
    public class CloudOnceUpgrader
    {
        static CloudOnceUpgrader()
        {
            var config = SerializationUtils.LoadCloudConfig();
            if (string.IsNullOrEmpty(config.Version))
            {
                return;
            }

            var oldVersion = new Version(config.Version);
            if (PluginVersion.Version.Equals(oldVersion))
            {
                return;
            }

            var upgraded = false;
            if (oldVersion < new Version(2, 4))
            {
                Upgrade240();
                upgraded = true;
            }

            if (oldVersion < new Version(2, 5))
            {
                Upgrade250();
                upgraded = true;
            }

            if (upgraded)
            {
                AssetDatabase.Refresh();
            }
        }

        private static void Upgrade240()
        {
            var obsoleteFiles = new[]
            {
                CloudOncePaths.GPGS + "/Editor/GPGSDependencies.cs",
                "Assets/Plugins/Android/libs/armeabi-v7a/libgpg.so",
                "Assets/Plugins/Android/libs/x86/libgpg.so",
                "Assets/Plugins/Android/MainLibProj/libs/play-games-plugin-support.jar",
                "Assets/Plugins/Android/MainLibProj/AndroidManifest.xml",
                "Assets/Plugins/Android/MainLibProj/project.properties"
            };

            DeleteObsoleteFiles(obsoleteFiles);
        }

        private static void Upgrade250()
        {
            var obsoleteFiles = new[]
            {
                CloudOncePaths.GPGS + "/BasicApi/Quests/IQuest.cs",
                CloudOncePaths.GPGS + "/BasicApi/Quests/IQuestMilestone.cs",
                CloudOncePaths.GPGS + "/BasicApi/Quests/IQuestsClient.cs",
                CloudOncePaths.GPGS + "/Editor/GPGSDependencies.xml",
                CloudOncePaths.GPGS + "/Platforms/Native/Cwrapper/Quest.cs",
                CloudOncePaths.GPGS + "/Platforms/Native/Cwrapper/QuestManager.cs",
                CloudOncePaths.GPGS + "/Platforms/Native/Cwrapper/QuestMilestone.cs",
                CloudOncePaths.GPGS + "/Platforms/Native/NativeQuestClient.cs",
                CloudOncePaths.GPGS + "/Platforms/Native/PInvoke/NativeQuest.cs",
                CloudOncePaths.GPGS + "/Platforms/Native/PInvoke/NativeQuestMilestone.cs",
                CloudOncePaths.GPGS + "/Platforms/Native/PInvoke/QuestManager.cs",
                "Assets/Plugins/Android/play-games-plugin-support.aar"
            };

            DeleteObsoleteFiles(obsoleteFiles);
        }

        private static void DeleteObsoleteFiles(IEnumerable<string> obsoleteFiles)
        {
            foreach (var file in obsoleteFiles.Where(File.Exists))
            {
                Debug.Log("Deleting obsolete file: " + file);
                File.Delete(file);
                var metaFile = file + ".meta";
                if (File.Exists(metaFile))
                {
                    File.Delete(metaFile);
                }

                var directory = Path.GetDirectoryName(file);
                if (directory == null)
                {
                    continue;
                }

                var directoryInfo = new DirectoryInfo(directory);
                if (directoryInfo.GetFiles().Length != 0)
                {
                    continue;
                }

                Debug.Log("Deleting obsolete directory: " + directory);
                directoryInfo.Delete();
                if (directoryInfo.Parent != null)
                {
                    var meta = Path.Combine(directoryInfo.Parent.FullName, directoryInfo.Name + ".meta");
                    if (File.Exists(meta))
                    {
                        File.Delete(meta);
                    }
                }
            }
        }
    }
}
#endif
