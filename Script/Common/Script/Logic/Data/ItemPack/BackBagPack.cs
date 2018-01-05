using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BackBagPack : DataPackBase
{
    #region 单例

    private static BackBagPack _Instance;
    public static BackBagPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new BackBagPack();
            }
            return _Instance;
        }
    }

    private BackBagPack()
    {
        GuiTextDebug.debug("BackBagPack init");
        _SaveFileName = "BackBagPack";
    }

    #endregion

    public const int _BAG_PAGE_SLOT_CNT = 25;

    [SaveField(1)]
    private List<ItemEquip> _PageEquips = new List<ItemEquip>();
    public List<ItemEquip> PageEquips
    {
        get
        {
            return _PageEquips;
        }
    }

    [SaveField(2)]
    private List<ItemBase> _PageItems = new List<ItemBase>();
    public List<ItemBase> PageItems
    {
        get
        {
            return _PageItems;
        }
    }

    public void InitBackPack()
    {
        bool needSave = false;
        if (_PageEquips == null || _PageEquips.Count != _BAG_PAGE_SLOT_CNT)
        {
            if (_PageEquips == null)
            {
                _PageEquips = new List<ItemEquip>();
            }
            int equipSlotCnt = _BAG_PAGE_SLOT_CNT;
            int startIdx = _PageEquips.Count;
            for (int i = startIdx; i < equipSlotCnt; ++i)
            {
                ItemEquip newItemEquip = new ItemEquip("-1");
                _PageEquips.Add(newItemEquip);
            }
            needSave = true;
        }

        if (_PageItems == null || _PageItems.Count != _BAG_PAGE_SLOT_CNT)
        {
            if (_PageItems == null)
            {
                _PageItems = new List<ItemBase>();
            }
            int equipSlotCnt = _BAG_PAGE_SLOT_CNT;
            int startIdx = _PageItems.Count;
            for (int i = 0; i < equipSlotCnt; ++i)
            {
                ItemBase newItemEquip = new ItemBase("-1");
                //newItemEquip._SaveFileName = "BackPack.Item" + i;
                _PageItems.Add(newItemEquip);
            }
            needSave = true;
        }

        if (needSave)
        {
            SaveClass(true);
        }
    }

    public bool AddEquip(ItemEquip equip)
    {
        var emptyPos = GetEmptyPageEquip();
        if (emptyPos == null)
            return false;

        emptyPos.ExchangeInfo(equip);
        return true;
    }

    public bool AddItem(ItemBase item)
    {
        if (PageItems.Count >= _BAG_PAGE_SLOT_CNT)
        {
            return false;
        }

        PageItems.Add(item);
        LogicManager.Instance.SaveGame();
        return true;
    }

    public bool AddItem(string itemID, int itemCnt)
    {
        var itemPos = GetItemPos(itemID);
        if (itemPos == null)
            return false;

        if (!itemPos.IsVolid())
        {
            itemPos.ItemDataID = itemID;
            itemPos.SetStackNum(itemCnt);
        }
        else
        {
            itemPos.AddStackNum(itemCnt);
        }
        itemPos.SaveClass(true);
        return true;
    }

    public ItemEquip GetEmptyPageEquip()
    {
        for (int i = 0; i < PageEquips.Count; ++i)
        {
            if (!PageEquips[i].IsVolid())
            {
                return PageEquips[i];
            }
        }
        UIMessageTip.ShowMessageTip(10002);
        return null;
    }

    public ItemBase GetItemPos(string itemID)
    {
        for (int i = 0; i < PageItems.Count; ++i)
        {
            if (PageItems[0].IsVolid() && PageItems[0].ItemDataID == itemID)
            {
                return PageItems[0];
            }
        }
        return GetEmptyPageItem();
    }

    

    public ItemBase GetEmptyPageItem()
    {
        for (int i = 0; i < PageItems.Count; ++i)
        {
            if (!PageItems[i].IsVolid())
            {
                return PageItems[i];
            }
        }
        return null;
    }

    public ItemEquip AddNewEquip(ItemEquip itemEquip)
    {
        for (int i = 0; i < PageEquips.Count; ++i)
        {
            if (string.IsNullOrEmpty(PageEquips[i].ItemDataID) || PageEquips[i].ItemDataID == "-1")
            {
                PageEquips[i].ExchangeInfo(itemEquip);
                return PageEquips[i];
            }
        }
        return null;
    }
}

