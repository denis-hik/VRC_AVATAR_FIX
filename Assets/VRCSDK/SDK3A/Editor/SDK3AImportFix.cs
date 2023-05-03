﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace VRC.SDK3A.Editor
{
    [InitializeOnLoad]
    public class SDK3AImportFix
    {
        private const string packageRuntimePluginsFolder = "Packages/com.vrchat.avatars/Runtime/VRCSDK/Plugins";
        private const string legacyRuntimePluginsFolder = "Assets/VRCSDK/Plugins/";
        private const string reloadPluginsKey = "ReloadPlugins";
        
        private static readonly HashSet<string> _samplesToImport = new HashSet<string>()
        {
            "AV3 Demo Assets",
            "Robot Avatar"
        };

        static SDK3AImportFix()
        {
            var reloadsUntilRun = SessionState.GetInt(reloadPluginsKey, 0);
            if (reloadsUntilRun > -1)
            {
                reloadsUntilRun--;
                if (reloadsUntilRun == 0)
                {
                    Run();
                }

                SessionState.SetInt(reloadPluginsKey, reloadsUntilRun);
            }
        }

        public static void Run(){
            if (Directory.Exists(packageRuntimePluginsFolder))
            {
                AssetDatabase.ImportAsset($"{packageRuntimePluginsFolder}/VRCSDK3A.dll",
                    ImportAssetOptions.ForceSynchronousImport);
                AssetDatabase.ImportAsset($"{packageRuntimePluginsFolder}/VRCSDK3A-Editor.dll",
                    ImportAssetOptions.ForceSynchronousImport);
            }
            else if (Directory.Exists(legacyRuntimePluginsFolder))
            {
                AssetDatabase.ImportAsset($"{legacyRuntimePluginsFolder}/VRCSDK3A.dll",
                    ImportAssetOptions.ForceSynchronousImport);
                AssetDatabase.ImportAsset($"{legacyRuntimePluginsFolder}/VRCSDK3A-Editor.dll",
                    ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}