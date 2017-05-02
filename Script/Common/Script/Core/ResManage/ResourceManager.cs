using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameBase
{
    public class ResourceManager
    {
        #region 唯一

        private static ResourceManager _Instance = null;
        public static ResourceManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ResourceManager();
                }
                return _Instance;
            }
        }

        private ResourceManager() { }

        #endregion

        #region 

        bool _IsFromBundle = false;

        public GameObject GetUI(string resName)
        {
            if (_IsFromBundle)
            {
                return GetUIBundle(resName);
            }
            else
            {
                return GetUIRes(resName);
            }
        }

        #endregion

        #region Assetbundle

        AssetBundle _UIBundle;
        public GameObject GetUIBundle(string path)
        {
            if (_UIBundle == null)
            {
                string mUrl = GetStreamingAssetsPath() + "ui";
                WWW www = WWW.LoadFromCacheOrDownload(mUrl, 1);
                _UIBundle = www.assetBundle;
            }

            string name = GetFileNameFromPath(path);
            GameObject tempGO = _UIBundle.LoadAsset(name) as GameObject;
            return tempGO;
        }

        public string GetFileNameFromPath(string path)
        {
            int lastPathIdx = path.LastIndexOf("/");
            string name = path;
            if (lastPathIdx > 0)
                name = path.Substring(lastPathIdx + 1);
            return name;
        }

        public static string GetStreamingAssetsPath()
        {
            string filepath = "";
#if UNITY_EDITOR
            filepath = "file:///" + Application.dataPath + "/StreamingAssets/";
            Debug.Log("editor");
#elif UNITY_STANDALONE_WIN
        filepath = "file:///" + Application.dataPath + "/StreamingAssets/";
        Debug.Log("UNITY_STANDALONE_WIN");
#elif UNITY_IPHONE
	    filepath = Application.dataPath +"/Raw/";
        Debug.Log("IPHONE");
#elif UNITY_ANDROID
	    filepath = "jar:file://" + Application.dataPath + "!/assets/";
        Debug.Log("android");
#endif
            return filepath;
        }

        #endregion

        #region ResourceFile

        public const string RES_SPRITE_PATH = "Sprite/";
        public const string RES_PREFAB_PATH = "Prefab/";
        public const string RES_TABLE_PATH = "Tables/";
        public const string RES_EFFECT_PATH = "Effect/";
        public const string RES_UI_PATH = "UI/";
        public const string RES_AUDIO_PATH = "Audios/";
        public const string RES_TEXTURE_PATH = "Texture/";

        public Sprite GetSprite(string resName)
        {
            string resPath = RES_SPRITE_PATH + resName;
            var resource = Resources.Load<Sprite>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
            }
            return resource;
        }

        public Texture GetTexture(string resName)
        {
            string resPath = RES_TEXTURE_PATH + resName;
            var resource = Resources.Load<Texture>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
            }
            return resource;
        }

        public GameObject GetGameObject(string resPath)
        {
            //string resPath = RES_PREFAB_PATH + resName;
            var resource = Resources.Load<GameObject>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
            }
            return resource;
        }

        public GameObject GetInstanceGameObject(string resPath)
        {
            //string resPath = RES_PREFAB_PATH + resName;
            var resource = Resources.Load<GameObject>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
                return null;
            }
            var instanceGO = GameObject.Instantiate<GameObject>(resource);
            return instanceGO;
        }

        public string GetTable(string resName)
        {
            string resPath = RES_TABLE_PATH + resName;
            var resource = Resources.Load<TextAsset>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
            }
            return resource.text;
        }

        public GameObject GetEffect(string resName)
        {
            string resPath = RES_EFFECT_PATH + resName;
            var resource = Resources.Load<GameObject>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
            }
            return resource;
        }

        public GameObject GetUIRes(string resName)
        {
            string resPath = RES_UI_PATH + resName;
            var resource = Resources.Load<GameObject>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
            }
            return resource;
        }

        public AudioClip GetAudioClip(string resName)
        {
            string resPath = RES_AUDIO_PATH + resName;
            var resource = Resources.Load<AudioClip>(resPath);
            if (resource == null)
            {
                LogManager.LogError("Resource error:" + resPath);
            }
            return resource;
        }

        public void DestoryObj(GameObject obj)
        {
            GameObject.Destroy(obj);
        }
        #endregion
    }
}
