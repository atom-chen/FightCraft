using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class SummonCollectItem
{
    public SummonSkillRecord _SummonRecord;
    public int _Star;
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

    public ItemPackBase<SummonMotionData> _SummonMotionList;

    public ItemPackBase<SummonMotionData> _SummonMatList;

    private bool InitSummonMotions()
    {
        _SummonMotionList = new ItemPackBase<SummonMotionData>();
        _SummonMotionList._SaveFileName = "PackSummonMotions";
        _SummonMotionList._PackSize = -1;
        _SummonMotionList.LoadClass(true);

        if (_SummonMotionList._PackItems == null)
        {
            _SummonMotionList._PackItems = new List<SummonMotionData>();
            _SummonMotionList.SaveClass(true);
        }

        _SummonMatList = new ItemPackBase<SummonMotionData>();
        _SummonMatList._SaveFileName = "PackSummonMaterials";
        _SummonMatList._PackSize = -1;
        _SummonMatList.LoadClass(true);

        if (_SummonMatList._PackItems == null)
        {
            _SummonMatList._PackItems = new List<SummonMotionData>();
            _SummonMatList.SaveClass(true);
        }
        return false;
    }

    #endregion

    #region fight

    public const int USING_SUMMON_NUM = 3;

    [SaveField(1)]
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
            else if (_SummonMotionList._PackItems.Count > _UsingSummonIds[i])
            {
                _UsingSummon.Add(_SummonMotionList._PackItems[_UsingSummonIds[i]]);
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
                int index = _SummonMotionList._PackItems.IndexOf(_UsingSummon[i]);
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
        int itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(_GoldCostItem);
        int needItemNum = buyTimes;
        if (itemCnt < buyTimes)
        {
            needItemNum = itemCnt;
        }

        if (!BackBagPack.Instance.PageItems.DecItem(_GoldCostItem, needItemNum))
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
        int itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(_DiamondCostItem);
        int needItemNum = buyTimes;
        if (itemCnt < buyTimes)
        {
            needItemNum = itemCnt;
        }

        if (!BackBagPack.Instance.PageItems.DecItem(_DiamondCostItem, needItemNum))
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
        int itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(_GoldCostItem);
        if (itemCnt >= buyTimes)
        {
            return 0;
        }

        int costMoney = (buyTimes - itemCnt) * _GoldCostOne;
        return costMoney;
    }

    public int GetExCostDiamond(int buyTimes)
    {
        int itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(_DiamondCostItem);
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

            SummonMotionData summonData = AddSummonData(TableReader.SummonSkillLottery.RecordsList[summonIdx].SummonSkill.Id);
            summonList.Add(summonData);
        }
        _SummonMotionList.SaveClass(true);
        _SummonMatList.SaveClass(true);

        RefreshCollection(summonList);
        return summonList;
    }

    public SummonMotionData AddSummonData(string summonID)
    {
        SummonMotionData summonData = new SummonMotionData(summonID);
        var listItem = _SummonMotionList.GetItem(summonID);
        if (listItem == null && summonData.SummonRecord.Quality > ITEM_QUALITY.GREEN)
        {
            _SummonMotionList.AddItem(summonData);
        }
        else
        {
            _SummonMatList.AddItem(summonID, 1);
        }


        return summonData;
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
        foreach (var starDict in _CollectionItems)
        {
            if (starDict._Star > 0)
            {
                _TotalCollectStars += starDict._Star;
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
            foreach (var summonMotion in _SummonMotionList._PackItems)
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

        if (collectItem == null)
            return;

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
        _SummonMatList.DecItem(summonData);

        RefreshUsingIdx();
        //SaveClass(true);
    }

    public int LevelUpSummonItem(SummonMotionData summonData, Dictionary<SummonMotionData, int> expItems)
    {
        int exp = GetItemsExp(expItems);

        int starCnt = 0;
        
        foreach(var expItem in expItems)
        {
            _SummonMatList.DecItem(expItem.Key, expItem.Value);
            if (expItem.Key.SummonRecordID == summonData.SummonRecordID)
            {
                starCnt += expItem.Value;
            }
        }

        _SummonMatList.SaveClass(true);
        summonData.AddExp(exp);
        summonData.AddStarExp(starCnt);
        summonData.SaveClass(true);

        RefreshUsingIdx();
        //SaveClass(true);

        UISummonSkillPack.RefreshPack();

        return exp;
    }

    public int GetItemsExp(Dictionary<SummonMotionData, int> expItems)
    {
        int exp = 0;
        int expItemCnt = expItems.Count;
        foreach(var expItem in expItems)
        {
            exp += (expItem.Key.Exp + (int)expItem.Key.SummonRecord.Quality * 2 + 1) * expItem.Value;
        }

        return exp;
    }

    public int GetItemsStarExp(SummonMotionData motionData, Dictionary<SummonMotionData, int> expItems)
    {
        int starCnt = 0;
        foreach (var expItem in expItems)
        {
            if (expItem.Key.ItemDataID == motionData.ItemDataID)
            {
                starCnt += expItem.Value;
            }
        }

        return starCnt;
    }

    public void SetLockSummonItem(SummonMotionData summonData, bool isLock)
    {
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

    /*
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
            if (BackBagPack.Instance.PageItems.DecItem(costItemID, costItemCnt))
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

        var itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(costItem);
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

        var itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(costItem);
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

        if (BackBagPack.Instance.PageItems.DecItem(costItem, 1))
        {
            summonData.AddStageAttr(idx);
            SaveClass(true);
        }
    }
    */
    #endregion

}
