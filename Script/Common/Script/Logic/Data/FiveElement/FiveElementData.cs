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
        needSave |= InitPackCore();
        needSave |= InitUsingCore();
        CalculateAttrs();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region element 

    public const int _ITEM_PACKET_CNT = 200;
    
    [SaveField(1)]
    public List<ItemFiveElement> _UsingElements;

    [SaveField(2)]
    public ItemPackBase<ItemFiveElement> _PackElements;

    private bool InitPackElements()
    {
        _PackElements = new ItemPackBase<ItemFiveElement>();
        _PackElements._SaveFileName = "PackFiveElements";
        _PackElements._PackSize = _ITEM_PACKET_CNT;
        _PackElements.LoadClass(true);

        if (_PackElements._PackItems == null)
        {
            _PackElements._PackItems = new List<ItemFiveElement>();
            _PackElements.SaveClass(true);
            return true;
        }

        return false;
    }

    private bool InitUsingElements()
    {
        if (_UsingElements == null || _UsingElements.Count == 0)
        {
            _UsingElements = new List<ItemFiveElement>();
            _UsingElements.Add(new ItemFiveElement("1100001"));
            _UsingElements.Add(new ItemFiveElement("1100002"));
            _UsingElements.Add(new ItemFiveElement("1100003"));
            _UsingElements.Add(new ItemFiveElement("1100004"));
            _UsingElements.Add(new ItemFiveElement("1100005"));
            return true;
        }

        return false;
    }

    public void AddElementItem(ItemFiveElement elementItem)
    {
        _PackElements.AddItem(elementItem);
    }

    public void SortPack()
    {
        _PackElements._PackItems.Sort((itemA, itemB) =>
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

    #region element core

    public const int _ELEMENT_CORE_PACKET_CNT = 50;

    [SaveField(3)]
    public List<ItemFiveElementCore> _UsingCores;

    [SaveField(4)]
    public ItemPackBase<ItemFiveElementCore> _PackCores;

    private bool InitPackCore()
    {
        _PackCores = new ItemPackBase<ItemFiveElementCore>();
        _PackCores._SaveFileName = "PackFiveElementCores";
        _PackCores._PackSize = _ITEM_PACKET_CNT;
        _PackCores.LoadClass(true);

        if (_PackCores._PackItems == null)
        {
            _PackCores._PackItems = new List<ItemFiveElementCore>();
            _PackCores.SaveClass(true);
            return true;
        }

        return false;
    }

    private bool InitUsingCore()
    {
        if (_UsingCores == null || _UsingCores.Count == 0)
        {
            _UsingCores = new List<ItemFiveElementCore>();
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            return true;
        }

        return false;
    }

    public void CreateCoreItem(string coreID)
    {
        var coreItem = new ItemFiveElementCore(coreID);
        //TODO: add attr
        _PackCores.AddItem(coreItem);
    }

    #endregion

    #region value attr

    private int _ElementsTotalValue = -1;
    public int ElementsTotalValue
    {
        get
        {
            CalculateValue();
            return _ElementsTotalValue;
        }
    }

    private List<EquipExAttr> _ValueAttrs = new List<EquipExAttr>();
    public List<EquipExAttr> ValueAttrs
    {
        get
        {
            return _ValueAttrs;
        }
    }

    private void CalculateValue()
    {
        _ElementsTotalValue = 0;
        for (int i = 0; i < _UsingElements.Count; ++i)
        {
            _ElementsTotalValue += _UsingElements[i].CombatValue;
        }
    }

    private void CalculateAttrs()
    {
        _ValueAttrs = new List<EquipExAttr>();
        CalculateValue();
        if (_ElementsTotalValue == 0)
            return;

        var valueAttrs = Tables.TableReader.FiveElementValueAttr.Records.Values;
        foreach (var valueAttr in valueAttrs)
        {
            string attr = Tables.StrDictionary.GetFormatStr(valueAttr.DescIdx);
            if (_ElementsTotalValue >= valueAttr.Value)
            {
                _ValueAttrs.Add(TableReader.AttrValue.GetExAttr(valueAttr.Attr, _ElementsTotalValue));
            }
        }
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

        for (int j = 0; j < ValueAttrs.Count; ++j)
        {
            if (ValueAttrs[j].AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)ValueAttrs[j].AttrParams[0], ValueAttrs[j].AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(ValueAttrs[j]));
            }
        }

    }

    #endregion

    #region opt

    public void PutOnElementCore(ItemFiveElementCore itemElement)
    {
        if (!itemElement.IsVolid())
            return;

        _UsingCores[(int)itemElement.FiveElementRecord.ElementType].ExchangeInfo(itemElement);

        RoleData.SelectRole.CalculateAttr();
    }

    public void PutOffElementCore(ItemFiveElementCore itemElement)
    {
        ItemFiveElementCore emptySlot = (ItemFiveElementCore)_PackCores.GetEmptyPos();
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

        int sameIdx = -1;
        for (int i = 0; i < usingElement.EquipExAttrs.Count; ++i)
        {
            if (usingElement.EquipExAttrs[i].AttrParams[0] == itemElement.EquipExAttrs[0].AttrParams[0])
            {
                sameIdx = i;
            }
        }
        //replace same attr
        if (sameIdx >= 0)
        {
            usingElement.ReplaceAttr(sameIdx, itemElement.EquipExAttrs[0]);
        }
        else
        {
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
                if (sameIdx >= 0 && replaceIdx >= sameIdx)
                {
                    ++replaceIdx;
                }
                usingElement.ReplaceAttr(replaceIdx, itemElement.EquipExAttrs[0]);
            }
        }
        itemElement.ResetItem();

        CalculateAttrs();

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
        else if (idx == 5)
        {
            extraRate = 0.03f;
        }

        return extraRate;
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
