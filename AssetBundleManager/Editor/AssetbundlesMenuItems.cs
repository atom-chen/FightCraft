using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace AssetBundles
{
	public class AssetBundlesMenuItems
	{
		const string kSimulationMode = "Assets/AssetBundles/Simulation Mode";

#if UNITY_EDITOR


        [MenuItem(kSimulationMode)]
        public static void ToggleSimulationMode()
        {
            AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundleManager.SimulateAssetBundleInEditor;
        }

        [MenuItem(kSimulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(kSimulationMode, AssetBundleManager.SimulateAssetBundleInEditor);
            return true;
        }

        [MenuItem ("Assets/AssetBundles/Build AssetBundles")]
		static public void BuildAssetBundles ()
		{
			BuildScript.BuildAssetBundles();
		}

        [MenuItem("Assets/AssetBundles/ReBuild All AssetBundles")]
        static public void ReBuildAssetBundles()
        {
            string outputPath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            if (Directory.Exists(outputPath))
            {
                DirectoryInfo di = new DirectoryInfo(outputPath);
                di.Delete(true);
            }

            BuildScript.BuildAssetBundles();
        }
#endif
    }
}