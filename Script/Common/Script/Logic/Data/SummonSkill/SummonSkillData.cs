﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class SummonCollectItem
{
    public SummonSkillRecord _SummonRecord;
    public int _Star;
}

public class SummonMotionData
{
    [SaveField(1)]
    public string SummonRecordID;
    [SaveField(2)]
    public int Exp;
    [SaveField(3)]
    public int StarExp;
    [SaveField(4)]
    public bool IsLock;
    [SaveField(5)]
    public int Stage;
    [SaveField(6)]
    public List<int> StageAttrs;

    public SummonMotionData()
    {
        SummonRecordID = "";
        Exp = 0;
        StarExp = 0;
        IsLock = false;
    }

    public SummonMotionData(string recordID)
    {
        SummonRecordID = recordID;
        Exp = 0;
        StarExp = 0;
        IsLock = false;
    }

    private SummonSkillRecord _SummonRecord;
    public SummonSkillRecord SummonRecord
    {
        get
        {
            if (_SummonRecord == null)
            {
                Debug.Log("SummonRecordID:" + SummonRecordID);
                _SummonRecord = Tables.TableReader.SummonSkill.GetRecord(SummonRecordID);
            }
            return _SummonRecord;
        }
    }

    #region level

    private int _Level = -1;
    public int Level
    {
        get
        {
            if (_Level < 0)
            {
                CalculateLevel();
            }
            return _Level;
        }
    }

    private int _CurLvExp = -1;
    public int CurLvExp
    {
        get
        {
            if (_CurLvExp < 0)
            {
                CalculateLevel();
            }
            return _CurLvExp;
        }
    }

    public void AddExp(int expValue)
    {
        Exp += expValue;
        CalculateLevel();
    }

    private void CalculateLevel()
    {
        int tempExp = Exp;
        int level = 1;
        while (true)
        {
            var summonAttrRecord = TableReader.SummonSkillAttr.GetRecord(level.ToString());
            if (tempExp >= summonAttrRecord.Cost[0])
            {
                tempExp = tempExp - summonAttrRecord.Cost[0];
                ++level;
            }
            else
            {
                break;
            }
        }

        _Level = level;
        _CurLvExp = tempExp;

        UpdateAttrs();
    }

    #endregion

    #region star

    private int _StarLevel = -1;
    public int StarLevel
    {
        get
        {
            if (_StarLevel < 0)
            {
                CalculateStarLevel();
            }
            return _StarLevel;
        }
    }

    private int _CurStarExp = -1;
    public int CurStarExp
    {
        get
        {
            if (_CurStarExp < 0)
            {
                CalculateStarLevel();
            }
            return _CurStarExp;
        }
    }

    public int CurStarLevelExp()
    {
        if (StarLevel < SummonRecord.StarExp.Count)
        {
            return SummonRecord.StarExp[StarLevel];
        }
        return 0;
    }

    public void AddStarExp(int expValue)
    {
        StarExp += expValue;
        CalculateStarLevel();
    }

    private void CalculateStarLevel()
    {
        int tempExp = StarExp;
        int starLv = 0;
        for (int i = 0; i < SummonRecord.StarExp.Count; ++i)
        {
            if (tempExp >= SummonRecord.StarExp[i])
            {
                tempExp = tempExp - SummonRecord.StarExp[i];
                ++starLv;
            }
            else
            {
                break;
            }
        }

        _StarLevel = starLv;
        _CurStarExp = tempExp;
    }

    #endregion

    #region stage

    public void InitStageAttr()
    {
        if (StageAttrs != null && StageAttrs.Count != 0)
            return;

        StageAttrs = new List<int>();
        for (int i = 0; i < 5; ++i)
        {
            StageAttrs.Add(0);
        }
    }

    public void AddStage()
    {
        ++Stage;
    }

    public void AddStageAttr(int idx)
    {
        StageAttrs[idx] += StageAttrAdd[idx];

        UpdateAttrs();
    }

    public List<int> GetStageAttrMax()
    {
        if (Stage == 0)
        {
            return SummonRecord.Stage1AttrMax;
        }
        else if (Stage == 1)
        {
            return SummonRecord.Stage2AttrMax;
        }
        else if (Stage == 2)
        {
            return SummonRecord.Stage3AttrMax;
        }
        else if (Stage == 3)
        {
            return SummonRecord.Stage4AttrMax;
        }
        else if (Stage == 4)
        {
            return SummonRecord.Stage5AttrMax;
        }

        return null;
    }

    #endregion

    #region attr

    public static List<RoleAttrEnum> StageAttrEnums = new List<RoleAttrEnum>()
    { RoleAttrEnum.Attack, RoleAttrEnum.CriticalHitDamge, RoleAttrEnum.CriticalHitChance, RoleAttrEnum.RiseHandSpeed, RoleAttrEnum.AttackSpeed};

    public static List<int> StageAttrAdd = new List<int>()
    { 1, 100,100,100,100 };

    private List<EquipExAttr> _SummonAttrs;
    public List<EquipExAttr> SummonAttrs
    {
        get
        {
            if (_SummonAttrs == null)
            {
                UpdateAttrs();
            }
            return _SummonAttrs;
        }
    }

    public void UpdateAttrs()
    {
        _SummonAttrs = new List<EquipExAttr>();
        InitStageAttr();

        var levelAttr = Tables.TableReader.SummonSkillAttr.GetRecord(Level.ToString());

        var atkValue = (int)(levelAttr.Attr[0] * SummonRecord.AttrModelfy) + StageAttrs[0];
        var critiDmgValue = (int)(levelAttr.Attr[1]) + StageAttrs[1];
        var critiRateValue = (int)(levelAttr.Attr[2]) + StageAttrs[2];
        var riseHandSpeedValue = (int)(levelAttr.Attr[3]) + StageAttrs[3];
        var atkSpeedValue = (int)(levelAttr.Attr[4]) + StageAttrs[4];

        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[0], atkValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[1], critiDmgValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[2], critiRateValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[3], riseHandSpeedValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[4], atkSpeedValue));
    }

    public string GetAttrCostItem(int idx)
    {
        string costItem = "";
        if (idx == 0)
        {
            costItem = SummonRecord.StageCostItems[0].ToString();
        }
        else if (idx == 1 || idx == 2)
        {
            costItem = SummonRecord.StageCostItems[1].ToString();
        }
        else if (idx == 3 || idx == 4)
        {
            costItem = SummonRecord.StageCostItems[2].ToString();
        }

        return costItem;
    }
    #endregion
}

public class SummonSkillData : SaveItemBase
{
    #region 唯一

    private static SummonSkillData _Instance = null;
    public static SummonSkillData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new SummonSkillData();
            }
            return _Instance;
        }
    }

    private SummonSkillData()
    {
        _SaveFileName = "SummonSkillData";
    }

    #endregion

    public void InitSummonSkillData()
    {
        bool needSave = false;

        needSave |= InitSummonMotions();
        needSave |= InitUsingSummon();
        InitCollection();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region pack

    public const int _MAX_PACK_SUMMON_NUM = 100;

    [SaveField(1)]
    public List<SummonMotionData> _SummonMotionList = new List<SummonMotionData>();

    private bool InitSummonMotions()
    {
        return false;
    }

    #endregion

    #region fight

    public const int USING_SUMMON_NUM = 3;

    [SaveField(2)]
    private List<int> _UsingSummonIds = new List<int>();

    public List<SummonMotionData> _UsingSummon;

    private bool InitUsingSummon()
    {
        if (_UsingSummonIds.Count != USING_SUMMON_NUM)
        {
            _UsingSummonIds.Clear();
            for (int i = 0; i < USING_SUMMON_NUM; ++i)
            {
                _UsingSummonIds.Add(-1);
            }
        }

        _UsingSummon = new List<SummonMotionData>();
        for (int i = 0; i < _UsingSummonIds.Count; ++i)
        {
            if (_UsingSummonIds[i] < 0)
            {
                _UsingSummon.Add(null);
            }
            else if (_SummonMotionList.Count > _UsingSummonIds[i])
            {
                _UsingSummon.Add(_SummonMotionList[_UsingSummonIds[i]]);
            }
        }

        return false;
    }

    public void SetUsingSummon(int idx, SummonMotionData summonData)
    {
        _UsingSummon[idx] = summonData;
        RefreshUsingIdx();
    }

    private void RefreshUsingIdx()
    {
        _UsingSummonIds.Clear();

        for (int i = 0; i < _UsingSummon.Count; ++i)
        {
            if (_UsingSummon[i] == null)
            {
                _UsingSummonIds.Add(-1);
            }
            else
            {
                int index = _SummonMotionList.IndexOf(_UsingSummon[i]);
                _UsingSummonIds.Add(index);
            }
        }

        SaveClass(true);
    }

    #endregion

    #region lottery

    public static int _GoldCostOne = 2000;
    public static int _DiamondCostOne = 200;
    public static string _GoldCostItem = "1200001";
    public static string _DiamondCostItem = "1200002";

    public List<SummonMotionData> LotteryGold(int buyTimes)
    {
        var needGold = GetExCostGold(buyTimes);
        if (PlayerDataPack.Instance.Gold < needGold)
        {
            UIMessageTip.ShowMessageTip(20000);
            return null;
        }
        int itemCnt = BackBagPack.Instance.GetItemCnt(_GoldCostItem);
        int needItemNum = buyTimes;
        if (itemCnt < buyTimes)
        {
            needItemNum = itemCnt;
        }

        if (!BackBagPack.Instance.DecItem(_GoldCostItem, needItemNum))
        {
            UIMessageTip.ShowMessageTip(20006);
            return null;
        }

        if (needGold > 0)
        {
            if (!PlayerDataPack.Instance.DecGold(needGold))
            {
                UIMessageTip.ShowMessageTip(20000);
                return null;
            }
        }

        return AddLotteryItems(0, buyTimes);
    }

    public List<SummonMotionData> LotteryDiamond(int buyTimes)
    {
        var needGold = GetExCostDiamond(buyTimes);
        if (PlayerDataPack.Instance.Diamond < needGold)
        {
            UIMessageTip.ShowMessageTip(20001);
            return null;
        }
        int itemCnt = BackBagPack.Instance.GetItemCnt(_DiamondCostItem);
        int needItemNum = buyTimes;
        if (itemCnt < buyTimes)
        {
            needItemNum = itemCnt;
        }

        if (!BackBagPack.Instance.DecItem(_DiamondCostItem, needItemNum))
        {
            UIMessageTip.ShowMessageTip(20006);
            return null;
        }

        if (needGold > 0)
        {
            if (!PlayerDataPack.Instance.DecDiamond(needGold))
            {
                UIMessageTip.ShowMessageTip(20001);
                return null;
            }
        }

        return AddLotteryItems(1, buyTimes);
    }

    public int GetExCostGold(int buyTimes)
    {
        int itemCnt = BackBagPack.Instance.GetItemCnt(_GoldCostItem);
        if (itemCnt >= buyTimes)
        {
            return 0;
        }

        int costMoney = (buyTimes - itemCnt) * _GoldCostOne;
        return costMoney;
    }

    public int GetExCostDiamond(int buyTimes)
    {
        int itemCnt = BackBagPack.Instance.GetItemCnt(_DiamondCostItem);
        if (itemCnt >= buyTimes)
        {
            return 0;
        }

        int costMoney = (buyTimes - itemCnt) * _DiamondCostOne;
        return costMoney;
    }

    private List<SummonMotionData> AddLotteryItems(int isGold, int times)
    {
        Debug.Log("AddLotteryItems:" + isGold + ", times:" + times);
        List<SummonMotionData> summonList = new List<SummonMotionData>();
        for (int i = 0; i < times; ++i)
        {
            int summonIdx = -1;
            if (isGold == 0)
            {
                summonIdx = GameRandom.GetRandomLevel(TableReader.SummonSkillLottery.GoldRates);
            }
            else if (isGold == 1)
            {
                summonIdx = GameRandom.GetRandomLevel(TableReader.SummonSkillLottery.DiamondRates);
            }

            SummonMotionData summonData = new SummonMotionData(TableReader.SummonSkillLottery.RecordsList[summonIdx].SummonSkill.Id);
            summonList.Add(summonData);
            _SummonMotionList.Add(summonData);
        }
        SaveClass(true);
        return summonList;
    }

    #endregion

    #region collection

    public static float _SummonSkillBaseCD = 25.0f;
    public static float _SummonCommonCD = 2.0f;
    public static float _SummonCDDecrease = 0.1f;

    private float _SummonSkillCD = -1;
    public float SummonSkillCD
    {
        get
        {
            if (_SummonSkillCD < 0)
            {
                RefreshCD();
            }
            return _SummonSkillCD;
        }
    }

    private void RefreshCD()
    {
        _SummonSkillCD = _SummonSkillBaseCD - _SummonCDDecrease * _TotalCollectStars;
    }

    public List<SummonCollectItem> _CollectionItems = new List<SummonCollectItem>();
    public int _TotalCollectStars = 0;

    public void RefreshTotalStar()
    {
        int _TotalStars = 0;
        foreach (var starDict in _CollectionItems)
        {
            if (starDict._Star > 0)
            {
                _TotalStars += starDict._Star;
            }
        }
        RefreshCD();
    }

    public void InitCollection()
    {
        List<SummonSkillRecord> recordKeys = new List<SummonSkillRecord>(TableReader.SummonSkill.Records.Values);
        recordKeys.Sort((recordA, recordB) =>
        {
            if (recordA.Quality > recordB.Quality)
            {
                return 1;
            }
            else if (recordA.Quality > recordB.Quality)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        foreach (var record in recordKeys)
        {
            if (record.Quality > ITEM_QUALITY.BLUE)
            {
                SummonCollectItem collectItem = new SummonCollectItem();
                collectItem._SummonRecord = record;
                collectItem._Star = -1;
                _CollectionItems.Add(collectItem);
            }
        }

        RefreshCollection();
    }

    public void RefreshCollection()
    {
        foreach (var record in _CollectionItems)
        {
            foreach (var summonMotion in _SummonMotionList)
            {
                if (record._SummonRecord.Id == summonMotion.SummonRecordID)
                {
                    record._Star = summonMotion.StarLevel;
                }
            }
        }
        RefreshTotalStar();
    }

    public void RefreshCollection(SummonMotionData summonMotion)
    {
        var collectItem = _CollectionItems.Find((collect) =>
        {
            if (collect._SummonRecord.Id == summonMotion.SummonRecordID)
            {
                return true;
            }
            else
            {
                return false;
            }
        });

        if (collectItem._Star < summonMotion.StarLevel)
        {
            collectItem._Star = summonMotion.StarLevel;
        }
        RefreshTotalStar();
    }

    public void RefreshCollection(List<SummonMotionData> summonMotions)
    {
        foreach (var summonMotion in summonMotions)
        {
            RefreshCollection(summonMotion);
        }
        RefreshTotalStar();
    }
    #endregion

    #region opt 

    public void SellSummonItem(SummonMotionData summonData)
    {
        _SummonMotionList.Remove(summonData);

        RefreshUsingIdx();
        //SaveClass(true);
    }

    public int LevelUpSummonItem(SummonMotionData summonData, List<SummonMotionData> expItems)
    {
        int exp = GetItemsExp(expItems);

        int starCnt = 0;
        int expItemCnt = expItems.Count;
        for (int i = 0; i < expItemCnt; ++i)
        {
            _SummonMotionList.Remove(expItems[i]);
            if (expItems[i].SummonRecordID == summonData.SummonRecordID)
            {
                ++starCnt;
            }
        }

        summonData.AddExp(exp);
        summonData.AddStarExp(starCnt);

        RefreshUsingIdx();
        //SaveClass(true);

        UISummonSkillPack.RefreshPack();

        return exp;
    }

    public int GetItemsExp(List<SummonMotionData> expItems)
    {
        int exp = 0;
        int expItemCnt = expItems.Count;
        for (int i = 0; i < expItemCnt; ++i)
        {
            exp += expItems[i].Exp + (int)expItems[i].SummonRecord.Quality * 2 + 1;
        }

        return exp;
    }

    public void SetLockSummonItem(SummonMotionData summonData, bool isLock)
    {
        summonData.IsLock = isLock;

        SaveClass(true);
    }

    public void SortSummonMotionsInExp(List<SummonMotionData> summonMotions, SummonMotionData sameMotion = null)
    {
        summonMotions.Sort((motionA, motionB) =>
        {

            if (sameMotion != null && motionA.SummonRecordID == sameMotion.SummonRecordID && motionB.SummonRecordID != sameMotion.SummonRecordID)
            {
                return -1;
            }
            else if (sameMotion != null && motionA.SummonRecordID != sameMotion.SummonRecordID && motionB.SummonRecordID == sameMotion.SummonRecordID)
            {
                return 1;
            }
            else if (motionA.IsLock && !motionB.IsLock)
            {
                return -1;
            }
            else if (!motionA.IsLock && motionB.IsLock)
            {
                return 1;
            }
            else
            {
                if (motionA.SummonRecord.Quality > motionB.SummonRecord.Quality)
                {
                    return 1;
                }
                else if (motionA.SummonRecord.Quality < motionB.SummonRecord.Quality)
                {
                    return -1;
                }
                else
                {
                    if (motionA.StarExp > motionB.StarExp)
                    {
                        return 1;
                    }
                    else if (motionA.StarExp < motionB.StarExp)
                    {
                        return -1;
                    }
                    else
                    {
                        if (motionA.Level > motionB.Level)
                        {
                            return 1;
                        }
                        else if (motionA.Level < motionB.Level)
                        {
                            return -1;
                        }
                        else
                        {
                            int motionAID = int.Parse(motionA.SummonRecordID);
                            int motionBID = int.Parse(motionB.SummonRecordID);
                            if (motionAID > motionBID)
                            {
                                return 1;
                            }
                            else if (motionAID < motionBID)
                            {
                                return -1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                }
            }
        });
    }

    public void SortSummonMotionsInPack(List<SummonMotionData> summonMotions, SummonMotionData sameMotion = null)
    {
        summonMotions.Sort((motionA, motionB) =>
        {
            if (motionA.SummonRecord.Quality > motionB.SummonRecord.Quality)
            {
                return -1;
            }
            else if (motionA.SummonRecord.Quality < motionB.SummonRecord.Quality)
            {
                return 1;
            }
            else
            {
                if (motionA.StarExp > motionB.StarExp)
                {
                    return -1;
                }
                else if (motionA.StarExp < motionB.StarExp)
                {
                    return 1;
                }
                else
                {
                    if (motionA.Level > motionB.Level)
                    {
                        return -1;
                    }
                    else if (motionA.Level < motionB.Level)
                    {
                        return 1;
                    }
                    else
                    {
                        int motionAID = int.Parse(motionA.SummonRecordID);
                        int motionBID = int.Parse(motionB.SummonRecordID);
                        if (motionAID > motionBID)
                        {
                            return -1;
                        }
                        else if (motionAID < motionBID)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            
        });
    }

    public void StageLevelUp(SummonMotionData summonData)
    {
        var maxAttrs = summonData.GetStageAttrMax();
        for (int i = 0; i < maxAttrs.Count; ++i)
        {
            if (summonData.StageAttrs[i] < maxAttrs[i])
            {
                UIMessageTip.ShowMessageTip(1200003);
                return;
            }
        }

        string costItemID = "";
        int costItemCnt = 0;
        if (IsStageLvUpItemEnough(summonData, out costItemID, out costItemCnt))
        {
            if (BackBagPack.Instance.DecItem(costItemID, costItemCnt))
            {
                summonData.AddStage();
                SaveClass(true);
            }
        }
        else
        {
            UIMessageTip.ShowMessageTip(20006);
        }
    }

    public bool IsStageLvUpItemEnough(SummonMotionData summonData, out string costItemID, out int costItemCnt)
    {
        string costItem = "";
        if (summonData.Stage == 0)
        {
            costItem = summonData.SummonRecord.StageCostItems[0].ToString();
        }
        else if (summonData.Stage == 1 || summonData.Stage == 2)
        {
            costItem = summonData.SummonRecord.StageCostItems[1].ToString();
        }
        else if (summonData.Stage == 3)
        {
            costItem = summonData.SummonRecord.StageCostItems[2].ToString();
        }

        int costCnt = summonData.SummonRecord.StageCostCnt[summonData.Stage];

        costItemID = costItem;
        costItemCnt = costCnt;

        var itemCnt = BackBagPack.Instance.GetItemCnt(costItem);
        return itemCnt >= costItemCnt;
    }

    public void StageAddAttr(SummonMotionData summonData, int idx)
    {
        summonData.InitStageAttr();
        string costItem = "";
        if (idx == 0)
        {
            costItem = summonData.SummonRecord.StageCostItems[0].ToString();
        }
        else if (idx == 1 || idx == 2)
        {
            costItem = summonData.SummonRecord.StageCostItems[1].ToString();
        }
        else if (idx == 3 || idx == 4)
        {
            costItem = summonData.SummonRecord.StageCostItems[2].ToString();
        }

        var itemCnt = BackBagPack.Instance.GetItemCnt(costItem);
        if (itemCnt < 1)
        {
            UIMessageTip.ShowMessageTip(20006);
            return;
        }

        var maxAttrs = summonData.GetStageAttrMax();
        int maxValue = maxAttrs[idx];
        int curValue = summonData.StageAttrs[idx];
        if (curValue >= maxValue)
        {
            UIMessageTip.ShowMessageTip(1200002);
            return;
        }

        if (BackBagPack.Instance.DecItem(costItem, 1))
        {
            summonData.AddStageAttr(idx);
            SaveClass(true);
        }
    }
    #endregion

}
