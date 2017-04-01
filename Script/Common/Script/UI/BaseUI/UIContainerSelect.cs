using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIContainerSelect : UIContainerBase
{
    public bool _IsMultiSelect = false;

    private List<object> _Selecteds = new List<object>();

    public T GetSelected<T>()
    {
        if (!_IsMultiSelect && _Selecteds.Count > 0)
        {
            return (T)_Selecteds[0];
        }
        return default(T);
    }

    public List<T> GetSelecteds<T>()
    {
        List<T> selectedObjs = new List<T>();
        foreach (var selectedPos in _Selecteds)
        {
            selectedObjs.Add((T)selectedPos);
        }
        return selectedObjs;
    }

    public delegate void SelectedObjCallBack(object obj);
    private SelectedObjCallBack _SelectedCallBack;
    private SelectedObjCallBack _DisSelectedCallBack;

    public void OnSelectedObj(object obj)
    {
        int idx = _ValueList.IndexOf(obj);
        if (idx < 0)
            return;

        if (!((UIItemSelect)_ItemPrefabList[idx]).IsCanSelect())
            return;

        var selectedItem = ((UIItemSelect)_ItemPrefabList[idx]);
        if (_IsMultiSelect)
        {
            if (_Selecteds.Contains(obj))
            {
                selectedItem.UnSelected();
                _Selecteds.Remove(obj);
            }
            else
            {

                selectedItem.Selected();
                _Selecteds.Add(obj);
            }
        }
        else
        {
            if (_Selecteds.Count > 0)
            {
                int idxLast = _ValueList.IndexOf(_Selecteds[0]);
                ((UIItemSelect)_ItemPrefabList[idxLast]).UnSelected();
            }
            _Selecteds.Clear();

            _Selecteds.Add(obj);
            selectedItem.Selected();
        }

        if (_SelectedCallBack != null && _Selecteds.Contains(obj))
        {
            _SelectedCallBack(obj);
        }
        else if (_DisSelectedCallBack != null && !_Selecteds.Contains(obj))
        {
            _DisSelectedCallBack(obj);
        }
    }

    #region 

    public void InitSelectContent(ICollection list, ICollection selectedList, SelectedObjCallBack onSelect = null, SelectedObjCallBack onDisSelect = null, Hashtable exhash = null)
    {
        _SelectedCallBack = onSelect;
        _DisSelectedCallBack = onDisSelect;

        base.InitContentItem(list, OnSelectedObj, exhash);

        _Selecteds.Clear();
        if (selectedList != null)
        {
            foreach (var selectItem in selectedList)
            {
                _Selecteds.Add(selectItem);
            }
        }
        ShowItems();
    }

    public void ClearSelect()
    {
        foreach (var selectObj in _Selecteds)
        {
            int idx = _ValueList.IndexOf(selectObj);
            var selectedItem = ((UIItemSelect)_ItemPrefabList[idx]);

            selectedItem.UnSelected();
            if (_DisSelectedCallBack != null)
            {
                _DisSelectedCallBack(selectObj);
            }
        }

        _Selecteds.Clear();
    }
    
    public override void ShowItems()
    {
        base.ShowItems();

       
    }

    #endregion


}

