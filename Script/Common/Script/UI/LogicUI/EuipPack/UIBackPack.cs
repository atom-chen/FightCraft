using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIBackPack : UIBase, IDragablePack
{
    #region static

    public static UIBackPack GetUIBackPackInstance(Transform parentTrans)
    {
        var tempGO = ResourceManager.Instance.GetUI("LogicUI/BagPack/UIBackPack");
        if (tempGO != null)
        {
            var uiGO = GameObject.Instantiate(tempGO);

            uiGO.transform.SetParent(parentTrans);
            uiGO.transform.localPosition = Vector3.zero;
            uiGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            uiGO.transform.localScale = Vector3.one;

            var backPack = uiGO.GetComponent<UIBackPack>();
            return backPack;
        }
        return null;
    }

    #endregion

    public delegate void OnSelectItem(ItemBase itemBase);
    public OnSelectItem _OnItemSelectCallBack;

    public delegate void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem);
    public OnDragItem _OnDragItemCallBack;

    public delegate bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem);
    public IsCanDropItem _IsCanDropItemCallBack;

    #region 

    public UITagPanel _TagPanel;
    public UIContainerBase _ItemsContainer;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        OnShowPage(0);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnShowPage(int page)
    {
        Hashtable hash = new Hashtable();
        hash.Add("DragPack", this);
        if (page == 0)
        {
            _ItemsContainer.InitContentItem(BackBagPack.Instance.PageEquips, ShowBackPackTooltips, hash);
        }
        else
        {
            _ItemsContainer.InitContentItem(BackBagPack.Instance.PageItems, ShowBackPackTooltips, hash);
        }

    }

    public void RefreshItems()
    {
        _ItemsContainer.RefreshItems();
    }

    private void ShowBackPackTooltips(object equipObj)
    {
        ItemBase equipItem = equipObj as ItemBase;
        _OnItemSelectCallBack.Invoke(equipItem);
    }


    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }



    #endregion

    #region 

    public bool IsCanDragItem(UIDragableItemBase dragItem)
    {
        if (!dragItem.ShowedItem.IsVolid())
            return false;

        return true;
    }

    void IDragablePack.OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if ((dragItem._DragPackBase == this && dropItem._DragPackBase == this))
        {
            dragItem.ShowedItem.ExchangeInfo(dropItem.ShowedItem);
            dragItem.Refresh();
            dropItem.Refresh();
            return;
        }

        if (_OnDragItemCallBack != null)
        {
            _OnDragItemCallBack(dragItem, dropItem);
        }
    }

    bool IDragablePack.IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if ((dragItem._DragPackBase == this && dropItem._DragPackBase == this))
        {
            return true;
        }

        if (_IsCanDropItemCallBack != null)
        {
            return _IsCanDropItemCallBack(dragItem, dropItem);
        }
        return true;
    }

    #endregion

}

