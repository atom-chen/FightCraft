using UnityEngine;
using System.Collections;

namespace GameBase
{
    public class GameObjectManager
    {
        #region 唯一

        private static GameObjectManager _Instance = null;
        public static GameObjectManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameObjectManager();
                }
                return _Instance;
            }
        }

        private GameObjectManager() { }

        #endregion

        #region 

        public GameObject GetEmptyObject(string name)
        {
            GameObject emptyGO = new GameObject(name);
            return emptyGO;
        }

        public GameObject GetObject(string path)
        {
            GameObject emptyGO = ResourceManager.Instance.GetGameObject(path);
            return emptyGO;
        }

        public GameObject GetInstantObject(string path)
        {
            GameObject tempGO = ResourceManager.Instance.GetGameObject(path);
            if (tempGO == null)
                return null;
            var go = GameObject.Instantiate(tempGO) as GameObject;
            return go;
        }

        public void DestoryGameObject(GameObject go)
        {
            GameObject.Destroy(go);
        }

        #endregion
    }
}
