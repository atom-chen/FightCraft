using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class UILegendaryPack : UIBase,IDragablePack
{

    #region 

    public UIContainerBase _LegencyContainer;
    public UIEquipPack _EquipPack;
    public Text _Attr1;
    public Text _Attr2;

    public void OnEnable()
    {
        _EquipPack._BackPack._OnItemSelectCallBack = ShowBackPackSelectItem;
        _EquipPack._BackPack._OnDragItemCallBack = OnDragItem;
        _EquipPack._BackPack._IsCanDropItemCallBack = IsCanDropItem;

        ShowPackItems();
    }

    public void OnDisable()
    {
        _EquipPack._BackPack._OnItemSelectCallBack = _EquipPack.ShowBackPackSelectItem;
        _EquipPack._BackPack._OnDragItemCallBack = _EquipPack.OnDragItem;
        _EquipPack._BackPack._IsCanDropItemCallBack = _EquipPack.IsCanDropItem;
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        _LegencyContainer.InitContentItem(LegendaryData.Instance._LegendaryEquipDict.Keys, ShowLegendaryPackTooltips, exHash);
        _EquipPack._BackPack.Show(null);
        RefreshAttrs();
    }

    private void RefreshAttrs()
    {
        _Attr1.text = LegendaryData.Instance.ExAttrs[0].GetAttrStr();
        _Attr2.text = LegendaryData.Instance.ExAttrs[1].GetAttrStr();
    }

    public void RefreshEquipItems()
    {
        _LegencyContainer.RefreshItems();
        _EquipPack._BackPack.RefreshItems();
    }

    public void ShowBackPackSelectItem(ItemBase itemObj)
    {
        ItemEquip equipItem = itemObj as ItemEquip;
        if (equipItem != null && equipItem.IsVolid() && LegendaryData.IsEquipLegendary(equipItem))
        {
            if (LegendaryData.IsEquipLegendary(equipItem))
            {
                UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10010, PutInEquip) });
            }
            else
            {
                UIEquipTooltips.ShowAsyn(equipItem);
            }
        }
        else if (itemObj.IsVolid())
        {
            UIItemTooltips.ShowAsyn(itemObj);
        }
    }

    private void ShowLegendaryPackTooltips(object equipObj)
    {
        EquipItemRecord equipRecord = equipObj as EquipItemRecord;
        if (equipRecord == null)
            return;

        var equipItem = LegendaryData.Instance._LegendaryEquipDict[equipRecord];
        if (equipItem == null || !equipItem.IsVolid())
            return;

        UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10011, PutOffEquip) });
    }
    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshEquipItems();
    }

    public bool IsCanDragItem(UIDragableItemBase dragItem)
    {
        if (!dragItem.ShowedItem.IsVolid())
            return false;

        if (!LegendaryData.IsEquipLegendary(dragItem.ShowedItem as ItemEquip))
            return false;

        return true;
    }

    public bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == dropItem._DragPackBase)
            return false;

        if (dragItem._DragPackBase == this)
        {
            if (!dropItem.ShowedItem.IsVolid())
                return true;

            if (dropItem.ShowedItem is ItemEquip)
            {
                if (dropItem.ShowedItem.ItemDataID == dropItem.ShowedItem.ItemDataID)
                    return true;
                else
                    return false;
            }
        }
        else if (dropItem._DragPackBase == this)
        {
            if (dragItem.ShowedItem is ItemEquip)
            {
                UILegendaryEquipItem legendaryItem = dropItem.transform.parent.GetComponent<UILegendaryEquipItem>();
                if (legendaryItem == null)
                    return false;

                if (legendaryItem._LegendaryRecord.Id == dragItem.ShowedItem.ItemDataID)
                    return true;
            }
        }

        return false;
    }

    public void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == dropItem._DragPackBase)
            return;

        if (dragItem._DragPackBase == this)
        {
            PutOffEquip(dragItem.ShowedItem);
        }
        else if (dropItem._DragPackBase == this)
        {
            PutInEquip(dragItem.ShowedItem);
        }
    }

    private void PutInEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            if (LegendaryData.Instance.PutInEquip(itemEquip))
            {
                RefreshAttrs();
                RefreshEquipItems();
            }
        }
    }

    private void PutOffEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            if (LegendaryData.Instance.PutOffEquip(itemEquip))
            {
                RefreshAttrs();
                RefreshEquipItems();
            }
        }
    }

    #endregion


}

