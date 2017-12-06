using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIShopPack : UIBase
{

    #region static funs

    public static void BuyItemStatic(ItemBase item)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIShopPack>("LogicUI/Shop/UIShopPack");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.BuyItem(item);
    }

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Shop/UIShopPack", UILayer.PopUI, hash);
    }

    public static void RefreshShopItems()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIShopPack>("LogicUI/Shop/UIShopPack");
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

        ShowPackItems(0);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void ShowPackItems(int page)
    {
        if (page == 0)
        {
            var itemList = new List<ItemEquip>( ShopData.Instance._EquipList);
            ExtendList(itemList);
            _ShopItemContainer.InitContentItem(itemList, ShowShopPackTooltips);
            _BackPack._TagPanel.ShowPage(0);
            _BackPack.OnShowPage(0);
        }
        else if (page == 1)
        {
            var itemList = new List<ItemBase>(ShopData.Instance._ItemList);
            ExtendList(itemList);
            _ShopItemContainer.InitContentItem(itemList, ShowShopPackTooltips);
            _BackPack._TagPanel.ShowPage(1);
            _BackPack.OnShowPage(1);
        }
        else if (page == 2)
        {
            var itemList = new List<ItemBase>(ShopData.Instance._GamblingItems);
            ExtendList(itemList);
            _ShopItemContainer.InitContentItem(itemList, ShowShopPackTooltips);
            _BackPack._TagPanel.ShowPage(0);
            _BackPack.OnShowPage(0);
        }
        
    }

    private void ExtendList(List<ItemBase> itemList)
    {
        int needExtend = 25 - itemList.Count;
        for (int i = 0; i < needExtend; ++i)
        {
            itemList.Add(new ItemBase() { ItemDataID = "" });
        }
    }

    private void ExtendList(List<ItemEquip> itemList)
    {
        int needExtend = 25 - itemList.Count;
        for (int i = 0; i < needExtend; ++i)
        {
            itemList.Add(new ItemEquip() { ItemDataID = "" });
        }
    }

    public void RefreshItems()
    {
        ShowPackItems(_TagPanel.GetShowingPage());
        _BackPack.RefreshItems();
    }

    public void ShowBackPackTooltips(ItemBase itemObj)
    {
        ItemEquip equipItem = itemObj as ItemEquip;
        if (equipItem != null && equipItem.IsVolid())
        {
            UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInShopRight);
        }
        else if(itemObj.IsVolid())
        {
            UIItemTooltips.ShowAsyn(itemObj, ToolTipsShowType.ShowInShopRight);
        }
    }

    private void ShowShopPackTooltips(object equipObj)
    {
        if (equipObj is ItemEquip)
        {
            ItemEquip equipItem = equipObj as ItemEquip;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInShopLeft);
        }
        else if (equipObj is ItemBase)
        {
            ItemBase equipItem = equipObj as ItemBase;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            UIItemTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInShopLeft);
        }
    }

    #endregion

    #region buy item

    public void BuyItem(ItemBase itemBase)
    {
        int page = _TagPanel.GetShowingPage();
        if (page == 0)
        {
            var shopIdx = ShopData.Instance._EquipList.IndexOf(itemBase as ItemEquip);
            if (shopIdx < 0)
                return;

            ShopData.Instance.BuyEquip(shopIdx);
        }
        else if (page == 1)
        {
            var shopIdx = ShopData.Instance._ItemList.IndexOf(itemBase);
            if (shopIdx < 0)
                return;

            ShopData.Instance.BuyItem(shopIdx);
        } 
        else if (page == 2)
        {
            var shopIdx = ShopData.Instance._GamblingItems.IndexOf(itemBase);
            if (shopIdx < 0)
                return;

            ShopData.Instance.Gambling(shopIdx);
        }
        RefreshItems();
    }

    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }

    #endregion

}

