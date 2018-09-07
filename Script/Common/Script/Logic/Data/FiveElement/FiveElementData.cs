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

        needSave |= InitElementsLv();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region element 

    public const int _ElemnetCnt = 5;
    public const int _MaxLevel = 200;

    public static List<string> _MatIDs = new List<string>()
    {
        "1100001",
        "1100002",
        "1100003",
        "1100004",
        "1100005",
    };

    public static List<string> _ElementAttr = new List<string>()
    {
        "100000",
        "100001",
        "100002",
        "100003",
        "100004",
    };

    [SaveField(1)]
    public List<int> _ElementsLvs;

    private bool InitElementsLv()
    {
        if (_ElementsLvs.Count != _ElemnetCnt)
        {
            _ElementsLvs = new List<int>();
            for (int i = 0; i < _ElemnetCnt; ++i)
            {
                _ElementsLvs.Add(0);
            }

            return true;
        }

        return false;
    }

    public int GetElementLv(int idx)
    {
        return _ElementsLvs[idx];
    }

    public FiveElementRecord GetEleLvRecord(int idx)
    {
        int level = GetElementLv(idx) + 1;
        level = Mathf.Clamp(level, 1, _MaxLevel);
        var record = TableReader.FiveElement.GetRecord(level.ToString());
        return record;
    }

    public void ElementLvUp(int idx)
    {
        var record = GetEleLvRecord(idx);

        if (PlayerDataPack.Instance.Gold < record.GoldCost)
        {
            UIMessageTip.ShowMessageTip(20000);
            return;
        }

        if (BackBagPack.Instance.GetItemCnt(_MatIDs[idx]) < record.MatCost)
        {
            UIMessageTip.ShowMessageTip(30003);
            return;
        }

        PlayerDataPack.Instance.DecGold(record.GoldCost);
        var matItem = BackBagPack.Instance.GetItem(_MatIDs[idx]);
        matItem.DecStackNum(record.MatCost);

        ++_ElementsLvs[idx];
    }

    #endregion

    #region attr

    public void SetGemAttr(RoleAttrStruct roleAttr)
    {
        //for (int i = 0; i < _ElemnetCnt; ++i)
        //{
        //    var level = GetElementLv(i);
        //    if (level == 0)
        //        continue;

        //    var record = GetEleLvRecord(i);

        //    var attrValue = TableReader.AttrValue.GetRecord(_ElementAttr[i]); 

        //    if (attrValue.AttrImpact == "RoleAttrImpactBaseAttr")
        //    {
        //        int levelID = Mathf.Clamp(level, 0, 200);
        //        var gemAttrRecord = TableReader.GemBaseAttr.GetRecord(levelID.ToString());
        //        if (gemAttrRecord == null)
        //        {
        //            gemAttrRecord = TableReader.GemBaseAttr.GetRecord("200");
        //        }
        //        var exAttr =  GetGemAttr((RoleAttrEnum)attrValue.AttrParams[0], gemAttrRecord.SetAttrValue);
        //        attrList.Add(exAttr);
        //    }
        //    else
        //    {
        //        int spAttrLv = GemSuit._ActAttrLevel[GemSuit._ActAttrLevel.Count - 1];
        //        int attrLv = 0;
        //        if (level > spAttrLv)
        //        {
        //            attrLv = Mathf.CeilToInt((level - spAttrLv) / 5 + 1);
        //        }
        //        var exAttr = attrValue.GetExAttr(attrLv);
        //        attrList.Add(exAttr);
        //    }
        //}

        GemSuit.Instance.SetGemSetAttr(roleAttr);

    }

    #endregion

}
