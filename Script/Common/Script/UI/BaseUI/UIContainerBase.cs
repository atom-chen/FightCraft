using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameUI
{
    public class UIContainerBase : UIBase
    {
        public GameObject _ContainItemPre;
        public GameObject _ContainerObj;
        protected List<UIItemBase> _ItemPrefabList = new List<UIItemBase>();
        protected Dictionary<object, UIItemBase> _ActivedItems = new Dictionary<object, UIItemBase>();

        public virtual void InitContentItem(IEnumerator enumerator, Hashtable exhash = null, UIItemBase.ItemClick onClick = null)
        {
            enumerator.Reset();
            int preIdx = 0;
            _ActivedItems.Clear();

            InitItems();

            while (enumerator.MoveNext())
            {
                if (_ItemPrefabList.Count < preIdx + 1)
                {
                    GameObject obj = GameObject.Instantiate<GameObject>(_ContainItemPre);
                    obj.transform.parent = _ContainerObj.transform;
                    obj.transform.localScale = new Vector3(1,1,1);
                    var itemScript = obj.GetComponent<UIItemBase>();
                    
                    _ItemPrefabList.Add(itemScript);
                }

                Hashtable hash = new Hashtable();
                if (exhash != null)
                    hash = new Hashtable(exhash);

                hash.Add("InitObj", enumerator.Current);

                _ItemPrefabList[preIdx].Show(hash);
                _ItemPrefabList[preIdx]._InitInfo = enumerator.Current;
                _ItemPrefabList[preIdx]._ClickEvent = onClick;

                _ActivedItems.Add(enumerator.Current, _ItemPrefabList[preIdx]);
                ++preIdx;
            }
            

            if (_ItemPrefabList.Count > preIdx)
            {
                for (int i = preIdx; i < _ItemPrefabList.Count; ++i)
                {
                    _ItemPrefabList[i].Hide();
                }
            }
        }

        public void PreLoadItem(int preLoadCount)
        {
            InitItems();
            for (int i = 0; i < preLoadCount; ++i)
            {
                GameObject obj = GameObject.Instantiate<GameObject>(_ContainItemPre);
                obj.transform.parent = _ContainerObj.transform;
                obj.transform.localScale = new Vector3(1, 1, 1);
                var itemScript = obj.GetComponent<UIItemBase>();
                obj.SetActive(false);
                _ItemPrefabList.Add(itemScript);
            }
        }

        private void InitItems()
        {
            if (_ItemPrefabList.Count == 0)
            {
                _ContainerObj.GetComponentsInChildren<UIItemBase>(true, _ItemPrefabList);
            }
        }

        public T GetContainItem<T>(int idx)
        {
            return _ItemPrefabList[idx].GetComponent<T>();
        }

        public T GetContainItem<T>(object dataObj)
        {
            if (_ActivedItems.ContainsKey(dataObj))
                return _ActivedItems[dataObj].GetComponent<T>();
            return default(T);
        }

        public void ForeachActiveItem<T>(Action<T> action)
        {
            for (int i = 0; i < _ItemPrefabList.Count; ++i)
            {
                if (_ItemPrefabList[i].gameObject.activeSelf)
                {
                    action((_ItemPrefabList[i].gameObject.GetComponent<T>()));
                }
            }
        }
    }
}
