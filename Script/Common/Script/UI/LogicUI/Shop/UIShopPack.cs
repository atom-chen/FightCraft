using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIShopPack : UIBase,IDragablePack
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

    public UISubScollMenu _TagMenu;
    public UIContainerBase _ShopItemContainer;
    public UIBackPack _BackPack;

    public static int _MAX_PAGE_ITEM_CNT = 25;

    #endregion

    #region 

    public override void Init()
    {
        base.Init();

        foreach (var shopItem in ShopData.Instance._ShopItems)
        {
            _TagMenu.PushMenu(shopItem.Key);
        }
        _TagMenu.ShowDefaultFirst();

        _BackPack = UIBackPack.GetUIBackPackInstance(transform);
        _BackPack._OnItemSelectCallBack = ShowBackPackSelectItem;
        _BackPack._OnDragItemCallBack = OnDragItem;
        _BackPack._IsCanDropItemCallBack = IsCanDropItem;
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _BackPack.Show(null);
        _TagMenu.ShowDefaultFirst();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnMenu(object menuObj)
    {
        string tagStr = menuObj as string;

        List<ItemShop> itemShow = new List<ItemShop>(ShopData.Instance._ShopItems[tagStr]);
        for (int i = itemShow.Count; i < _MAX_PAGE_ITEM_CNT; ++i)
        {
            itemShow.Add(new ItemShop());
        }

        _ShopItemContainer.InitContentItem(itemShow, ShowShopPackTooltips);
    }
    
    public void ShowBackPackSelectItem(ItemBase itemObj)
    {
        if (itemObj is ItemEquip)
        {
            ItemEquip equipItem = itemObj as ItemEquip;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            var price = GameDataValue.GetEquipSellGold(equipItem);

            UIEquipTooltips.ShowShopAsyn(equipItem, false, MONEYTYPE.GOLD, price, new ToolTipFunc[1] { new ToolTipFunc(10005, SellItem) });
        }
        else if (itemObj is ItemBase)
        {
            ItemBase equipItem = itemObj as ItemBase;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            UIItemTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10005, SellItem) });
        }
    }

    private void ShowShopPackTooltips(object itemObj)
    {
        ItemShop shopItem = itemObj as ItemShop;

        if (shopItem != null && shopItem.IsVolid())
        {
            MONEYTYPE moneyType = shopItem.ShopRecord.MoneyType > 0 ? MONEYTYPE.GOLD : MONEYTYPE.DIAMOND;
            UIItemTooltips.ShowShopAsyn(shopItem, true, moneyType, shopItem.BuyPrice, new ToolTipFunc[1] { new ToolTipFunc(10006, BuyItem) });
        }
    }

    public void RefreshItems()
    {
        _BackPack.RefreshItems();
    }

    #endregion

    #region buy item

    public void BuyItem(ItemBase itemBase)
    {
        var shopItem = itemBase as ItemShop;
        if (!shopItem.ShopRecord.MutiBuy)
        {
            ShopData.Instance.BuyItem(shopItem);
            RefreshItems();
        }
        else
        {
            UIShopNum.ShowAsyn(shopItem, MutiBuyCallBack);
        }
        
    }

    public void MutiBuyCallBack(ItemShop shopItem, int num)
    {
        for (int i = 0; i < num; ++i)
        {
            ShopData.Instance.BuyItem(shopItem);
        }
        RefreshItems();
    }

    public void SellItem(ItemBase itemBase)
    {
        ShopData.Instance.SellItem(itemBase);
        RefreshItems();
    }

    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
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

        return true;

    }

    public void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == this)
        {
            BuyItem(dragItem.ShowedItem);
        }
        else if (dropItem._DragPackBase == this)
        {
            SellItem(dragItem.ShowedItem);
        }
    }

    #endregion

}

