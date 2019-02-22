using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

public class ItemFiveElementCore : ItemBase
{
    public ItemFiveElementCore(string datID) : base(datID)
    {

    }

    public ItemFiveElementCore():base()
    {

    }

    #region attr

    public int Level
    {
        get
        {
            return ItemStackNum;
        }
        protected set
        {
            ItemStackNum = value;
        }
    }

    #endregion

    #region elevemt data

    private FiveElementCoreRecord _FiveElementCoreRecord;
    public FiveElementCoreRecord FiveElementRecord
    {
        get
        {
            if (_FiveElementCoreRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _FiveElementCoreRecord = TableReader.FiveElementCore.GetRecord(_ItemDataID);
            }
            return _FiveElementCoreRecord;
        }
    }
    
    public string GetElementNameWithColor()
    {
        string equipName = StrDictionary.GetFormatStr(CommonItemRecord.NameStrDict);
        return CommonDefine.GetQualityColorStr(FiveElementRecord.Quality) + equipName + "</color>";
    }

    public bool IsHaveCondition(int idx)
    {
        if (idx < 0 || idx >= FiveElementRecord.PosCondition.Count)
            return false;

        if (FiveElementRecord.PosCondition[idx] == -1)
        {
            return false;
        }

        return true;
    }

    public int ConditionState(int idx)
    {
        if (idx < 0 || idx >= FiveElementRecord.PosCondition.Count)
            return -1;

        if (FiveElementRecord.PosCondition[idx] == -1)
        {
            return -1;
        }

        List<int> subCons = new List<int>();
        int tempCon = FiveElementRecord.PosCondition[idx];
        while (tempCon >= 10)
        {
            int subCon = (int)tempCon % 10;
            tempCon /= 10;
            subCons.Add(subCon);
        }
        subCons.Add(tempCon);

        ItemFiveElement usingElement = FiveElementData.Instance._UsingElements[(int)FiveElementRecord.ElementType];
        bool allConditionComplate = true;
        for (int i = 0; i < subCons.Count; ++i)
        {
            if(usingElement.EquipExAttrs.Count <= subCons[i])
            {
                allConditionComplate = false;
                break;
            }

            if (FiveElementRecord.PosAttrLimit[subCons[i]] >= 0 
                && usingElement.EquipExAttrs[subCons[i]].AttrParams[0] != FiveElementRecord.PosAttrLimit[subCons[i]])
            {
                allConditionComplate = false;
                break;
            }
        }

        if (allConditionComplate)
            return 1;

        return 0;
    }

    public string GetConditionDesc(int idx)
    {
        if (idx < 0 || idx >= FiveElementRecord.PosCondition.Count)
            return "";

        if (FiveElementRecord.PosCondition[idx] == -1)
        {
            return "";
        }

        List<int> subCons = new List<int>();
        int tempCon = FiveElementRecord.PosCondition[idx];
        while (tempCon >= 10)
        {
            int subCon = (int)tempCon % 10;
            tempCon /= 10;
            subCons.Add(subCon);
        }
        subCons.Add(tempCon);

        ItemFiveElement usingElement = FiveElementData.Instance._UsingElements[(int)FiveElementRecord.ElementType];
        string desc = "";
        for (int i = 0; i < subCons.Count; ++i)
        {
            string attrStr = "";
            if (FiveElementRecord.PosAttrLimit[subCons[i]] > 0)
            {
                attrStr = StrDictionary.GetFormatStr(FiveElementRecord.PosAttrLimit[subCons[i]]);
            }
            else
            {
                attrStr = StrDictionary.GetFormatStr("1350002");
            }

            desc += StrDictionary.GetFormatStr("1350001", subCons[i] + 1, attrStr);

            if (i != subCons.Count - 1)
            {
                desc += " \n& ";
            }
            
        }

        return desc;
    }

    #endregion

}

