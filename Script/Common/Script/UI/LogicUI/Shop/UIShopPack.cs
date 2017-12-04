using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIShopPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIShopPack", UILayer.PopUI, hash);
    }

    public static void RefreshShopItems()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIEquipPack>("LogicUI/BagPack/UIShopPack");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public UIContainerBase _ShopItemContainer;
    public UITagPanel _TagPanel;
    public UIBackPack _BackPack;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowPackItems();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void ShowPackItems()
    {
        var page = _TagPanel.GetShowingPage();
        if (page == 0)
        {
            var itemList = new List<ItemEquip>( ShopData.Instance._EquipList);
            ExtendList(itemList);
            _ShopItemContainer.InitContentItem(itemList, ShowShopPackTooltips);
        }
        else if (page == 1)
        {
            var itemList = new List<ItemBase>(ShopData.Instance._ItemList);
            ExtendList(itemList);
            _ShopItemContainer.InitContentItem(itemList, ShowShopPackTooltips);
        }
        else if (page == 2)
        {
            var itemList = new List<ItemBase>(ShopData.Instance._GamblingItems);
            ExtendList(itemList);
            _ShopItemContainer.InitContentItem(itemList, ShowShopPackTooltips);
        }
        _BackPack.Show(null);
    }

    private void ExtendList(List<ItemBase> itemList)
    {
        for (int i = 0; i < 25 - itemList.Count; ++i)
        {
            itemList.Add(new ItemBase() { ItemDataID = "" });
        }
    }

    private void ExtendList(List<ItemEquip> itemList)
    {
        for (int i = 0; i < 25 - itemList.Count; ++i)
        {
            itemList.Add(new ItemEquip() { ItemDataID = "" });
        }
    }

    public void RefreshItems()
    {
        ShowPackItems();
        _BackPack.RefreshItems();
    }

    public void ShowBackPackTooltips(ItemBase itemObj)
    {
        ItemEquip equipItem = itemObj as ItemEquip;
        if (equipItem != null && equipItem.IsVolid())
        {
            UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInBackPack);
        }
        else
        {

        }
    }

    private void ShowShopPackTooltips(object equipObj)
    {
        if (equipObj is ItemEquip)
        {
            ItemEquip equipItem = equipObj as ItemEquip;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
        }
        else if (equipObj is ItemBase)
        {
            ItemBase equipItem = equipObj as ItemBase;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            
        }
    }
    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }

    #endregion

}

