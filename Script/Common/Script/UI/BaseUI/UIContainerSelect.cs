using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIContainerSelect : UIContainerBase
{
    public bool _IsMultiSelect = false;
    private List<ContentPos> _Selecteds = new List<ContentPos>();

    public T GetSelected<T>()
    {
        if (!_IsMultiSelect && _Selecteds.Count > 0)
        {
            return (T)_Selecteds[0].Obj;
        }
        return default(T);
    }

    public List<T> GetSelecteds<T>()
    {
        List<T> selectedObjs = new List<T>();
        foreach (var selectedPos in _Selecteds)
        {
            selectedObjs.Add((T)selectedPos.Obj);
        }
        return selectedObjs;
    }

    public delegate void SelectedObjCallBack(object obj);
    private SelectedObjCallBack _SelectedCallBack;
    private SelectedObjCallBack _DisSelectedCallBack;

    public void OnSelectedObj(object obj)
    {
        ContentPos selectPos = _ValueList.Find((pos) =>
        {
            if (pos.Obj == obj)
                return true;
            return false;
        });

        if (selectPos == null)
            return;

        if (!((UIItemSelect)selectPos.ShowItem).IsCanSelect())
            return;

        if (_IsMultiSelect)
        {
            if (_Selecteds.Contains(selectPos))
            {
                if (selectPos.ShowItem != null)
                {
                    ((UIItemSelect)selectPos.ShowItem).UnSelected();
                }
                _Selecteds.Remove(selectPos);
            }
            else
            {
                

                if (selectPos.ShowItem != null)
                {
                    ((UIItemSelect)selectPos.ShowItem).Selected();
                }
                _Selecteds.Add(selectPos);
            }
        }
        else
        {
            if (_Selecteds.Count > 0)
            {
                //if (_Selecteds[0] == selectPos)
                //    return;

                ((UIItemSelect)_Selecteds[0].ShowItem).UnSelected();
            }
            _Selecteds.Clear();

            _Selecteds.Add(selectPos);
            ((UIItemSelect)_Selecteds[0].ShowItem).Selected();
        }

        if (_SelectedCallBack != null && _Selecteds.Contains(selectPos))
        {
            _SelectedCallBack(obj);
        }
        else if (_DisSelectedCallBack != null && !_Selecteds.Contains(selectPos))
        {
            _DisSelectedCallBack(obj);
        }
    }

    #region 

    public void InitSelectContent(IEnumerable list, IEnumerable selectedList, SelectedObjCallBack onSelect = null, SelectedObjCallBack onDisSelect = null, Hashtable exhash = null)
    {
        _SelectedCallBack = onSelect;
        _DisSelectedCallBack = onDisSelect;

        base.InitContentItem(list, OnSelectedObj, exhash);

        _Selecteds.Clear();
        if (selectedList != null)
        {
            foreach (var selectItem in selectedList)
            {
                ContentPos selectPos = _ValueList.Find((pos) =>
                {
                    if (pos.Obj.Equals(selectItem))
                        return true;
                    return false;
                });
                if (selectPos != null)
                {
                    _Selecteds.Add(selectPos);
                }
            }
        }
        ShowItems();
    }

    public void ClearSelect()
    {
        foreach (var selectPos in _Selecteds)
        {
            ((UIItemSelect)selectPos.ShowItem).UnSelected();
            if (_DisSelectedCallBack != null)
            {
                _DisSelectedCallBack(selectPos.Obj);
            }

        }

        _Selecteds.Clear();
    }

    public void SetSelect(ICollection selectedList)
    {
        _Selecteds.Clear();
        if (selectedList != null)
        {
            foreach (var selectItem in selectedList)
            {
                ContentPos selectPos = _ValueList.Find((pos) =>
                {
                    if (pos.Obj == selectItem)
                        return true;
                    return false;
                });
                if (selectPos != null)
                    _Selecteds.Add(selectPos);
            }
        }

        foreach (var shoItem in _ItemPrefabList)
        {
            var showObj = shoItem._InitInfo;
            ContentPos selectPos = _Selecteds.Find((pos) =>
            {
                if (pos.Obj == showObj)
                    return true;
                return false;
            });

            if (selectPos != null)
            {
                ((UIItemSelect)shoItem).Selected();
                if (_SelectedCallBack != null)
                {
                    _SelectedCallBack(selectPos.Obj);
                }
            }
            else
            {
                ((UIItemSelect)shoItem).UnSelected();
            }
        }
    }

    public override void ShowItemsFinish()
    {
        int index = 0;
        foreach (var shoItem in _ItemPrefabList)
        {
            var showObj = shoItem._InitInfo;
            ContentPos selectPos = _Selecteds.Find((pos) =>
            {
                if (pos.Obj == showObj)
                    return true;
                return false;
            });

            if (selectPos != null)
            {
                ((UIItemSelect)shoItem).Selected();
                if (_SelectedCallBack != null)
                {
                    _SelectedCallBack(selectPos.Obj);
                }
                if (index > 0)
                {
                    StartCoroutine(ShowSelectContainPos(shoItem));
                }
            }
            else
            {
                ((UIItemSelect)shoItem).UnSelected();
            }
            ++index;
        }
    }

    private IEnumerator ShowSelectContainPos(UIItemBase selectPos)
    {
        yield return new WaitForFixedUpdate();

        if (_ScrollRect != null)
        {
            if (_ScrollRect.horizontal == true)
            {
                float containerMaxX = _ContainerObj.sizeDelta.x;
                float containPosX = -selectPos.GetComponent<RectTransform>().anchoredPosition.x - _ScrollTransform.rect.width * 0.5f;
                containPosX = Mathf.Clamp(containPosX, 0, containerMaxX - _ScrollTransform.sizeDelta.x);
                _ContainerObj.anchoredPosition = new Vector2(containPosX, _ContainerObj.anchoredPosition.y);
            }

            if (_ScrollRect.vertical == true)
            {
                float containerMaxY = _ContainerObj.sizeDelta.y;
                float containPosY = -selectPos.GetComponent<RectTransform>().anchoredPosition.y - _ScrollTransform.rect.height * 0.5f;
                containPosY = Mathf.Clamp(containPosY, 0, containerMaxY - _ScrollTransform.sizeDelta.y);
                Debug.Log("containPosY:" + containPosY);
                _ContainerObj.anchoredPosition = new Vector2(_ContainerObj.anchoredPosition.x, containPosY);
            }
        }
    }

    public override void ShowItems()
    {
        base.ShowItems();

       
    }

    #endregion


}

