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
        if (_PageEquips == null || _PageEquips.Count == 0)
        {
            int equipSlotCnt = _BAG_PAGE_SLOT_CNT;
            _PageEquips = new List<ItemEquip>();
            for (int i = 0; i < equipSlotCnt; ++i)
            {
                ItemEquip newItemEquip = new ItemEquip();
                newItemEquip.ItemDataID = "-1";
                newItemEquip._SaveFileName = "BackPack.Equip" + i;
                _PageEquips.Add(newItemEquip);
            }
        }

        if (_PageItems == null || _PageItems.Count == 0)
        {
            int equipSlotCnt = _BAG_PAGE_SLOT_CNT;
            _PageItems = new List<ItemBase>();
            for (int i = 0; i < equipSlotCnt; ++i)
            {
                ItemBase newItemEquip = new ItemBase();
                newItemEquip.ItemDataID = "-1";
                newItemEquip._SaveFileName = "BackPack.Item" + i;
                _PageItems.Add(newItemEquip);
            }
        }
    }

    public bool AddEquip(ItemEquip equip)
    {
        if (PageEquips.Count >= _BAG_PAGE_SLOT_CNT)
        {
            return false;
        }

        PageEquips.Add(equip);
        LogicManager.Instance.SaveGame();
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

    public ItemEquip GetEmptyPageEquip()
    {
        for (int i = 0; i < PageEquips.Count; ++i)
        {
            if (!PageEquips[i].IsVolid())
            {
                return PageEquips[i];
            }
        }
        return null;
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

