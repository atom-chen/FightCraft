using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace AssetBundles
{
	public class BuildScript
	{
		public static string overloadedDevelopmentServerURL = "";
	
		public static void BuildAssetBundles()
		{
			// Choose the output path according to the build target.
			string outputPath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
			if (!Directory.Exists(outputPath) )
				Directory.CreateDirectory (outputPath);
	
			//@TODO: use append hash... (Make sure pipeline works correctly with it.)
			BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.DisableWriteTypeTree, EditorUserBuildSettings.activeBuildTarget);
            Debug.Log("BuildPipeline.BuildAssetBundles finish");

            WriteUpdate(outputPath);
        }
	
		public static void WriteServerURL()
		{
			string downloadURL;
			if (string.IsNullOrEmpty(overloadedDevelopmentServerURL) == false)
			{
				downloadURL = overloadedDevelopmentServerURL;
			}
			else
			{
				IPHostEntry host;
				string localIP = "";
				host = Dns.GetHostEntry(Dns.GetHostName());
				foreach (IPAddress ip in host.AddressList)
				{
					if (ip.AddressFamily == AddressFamily.InterNetwork)
					{
						localIP = ip.ToString();
						break;
					}
				}
				downloadURL = "http://"+localIP+":7888/";
			}
			
			string assetBundleManagerResourcesDirectory = "Assets/AssetBundleManager/Resources";
			string assetBundleUrlPath = Path.Combine (assetBundleManagerResourcesDirectory, "AssetBundleServerURL.bytes");
			Directory.CreateDirectory(assetBundleManagerResourcesDirectory);
			File.WriteAllText(assetBundleUrlPath, downloadURL);
			AssetDatabase.Refresh();
		}
	
		public static void BuildPlayer()
		{
			var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
			if (outputPath.Length == 0)
				return;
	
			string[] levels = GetLevelsFromBuildSettings();
			if (levels.Length == 0)
			{
				Debug.Log("Nothing to build.");
				return;
			}
	
			string targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
			if (targetName == null)
				return;
	
			// Build and copy AssetBundles.
			BuildScript.BuildAssetBundles();
			WriteServerURL();
	
			BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
			BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
		}
		
		public static void BuildStandalonePlayer()
		{
			var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
			if (outputPath.Length == 0)
				return;
			
			string[] levels = GetLevelsFromBuildSettings();
			if (levels.Length == 0)
			{
				Debug.Log("Nothing to build.");
				return;
			}
			
			string targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
			if (targetName == null)
				return;
			
			// Build and copy AssetBundles.
			BuildScript.BuildAssetBundles();
			BuildScript.CopyAssetBundlesTo(Path.Combine(Application.streamingAssetsPath, Utility.AssetBundlesOutputPath) );
			AssetDatabase.Refresh();
			
			BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
			BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
		}
	
		public static string GetBuildTargetName(BuildTarget target)
		{
			switch(target)
			{
			case BuildTarget.Android :
				return "/test.apk";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "/test.exe";
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
			case BuildTarget.StandaloneOSX:
				return "/test.app";
			case BuildTarget.WebGL:
				return "";
				// Add more build targets for your own.
			default:
				Debug.Log("Target not implemented.");
				return null;
			}
		}
	
		static void CopyAssetBundlesTo(string outputPath)
		{
			// Clear streaming assets folder.
			FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
			Directory.CreateDirectory(outputPath);
	
			string outputFolder = Utility.GetPlatformName();
	
			// Setup the source folder for assetbundles.
			var source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, Utility.AssetBundlesOutputPath), outputFolder);
			if (!System.IO.Directory.Exists(source) )
				Debug.Log("No assetBundle output folder, try to build the assetBundles first.");
	
			// Setup the destination folder for assetbundles.
			var destination = System.IO.Path.Combine(outputPath, outputFolder);
			if (System.IO.Directory.Exists(destination) )
				FileUtil.DeleteFileOrDirectory(destination);
			
			FileUtil.CopyFileOrDirectory(source, destination);
		}
	
		static string[] GetLevelsFromBuildSettings()
		{
			List<string> levels = new List<string>();
			for(int i = 0 ; i < EditorBuildSettings.scenes.Length; ++i)
			{
				if (EditorBuildSettings.scenes[i].enabled)
					levels.Add(EditorBuildSettings.scenes[i].path);
			}
	
			return levels.ToArray();
		}

        #region for asset update

        public static string VersionFold = "versionData";
        public static string VersionFileName = "ResVersion.txt";
        public static string ResFileListName = "update.info";

        public static void WriteUpdate(string bundleFold)
        {
            string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();

            //string versionFoldPath = Path.Combine(bundleFold, VersionFold);
            //if (!Directory.Exists(versionFoldPath))
            //{
            //    Directory.CreateDirectory(versionFoldPath);
            //}

            string updateFilePath = Path.Combine(bundleFold, ResFileListName);
            if (File.Exists(updateFilePath))
            {
                File.Delete(updateFilePath);
            }

            StreamWriter streamWriter = new StreamWriter(updateFilePath);

            WriteManifest(streamWriter);
            foreach (var bundleFile in bundleNames)
            {
                if (!bundleFile.Contains("."))
                    continue;

                string bundlePath = Path.Combine(bundleFold, bundleFile);
                string md5 = /*GCGame.Utils.GetMD5Hash(bundlePath)*/"";

                FileInfo fileInfo = new FileInfo(bundlePath);
                long size = fileInfo.Length;

                streamWriter.WriteLine(bundleFile + ":" + md5 + ":" + size);
            }
            streamWriter.Close();

            int versionValue = 0;

            string versionFilePath = Path.Combine(bundleFold, VersionFileName);

            StreamReader versionReader = new StreamReader(versionFilePath);
            if (versionReader != null)
            {
                string versionStr = versionReader.ReadLine();
                versionReader.Close();

                File.Delete(versionFilePath);


                if (!string.IsNullOrEmpty(versionStr))
                {
                    int.TryParse(versionStr, out versionValue);
                }
                ++versionValue;
            }

            StreamWriter versionWriter = new StreamWriter(versionFilePath);
            versionWriter.WriteLine(versionValue.ToString());
            versionWriter.Close();
        }

        public static void WriteManifest(StreamWriter streamWriter)
        {
            string sourceFile = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName()) + "/" + Utility.GetPlatformName();
            string destFile = sourceFile + ".common";
            string fileName = Utility.GetPlatformName() + ".common";

            if(File.Exists(destFile))
                File.Delete(destFile);
            File.Copy(sourceFile, destFile);

            string md5 = /*GCGame.Utils.GetMD5Hash(destFile)*/ "";

            FileInfo fileInfo = new FileInfo(destFile);
            long size = fileInfo.Length;

            streamWriter.WriteLine(fileName + ":" + md5 + ":" + size);


        }


        public static string ReadCRC(string manifestFilePath)
        {
            StreamReader streamReader = new StreamReader(manifestFilePath);
            if (streamReader == null)
                return "";

            streamReader.ReadLine();
            string crcLine = streamReader.ReadLine();
            string[] crcSplits = crcLine.Split(':');

            if (crcSplits.Length < 2)
                return "";

            streamReader.Close();
            return crcSplits[1].Trim(' ', '\t');
        }

        #endregion
    }
}