using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIContainerBase : UIBase
{

    

    public int _InitShowCnt;
    public Transform _ContainerObj;
    public UIItemBase _ContainItemPre;

    protected List<UIItemBase> _ItemPrefabList = new List<UIItemBase>();
    protected List<object> _ValueList = new List<object>();
    protected UIItemBase.ItemClick _OnClickItem;

    private int _RealInitShowCnt;

    public virtual void InitContentItem(ICollection valueList, UIItemBase.ItemClick onClick = null, Hashtable exhash = null)
    {
        _ValueList.Clear();
        if (valueList == null)
            return;

        foreach (var itemValue in valueList)
        {
            _ValueList.Add(itemValue);
        }

        _RealInitShowCnt = Math.Min(_ValueList.Count, _InitShowCnt);

        _OnClickItem = onClick;

        if (_ValueList.Count > 0)
        {
            StartCoroutine(InitItems(exhash));
        }
        else
        {
            ClearPrefab();
        }

        //_LayoutGroup.enabled = false;

    }

    public virtual void ShowItems()
    {
        
    }

    public virtual void ShowItemsFinish()
    {

    }

    private void InitItem(object itemValue, UIItemBase preItem, Hashtable exhash)
    {
        preItem.gameObject.SetActive(true);
        //preItem.transform.localPosition = new Vector3(contentItem.Pos.x, contentItem.Pos.y, 0);
        //if (preItem._InitInfo != contentItem.Obj)
        {
            Hashtable hash;
            if (exhash == null)
            {
                hash = new Hashtable();
            }
            else
            {
                hash = new Hashtable(exhash);
            }
            hash.Add("InitObj", itemValue);
            preItem.Show(hash);
            preItem._InitInfo = itemValue;
            preItem._ClickEvent = _OnClickItem;
        }
    }

    public virtual void RefreshItems()
    {
        foreach (var item in _ItemPrefabList)
        {
            item.Refresh();
        }
    }

    public virtual void RefreshItems(Hashtable hash)
    {
        foreach (var item in _ItemPrefabList)
        {
            item.Refresh(hash);
        }
    }

    public void PreLoadItem(int preLoadCount)
    {
        InitItems(null);
    }

    private void ClearPrefab()
    {
        foreach (var prefab in _ItemPrefabList)
        {
            //prefab.transform.localPosition = new Vector3(1000, 1000, 1);
            prefab.Hide();
            prefab._InitInfo = null;
        }
    }

    private UIItemBase InstantiateItem()
    {
        UIItemBase uiItemBase = GameObject.Instantiate<UIItemBase>(_ContainItemPre);
        uiItemBase.transform.SetParent(_ContainerObj.transform);
        uiItemBase.transform.localScale = Vector3.one;
        uiItemBase.transform.localPosition = Vector3.zero;

        _ItemPrefabList.Add(uiItemBase);

        return uiItemBase;
    }

    private IEnumerator InitItems(Hashtable exhash)
    {
        if (_RealInitShowCnt > _ItemPrefabList.Count)
        {
            for (int i = _ItemPrefabList.Count; i < _RealInitShowCnt; ++i)
            {
                InstantiateItem();
            }
        }

        for (int i = 0; i < _RealInitShowCnt; ++i)
        {
            InitItem(_ValueList[i], _ItemPrefabList[i], exhash);
        }

        if (_RealInitShowCnt >= _ValueList.Count)
        {
            ShowItemsFinish();
            yield break;
        }

        yield return new WaitForFixedUpdate();

        int nextNeedCount = _ValueList.Count - _RealInitShowCnt;
        if (nextNeedCount > 0)
        {
            int needCreateCount = nextNeedCount - (_ItemPrefabList.Count - _RealInitShowCnt);
            for (int i = 0; i < needCreateCount; ++i)
            {
                InstantiateItem();
            }

            for (int i = _RealInitShowCnt; i < _ValueList.Count; ++i)
            {
                InitItem(_ValueList[i], _ItemPrefabList[i], exhash);
            }
        }

        if (_ItemPrefabList.Count > _ValueList.Count)
        {
            for (int i = _ValueList.Count; i < _ItemPrefabList.Count; ++i)
            {
                _ItemPrefabList[i].Hide();
            }
        }

        ShowItemsFinish();
    }

    public T GetContainItem<T>(int idx)
    {
        return _ItemPrefabList[idx].GetComponent<T>();
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

    public Vector3 GetObjItemPos(object obj)
    {
        foreach (var item in _ItemPrefabList)
        {
            if (item._InitInfo == obj)
            {
                return item.transform.position;
            }
        }
        return Vector3.zero;
    }
}

