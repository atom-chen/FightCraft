using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIEquipPack : UIBase,IDragablePack
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIEquipPack", UILayer.PopUI, hash);
    }

    public static void RefreshBagItems()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIEquipPack>("LogicUI/BagPack/UIEquipPack");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshEquipItems();
    }

    

    #endregion

    #region 

    public UIContainerBase _EquipContainer;
    public UIBackPack _BackPack;
    public UITagPanel _TagPanel;

    public override void Init()
    {
        base.Init();

        _BackPack = UIBackPack.GetUIBackPackInstance(transform);
        _BackPack._OnItemSelectCallBack = ShowBackPackSelectItem;
        _BackPack._OnDragItemCallBack = OnDragItem;
        _BackPack._IsCanDropItemCallBack = IsCanDropItem;
        _BackPack._TagPanel._Tags[1].gameObject.SetActive(false);
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _TagPanel.ShowPage(0);
        ShowPackItems();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        _EquipContainer.InitContentItem(PlayerDataPack.Instance._SelectedRole._EquipList, ShowEquipPackTooltips, exHash);
        _BackPack.Show(null);
    }

    public void RefreshEquipItems()
    {
        _EquipContainer.RefreshItems();
        _BackPack.RefreshItems();
    }

    public void ShowBackPackSelectItem(ItemBase itemObj)
    {
        ItemEquip equipItem = itemObj as ItemEquip;
        if (equipItem != null && equipItem.IsVolid())
        {
            UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10003, PutOnEquip) });
        }
        else if (itemObj.IsVolid())
        {
            UIItemTooltips.ShowAsyn(itemObj);
        }
    }

    private void ShowEquipPackTooltips(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10004, PutOffEquip) });
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
                var equip = (ItemEquip)dropItem.ShowedItem;
                var slot = PlayerDataPack.Instance._SelectedRole._EquipList.IndexOf(equip);
                if (slot < 0)
                    return false;

                return PlayerDataPack.Instance._SelectedRole.IsCanEquipItem((Tables.EQUIP_SLOT)slot, equip);
            }
        }
        else if (dropItem._DragPackBase == this)
        {
            if (dragItem.ShowedItem is ItemEquip)
            {
                var equip = (ItemEquip)dragItem.ShowedItem;
                var slot = PlayerDataPack.Instance._SelectedRole._EquipList.IndexOf(dropItem.ShowedItem as ItemEquip);
                if (slot < 0)
                    return false;

                return PlayerDataPack.Instance._SelectedRole.IsCanEquipItem((Tables.EQUIP_SLOT)slot, equip);
            }
        }

        return true;
    }

    public void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == this)
        {
            var dragEquip = dragItem.ShowedItem as ItemEquip;

            if (!dropItem.ShowedItem.IsVolid())
            {
                PlayerDataPack.Instance._SelectedRole.PutOffEquip(dragEquip.EquipItemRecord.Slot, dragEquip);
                return;
            }

            if (dropItem.ShowedItem is ItemEquip)
            {
                PutOnEquip(dropItem.ShowedItem);
            }
            else
            {
                PutOffEquip(dragItem.ShowedItem);
            }
        }
        else if (dropItem._DragPackBase == this)
        {
            if (dragItem.ShowedItem is ItemEquip)
            {
                PutOnEquip(dragItem.ShowedItem);
            }
        }
    }

    private void PutOnEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            PlayerDataPack.Instance._SelectedRole.PutOnEquip(itemEquip.EquipItemRecord.Slot, itemEquip);
        }
    }

    private void PutOffEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            PlayerDataPack.Instance._SelectedRole.PutOffEquip(itemEquip.EquipItemRecord.Slot, itemEquip);
        }
    }

    #endregion


}

