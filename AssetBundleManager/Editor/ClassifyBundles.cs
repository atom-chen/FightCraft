using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ClassifyBundles : MonoBehaviour
{
    public static string ASSET_PATH = "/Project3D/BundleData";

    public static string BUNDLE_MAIN_BASE_UI = "MainBase";

    [MenuItem("ProTool/ClassifyBundles/Scene")]
    public static void ClassifySceneBundles()
    {
        string sysPath = Application.dataPath + "/FightCraft/Res/Scenes";
        string[] filePaths = Directory.GetFiles(sysPath, "*.unity", SearchOption.AllDirectories);

        Dictionary<string, List<string>> resCallTimes = new Dictionary<string, List<string>>();

        List<string> bundleNameList = new List<string>();
        foreach (var file in filePaths)
        {
            if (file.Contains("login") || file.Contains("CeShi"))
            {//登陆场景不进bundle
                continue;
            }

            string dataFile = file.Replace(Application.dataPath, "Assets");
            string fileName = Path.GetFileNameWithoutExtension(file).ToLower();
            string bundleName = "scene/" + fileName;
            bundleNameList.Add(fileName);
            var prefabImporter = AssetImporter.GetAtPath(dataFile);
            if (prefabImporter != null)
            {
                prefabImporter.assetBundleName = "scene/" + fileName.ToLower();
                prefabImporter.assetBundleVariant = "common";
            }

            Object assetData = AssetDatabase.LoadAssetAtPath<Object>(dataFile);
            Object[] dependObjs = EditorUtility.CollectDependencies(new Object[1] { assetData });
            foreach (var dependObj in dependObjs)
            {
                if (dependObj == null)
                    continue;

                if (dependObj is GameObject)
                {
                    string dependObjPath = AssetDatabase.GetAssetPath(dependObj);
                    if (resCallTimes.ContainsKey(dependObjPath))
                    {
                        if (resCallTimes[dependObjPath].Contains(file))
                        {
                            continue;
                        }
                        else
                        {
                            resCallTimes[dependObjPath].Add(file);
                        }
                    }
                    else
                    {
                        resCallTimes.Add(dependObjPath, new List<string>() { file });
                    }

                    if (dependObj is Texture2D)
                    {
                        SetTextureImporter(dependObj);
                    }
                }
            }
        }

        foreach (var dependObj in resCallTimes)
        {
            if (dependObj.Key.EndsWith("meta") || dependObj.Key.EndsWith("cs"))
                continue;

            //if (dependObj.Value.Count == 1)
            //    continue;

            Object assetData = AssetDatabase.LoadAssetAtPath<Object>(dependObj.Key);
            if (bundleNameList.Contains(assetData.name))
            {
                Debug.LogError("Res has the same name:" + dependObj.Key);
            }
            bundleNameList.Add(assetData.name);
            ClassifySceneModel(assetData);

        }
    }

    public static void ClassifySceneModel(Object sceneModel)
    {
        string bundleName = "scene/depend_" + sceneModel.name;

        Object[] dependObjs = EditorUtility.CollectDependencies(new Object[1] { sceneModel });
        foreach (var dependObj in dependObjs)
        {
            if (dependObj is UnityEngine.Mesh
                        || dependObj is Texture2D
                        || dependObj is Material
                        || dependObj is Animator
                        || dependObj is RuntimeAnimatorController)
            {
                string dependObjPath = AssetDatabase.GetAssetPath(dependObj);
                var dependImporter = AssetImporter.GetAtPath(dependObjPath);

                if (dependImporter != null)
                {

                    dependImporter.assetBundleName = bundleName;
                    dependImporter.assetBundleVariant = "common";
                }

                //AssetDatabase.ImportAsset(dependObjPath, ImportAssetOptions.ForceUpdate);
            }
        }
    }
    
    [MenuItem("ProTool/ClassifyBundles/ClearAll")]
    public static void ClearAllBundleName()
    {
        string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (var bundleName in bundleNames)
        {
            //if (bundleName == ("ui/countdown.common"))
            {
                string[] bundleAssets = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
                foreach (var bundleAsset in bundleAssets)
                {
                    var prefabImporter = AssetImporter.GetAtPath(bundleAsset);
                    prefabImporter.assetBundleVariant = "";
                    prefabImporter.assetBundleName = "";
                }
            }
        }
    }

    private static void ClassifySingleFile(string[] files, string className)
    {
        foreach (var file in files)
        {
            string dataFile = file.Replace(Application.dataPath, "Assets");
            Object assetData = AssetDatabase.LoadAssetAtPath<Object>(dataFile);
            Object[] dependObjs = EditorUtility.CollectDependencies(new Object[1] { assetData });
            string prefabPath = AssetDatabase.GetAssetPath(assetData);
            var prefabImporter = AssetImporter.GetAtPath(prefabPath);
            prefabImporter.assetBundleName = className + "/" + assetData.name;
            prefabImporter.assetBundleVariant = "common";

            foreach (var dependObj in dependObjs)
            {
                if (dependObj is TextAsset)
                    continue;

                string dependObjPath = AssetDatabase.GetAssetPath(dependObj);
                var dependImporter = AssetImporter.GetAtPath(dependObjPath);
                if (dependImporter != null /*&& string.IsNullOrEmpty(dependImporter.assetBundleName)*/)
                {
                    dependImporter.assetBundleName = className + "/" + assetData.name;
                    dependImporter.assetBundleVariant = "common";
                }
            }

        }
    }

    private static void ClassifyAllDependFile(string[] files, string className)
    {
        foreach (var file in files)
        {
            string dataFile = file.Replace(Application.dataPath, "Assets");
            Object assetData = AssetDatabase.LoadAssetAtPath<Object>(dataFile);
            Object[] dependObjs = EditorUtility.CollectDependencies(new Object[1] { assetData });
            string prefabPath = AssetDatabase.GetAssetPath(assetData);
            var prefabImporter = AssetImporter.GetAtPath(prefabPath);
            prefabImporter.assetBundleName = className;
            prefabImporter.assetBundleVariant = "common";

            foreach (var dependObj in dependObjs)
            {
                if (dependObj is TextAsset)
                    continue;

                string dependObjPath = AssetDatabase.GetAssetPath(dependObj);
                var dependImporter = AssetImporter.GetAtPath(dependObjPath);
                if (dependImporter != null /*&& string.IsNullOrEmpty(dependImporter.assetBundleName)*/)
                {
                    dependImporter.assetBundleName = className;
                    dependImporter.assetBundleVariant = "common";
                }
            }

        }
    }

    private static void ClassifyAllFile(string[] files, string bundleName)
    {
        foreach (var file in files)
        {
            string dataFile = file.Replace(Application.dataPath, "Assets");
            var prefabImporter = AssetImporter.GetAtPath(dataFile);
            if (prefabImporter != null)
            {
                prefabImporter.assetBundleName = bundleName;
                prefabImporter.assetBundleVariant = "common";
            }
        }
    }

    private static void ClassifyAllSingleFile(string[] files, string path)
    {
        foreach (var file in files)
        {
            string dataFile = file.Replace(Application.dataPath, "Assets");
            string fileName = Path.GetFileNameWithoutExtension(file).ToLower();
            var prefabImporter = AssetImporter.GetAtPath(dataFile);
            if (prefabImporter != null)
            {
                prefabImporter.assetBundleName = path + "/" + fileName;
                prefabImporter.assetBundleVariant = "common";
            }
        }
    }

    #region 

    private const int MAXPER = 512;

    private static void SetTextureImporter(Object dependObj)
    {
        string filePath = AssetDatabase.GetAssetPath(dependObj);
        var importer = AssetImporter.GetAtPath(filePath);
        TextureImporter resImporter = importer as TextureImporter;
        if (resImporter == null)
            return;

        resImporter.mipmapEnabled = false;

        AssetDatabase.ImportAsset(importer.assetPath, ImportAssetOptions.ForceUpdate);
    }

    private static Dictionary<Object, string> ClassDependObj(Dictionary<Object, List<string>> dependObjs)
    {
        Dictionary<Object, string> classedObjs = new Dictionary<Object, string>();
        int independCount = 1;

        foreach (var dependObj in dependObjs)
        {
            if (dependObj.Value.Count == 1)
                continue;

            if (classedObjs.ContainsKey(dependObj.Key))
                continue;

            classedObjs.Add(dependObj.Key, "dependIdx" + independCount);
            foreach (var otherDependObj in dependObjs)
            {
                if (dependObj.Key == otherDependObj.Key)
                    continue;

                if (dependObj.Value.Count != otherDependObj.Value.Count)
                    continue;

                bool isSameRef = true;
                foreach (string refStr in dependObj.Value)
                {
                    if (!otherDependObj.Value.Contains(refStr))
                    {
                        isSameRef = false;
                        break;
                    }
                }

                if (isSameRef)
                {
                    classedObjs.Add(otherDependObj.Key, "dependIdx" + independCount);
                }
            }
            ++independCount;

        }

        return classedObjs;
    }

    private static Dictionary<string, string> ClassDependObjCommon(Dictionary<string, List<string>> dependObjs)
    {
        List<List<string>> commonConflict = new List<List<string>>();
        commonConflict.Add(new List<string>());

        Dictionary<string, string> classedObjs = new Dictionary<string, string>();
        int independCount = 1;

        Dictionary<string, int> dependFold = new Dictionary<string, int>();
        int effectDependIdx = 0;
        foreach (var dependObj in dependObjs)
        {
            if (dependObj.Value.Count == 1)
                continue;

            if (classedObjs.ContainsKey(dependObj.Key))
                continue;

            if (string.IsNullOrEmpty(dependObj.Key))
                continue;

            if (dependObj.Key == "Resources" || dependObj.Key == "Library")
                continue;

            string dependObjFold = Path.GetDirectoryName(dependObj.Key);
            int dependIdx = 0;
            if (dependFold.ContainsKey(dependObjFold))
            {
                dependIdx = dependFold[dependObjFold];
            }
            else
            {
                dependIdx = effectDependIdx;
                dependFold.Add(dependObjFold, effectDependIdx);
                ++effectDependIdx;
            }

   
            classedObjs.Add(dependObj.Key, "depend_" + dependIdx);
            

        }

        return classedObjs;
    }

    #endregion
}
