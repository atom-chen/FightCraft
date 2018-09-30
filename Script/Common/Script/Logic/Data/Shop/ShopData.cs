using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class ShopData : SaveItemBase
{
    #region 唯一

    private static ShopData _Instance = null;
    public static ShopData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ShopData();
            }
            return _Instance;
        }
    }

    private ShopData()
    {
        _SaveFileName = "ShopData";
    }

    #endregion

    #region shop refresh

    [SaveField(1)]
    public string _LastRefreshTimeStr;

    private DateTime _LastRefreshTime;
    public DateTime LastRefreshTime
    {
        get
        {
            if (_LastRefreshTime.Year < 2000)
            {
                if (!string.IsNullOrEmpty(_LastRefreshTimeStr))
                {
                    _LastRefreshTime = DateTime.Parse(_LastRefreshTimeStr);
                }
            }
            return _LastRefreshTime;
        }
        set
        {
            _LastRefreshTime = value;
            _LastRefreshTimeStr = _LastRefreshTime.ToString();
        }
    }

    public void InitShop()
    {
        if (DateTime.Now.Day != LastRefreshTime.Day || LastRefreshTime.Year < 2000)
        {
            RefreshShopItem();
            
            LastRefreshTime = DateTime.Now;

            SaveClass(true);
        }
        InitShopItem();
    }

    public void SellItem(ItemBase sellItem, bool isNeedEnsure = true)
    {
        if (isNeedEnsure)
        {
            if (sellItem is ItemEquip)
            {
                ItemEquip itemEquip = sellItem as ItemEquip;
                if (itemEquip.EquipRefreshCostMatrial > 0)
                {
                    UIMessageBox.Show(20002, null, null, BtnType.OKBTN);
                    return;
                }
            }

            if (sellItem.CommonItemRecord.Quality == ITEM_QUALITY.ORIGIN
                || sellItem.CommonItemRecord.Quality == ITEM_QUALITY.PURPER)
            {
                UIMessageBox.Show(20003, () => { SellItemOK(sellItem); }, null);
                return;
            }
        }

        SellItemOK(sellItem);

    }

    public void SellItemOK(ItemBase sellItem)
    {
        int gold = 1;
        if (sellItem is ItemEquip)
        {
            gold = GameDataValue.GetEquipSellGold((ItemEquip)sellItem);
        }
        sellItem.ResetItem();
        PlayerDataPack.Instance.AddGold(gold);
        sellItem.SaveClass(true);

        AddToBuyBack(sellItem as ItemEquip);
    }

    #endregion

    #region item shop

    [SaveField(2)]
    public List<int> _ShopLimit=new List<int>();

    public Dictionary<string, List<ItemShop>> _ShopItems;

    public void InitShopItem()
    {
        _ShopItems = new Dictionary<string, List<ItemShop>>();
        int i = 0; 
        foreach (var shopItem in TableReader.ShopItem.Records.Values)
        {
            ItemShop itemShop = new ItemShop(shopItem.Id);
            itemShop.BuyTimes = _ShopLimit[i];

            if (!_ShopItems.ContainsKey(itemShop.ShopRecord.Class))
            {
                _ShopItems.Add(itemShop.ShopRecord.Class, new List<ItemShop>());
            }
            _ShopItems[itemShop.ShopRecord.Class].Add(itemShop);
            ++i;
        }
    }

    public void RefreshShopItem()
    {
        _ShopLimit = new List<int>();
        foreach (var shopItem in TableReader.ShopItem.Records.Values)
        {
            _ShopLimit.Add(shopItem.DailyLimit);
        }
    }

    public void BuyItem(ItemShop shopItem)
    {
        var shopItemTab = shopItem.ShopRecord;
        if (shopItem.BuyTimes == 0)
        {
            UIMessageTip.ShowMessageTip(20004);
            return;
        }
        if (BackBagPack.Instance.GetEmptyPageEquip() == null)
        {
            return;
        }
        if (shopItemTab.MoneyType == 0)
        {
            if (!PlayerDataPack.Instance.DecGold(shopItem.BuyPrice))
                return;
        }
        else
        {
            if (!PlayerDataPack.Instance.DecDiamond(shopItem.BuyPrice))
                return;
        }

        var scriptType = Type.GetType(shopItemTab.Script);
        var buyMethod = scriptType.GetMethod("BuyItem", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        buyMethod.Invoke(null, new object[1] { shopItem} );

        if (shopItem.BuyTimes > 0)
            --shopItem.BuyTimes;
        Debug.Log("BuyItem:" + shopItem.ItemDataID);
    }

    #endregion


    #region buy back

    public const int _MaxBuyBackCnt = 25;
    private List<ItemEquip> _BuyBackList;
    public List<ItemEquip> BuyBackList
    {
        get
        {
            if (_BuyBackList == null)
            {
                _BuyBackList = new List<ItemEquip>();
                for (int i = 0; i < _MaxBuyBackCnt; ++i)
                {
                    _BuyBackList.Add(new ItemEquip());
                }
            }
            return _BuyBackList;
        }
    }



    public void AddToBuyBack(ItemEquip itemBase)
    {
        if (itemBase == null)
            return;

        for (int i = 0; i < BuyBackList.Count; ++i)
        {
            if (string.IsNullOrEmpty(BuyBackList[i].ItemDataID) || BuyBackList[i].ItemDataID == "-1")
            {
                BuyBackList[i].ExchangeInfo(itemBase);
            }
        }
    }

    #endregion

}
