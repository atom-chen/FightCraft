using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class FiveElementData : SaveItemBase
{
    #region 唯一

    private static FiveElementData _Instance = null;
    public static FiveElementData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new FiveElementData();
            }
            return _Instance;
        }
    }

    private FiveElementData()
    {
        _SaveFileName = "FiveElementData";
    }

    #endregion

    public void InitFiveElementData()
    {
        bool needSave = false;

        needSave |= InitPackElements();
        needSave |= InitUsingElements();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region element 

    public const int _ITEM_PACKET_CNT = 50;
    
    [SaveField(1)]
    public List<ItemFiveElement> _UsingElements;

    [SaveField(2)]
    public List<ItemFiveElement> _PackElements;

    private bool InitPackElements()
    {
        if (_PackElements == null || _PackElements.Count == 0)
        {
            _PackElements = new List<ItemFiveElement>();
            for (int i = 0; i < _ITEM_PACKET_CNT; ++i)
            {
                _PackElements.Add(new ItemFiveElement());
            }
            return true;
        }

        return false;
    }

    private bool InitUsingElements()
    {
        if (_UsingElements == null || _UsingElements.Count == 0)
        {
            _UsingElements = new List<ItemFiveElement>();
            for (int i = 0; i <= (int)FIVE_ELEMENT.EARTH; ++i)
            {
                _UsingElements.Add(new ItemFiveElement());
            }
            return true;
        }

        return false;
    }

    public void AddElementItem(ItemFiveElement elementItem)
    {
        ItemFiveElement emptySlot = FindEmptySlot();
        if (emptySlot == null)
        {
            UIMessageTip.ShowMessageTip(10002);
            return;
        }

        emptySlot.ExchangeInfo(elementItem);
    }

    public void SortPack()
    {
        _PackElements.Sort((itemA, itemB) =>
        {
            if (itemA.IsVolid() && !itemB.IsVolid())
                return -1;
            else if (!itemA.IsVolid() && itemB.IsVolid())
                return 1;
            else
                return 0;
        });
    }

    #endregion

    #region opt

    public ItemFiveElement FindEmptySlot()
    {
        ItemFiveElement emptySlot = null;
        for (int i = 0; i < _PackElements.Count; ++i)
        {
            if (!_PackElements[i].IsVolid())
            {
                emptySlot = _PackElements[i];
                break;
            }
        }

        return emptySlot;
    }

    public void PutOnElement(ItemFiveElement itemElement)
    {
        if (!itemElement.IsVolid())
            return;

        _UsingElements[(int)itemElement.FiveElementRecord.EvelemtType].ExchangeInfo(itemElement);

        RoleData.SelectRole.CalculateAttr();
    }

    public void PutOffElement(ItemFiveElement itemElement)
    {
        ItemFiveElement emptySlot = FindEmptySlot();
        if (emptySlot == null)
        {
            UIMessageTip.ShowMessageTip(10002);
            return;
        }

        emptySlot.ExchangeInfo(itemElement);

        RoleData.SelectRole.CalculateAttr();
    }

    public bool Extract(ItemFiveElement itemElement)
    {
        ItemFiveElement usingElement = _UsingElements[(int)itemElement.FiveElementRecord.EvelemtType];
        if (usingElement == null || !usingElement.IsVolid())
            return false;

        float extraRate = GetAddExAttrRate(usingElement.EquipExAttrs.Count);

        float extraRandom = UnityEngine.Random.Range(0, 1.0f);
        if (extraRandom < extraRate)
        {
            usingElement.AddExAttr(itemElement.EquipExAttrs[0]);
        }
        else
        {
            List<int> randomIdxs = new List<int>();
            for (int i = 0; i < usingElement.EquipExAttrs.Count; ++i)
            {
                randomIdxs.Add(100);
            }
            int replaceIdx = GameRandom.GetRandomLevel(randomIdxs);
            usingElement.ReplaceAttr(replaceIdx, itemElement.EquipExAttrs[0]);
        }
        itemElement.ResetItem();
        SortPack();
        SaveClass(true);
        return true;
    }

    public float GetAddExAttrRate(int idx)
    {
        float extraRate = 0;
        if (idx == 0)
        {
            extraRate = 1;
        }
        else if (idx == 1)
        {
            extraRate = 0.5f;
        }
        else if (idx == 2)
        {
            extraRate = 0.3f;
        }
        else if (idx == 3)
        {
            extraRate = 0.15f;
        }
        else if (idx == 4)
        {
            extraRate = 0.05f;
        }

        return extraRate;
    }

    public void SetAttr(RoleAttrStruct roleAttr)
    {
        for (int i = 0; i < _UsingElements.Count; ++i)
        {
            if (_UsingElements[i] == null)
                continue;

            if (!_UsingElements[i].IsVolid())
                continue;

            for (int j = 0; j < _UsingElements[i].EquipExAttrs.Count; ++j)
            {
                if (_UsingElements[i].EquipExAttrs[j].AttrType == "RoleAttrImpactBaseAttr")
                {
                    roleAttr.AddValue((RoleAttrEnum)_UsingElements[i].EquipExAttrs[j].AttrParams[0], _UsingElements[i].EquipExAttrs[j].AttrParams[1]);
                }
                else
                {
                    roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(_UsingElements[i].EquipExAttrs[j]));
                }
            }
        }

        GemSuit.Instance.SetGemSetAttr(roleAttr);

    }

    #endregion

    #region static create

    public static ItemFiveElement CreateElementItem(int level, FIVE_ELEMENT elementType = FIVE_ELEMENT.METAL)
    {
        var elementRecord = TableReader.FiveElement.GetFiveElementByType(elementType);
        ItemFiveElement itemElement = new ItemFiveElement(elementRecord.Id);

        //RandomEquipAttr(itemEquip);
        itemElement.AddExAttr(GameDataValue.GetFiveElementExAttr(level, elementType));

        return itemElement;
    }

    #endregion

}
