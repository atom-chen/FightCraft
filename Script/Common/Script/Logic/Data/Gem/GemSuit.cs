using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;

public class GemSuit
{
    #region 唯一

    private static GemSuit _Instance = null;
    public static GemSuit Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GemSuit();
            }
            return _Instance;
        }
    }

    private GemSuit()
    {
        
    }

    #endregion

    private GemSetRecord _ActSet;
    public GemSetRecord ActSet
    {
        get
        {
            return _ActSet;
        }
    }

    private int _ActLevel;
    public int ActLevel
    {
        get
        {
            return _ActLevel;
        }
    }

    public static List<int> _ActAttrLevel = new List<int>() { 5, 10, 15, 20, 25 };


    private List<EquipExAttr> _ActSetAttrs = new List<EquipExAttr>();
    public List<EquipExAttr> ActSetAttrs
    {
        get
        {
            if (_ActSetAttrs == null)
            {
                _ActSetAttrs = new List<EquipExAttr>();
                IsActSet();
            }
            return _ActSetAttrs;
        }
    }

    private int _ActSetAttrCnt = 0;
    public int ActSetAttrCnt
    {
        get
        {
            return _ActSetAttrCnt;
        }
    }

    public bool IsGemSetCanUse(GemSetRecord gemSet)
    {
        foreach (var gemRecord in gemSet.Gems)
        {
            if (gemRecord == null)
                continue;

            var gemInfo = GemData.Instance.GetGemClassMax(gemRecord.Class, gemSet.MinGemLv);
            if (gemInfo == null || !gemInfo.IsVolid() || gemInfo.GemRecord.Level < gemSet.MinGemLv)
                return false;
        }

        return true;
    }

    public void UseGemSet(GemSetRecord gemSet)
    {
        if (!IsGemSetCanUse(gemSet))
        {
            return;
        }

        for (int i = 0; i < gemSet.Gems.Count; ++i)
        {
            if (gemSet.Gems[i] == null)
                continue;

            var gemInfo = GemData.Instance.GetGemClassMax(gemSet.Gems[i].Class, gemSet.MinGemLv);
            if (gemInfo == null)
            {
                Debug.LogError("Get geminfo error");
            }

            GemData.Instance.PutOnGem(gemInfo, i);
        }
    }

    public bool IsActSet()
    {
        var gemSuitTabs = Tables.TableReader.GemSet.Records;

        _ActSet = null;
        _ActLevel = -1;
        foreach (var gemSuit in gemSuitTabs)
        {
            _ActLevel = -1;
            for (int i = 0; i < gemSuit.Value.Gems.Count; ++i)
            {
                if (gemSuit.Value.Gems[i] == null)
                {
                    continue;
                }
                if (GemData.Instance.EquipedGemDatas._PackItems[i] == null || !GemData.Instance.EquipedGemDatas._PackItems[i].IsVolid())
                {
                    break;
                }
                if (GemData.Instance.EquipedGemDatas._PackItems[i].GemRecord.Class == gemSuit.Value.Gems[i].Class
                    && GemData.Instance.EquipedGemDatas._PackItems[i].GemRecord.Level >= gemSuit.Value.MinGemLv)
                {
                    if (_ActLevel < 0)
                    {
                        _ActLevel = GemData.Instance.EquipedGemDatas._PackItems[i].GemRecord.Level;
                    }
                    else
                    {
                        _ActLevel = Mathf.Min(_ActLevel, GemData.Instance.EquipedGemDatas._PackItems[i].GemRecord.Level);
                    }
                }
                else
                {
                    break;
                }

                if (i == gemSuit.Value.Gems.Count - 1)
                {
                    _ActSet = gemSuit.Value;
                    break;
                }
            }
            if (_ActSet != null)
            {
                CalculateSetAttrs();
                Hashtable hash = new Hashtable();
                hash.Add("ActGemSet", _ActSet);
                GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_GEM_ACT_SUIT, this, hash);

                return true;
            }
        }
        CalculateSetAttrs();

        return false;
    }

    private void CalculateSetAttrs()
    {
        _ActSetAttrs.Clear();
        _ActSetAttrCnt = 0;
        if (_ActSet != null)
        {
            _ActSetAttrs = GameDataValue.GetGemSetAttr(_ActSet, _ActLevel);
            _ActSetAttrCnt = _ActSetAttrs.Count;
            //for (int i = 0; i < _ActAttrLevel.Count; ++i)
            //{
            //    //if (_ActLevel >= _ActAttrLevel[i])
            //    {
            //        _ActSetAttrCnt = i + 1;
            //    }
            //}
        }
    }

    public void SetGemSetAttr(RoleAttrStruct roleAttr)
    {
        for (int i = 0; i < _ActSetAttrCnt; ++i)
        {
            if (ActSetAttrs[i].AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)ActSetAttrs[i].AttrParams[0], ActSetAttrs[i].AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(ActSetAttrs[i]));
            }
        }
    }

    #region suit gems

    private Dictionary<GemSetRecord, List<GemTableRecord>> _SuitGemRecordDict;

    private void InitSuitGems()
    {
        if (_SuitGemRecordDict != null)
            return;

        _SuitGemRecordDict = new Dictionary<GemSetRecord, List<GemTableRecord>>();
        foreach (var suitRecord in TableReader.GemSet.Records.Values)
        {
            List<GemTableRecord> gemList = new List<GemTableRecord>();
            foreach (var suitGem in suitRecord.Gems)
            {
                if (suitGem == null)
                    continue;

                foreach (var gemRecord in TableReader.GemTable.Records.Values)
                {
                    if (suitGem.Class == gemRecord.Class && gemRecord.Level == suitRecord.MinGemLv)
                    {
                        gemList.Add(suitGem);
                    }
                }
            }
            _SuitGemRecordDict.Add(suitRecord, gemList);
        }
    }

    public List<GemTableRecord> GetRecordGemRecords(GemSetRecord gemSetRecord)
    {
        InitSuitGems();

        return _SuitGemRecordDict[gemSetRecord];
    }

    #endregion


}
