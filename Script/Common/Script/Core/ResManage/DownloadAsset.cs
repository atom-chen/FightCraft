using UnityEngine;
using System.Collections;

namespace GameBase
{
    public class DownloadAsset
    {
        #region 唯一

        private static DownloadAsset _Instance = null;
        public static DownloadAsset Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DownloadAsset();
                }
                return _Instance;
            }
        }

        private DownloadAsset() { }

        #endregion


//        public T GetAsset<T>(string name) where T : Object
//        {
//            Object obj = null;

//            return obj as T;
//        }

//        public void DownloadAsset<T>(string asset) where T : Object
//        {
//            string wwwFile =
//#if UNITY_ANDROID && !UNITY_EDITOR
//             Application.streamingAssetsPath + "/"+ filePath;
//#else
//            @"file://" + Application.streamingAssetsPath + "/" + asset;
//#endif
//            using (WWW www = new  WWW(wwwFile))
//            {
//                //return www;
//            }
//        }

    }
}
