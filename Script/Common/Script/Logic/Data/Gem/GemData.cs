using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class GemLevelInfo
{
    public string MaterialData;
    public int MaterialCnt;
    public int CostMoney;
}

public class GemData : DataPackBase
{
    #region 唯一

    private static GemData _Instance = null;
    public static GemData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GemData();
            }
            return _Instance;
        }
    }

    private GemData()
    {
        _SaveFileName = "GemData";
    }

    #endregion

    public void InitGemData()
    {
        InitGemContainer();
        InitGemPack();
        
    }

    #region gem pack

    public const int MAX_GEM_EQUIP = 5;

    [SaveField(1)]
    public List<string> _EquipGemDataID;

    private List<ItemGem> _EquipedGemDatas;
    public List<ItemGem> EquipedGemDatas
    {
        get
        {
            return _EquipedGemDatas;
        }
    }

    public bool IsEquipedGem(ItemGem itemGem)
    {
        return _EquipedGemDatas.Contains(itemGem);
    }

    public void SaveEquipGem()
    {
        SaveClass(true);
    }

    public void RefreshEquipedGems()
    {
        _EquipedGemDatas = new List<ItemGem>();
        for (int i = 0; i < _EquipGemDataID.Count; ++i)
        {
            _EquipedGemDatas.Add(_PackGemDatas.GetItem(_EquipGemDataID[i]));
        }

        GemSuit.Instance.IsActSet();
    }

    public bool InitGemPack()
    {
        if (_EquipGemDataID == null || _EquipGemDataID.Count == 0)
        {
            _EquipGemDataID = new List<string>();
            for (int i = 0; i < MAX_GEM_EQUIP; ++i)
            {
                _EquipGemDataID.Add("-1");
            }
        }

        RefreshEquipedGems();
        return false;
    }

    public int GetPutOnIdx()
    {
        for (int i = 0; i < EquipedGemDatas.Count; ++i)
        {
            if (EquipedGemDatas[i] == null || !EquipedGemDatas[i].IsVolid())
                return i;
        }
        return -1;
    }

    public bool PutOnGem(ItemGem gem, int slot)
    {
        if (slot< 0 || slot >= MAX_GEM_EQUIP)
        {
            UIMessageTip.ShowMessageTip("gem slot error");
            return false;
        }

        var equipedGem = EquipedGemDatas.Find((gemItem) =>
        {
            if (gemItem != null && gemItem.IsVolid() && gemItem.ItemDataID == gem.ItemDataID)
            {
                return true;
            }
            else
            {
                return false;
            }
        });
        if (equipedGem != null)
        {
            UIMessageTip.ShowMessageTip("allready put on gem");
            return false;
        }

        _EquipGemDataID[slot] = gem.ItemDataID;

        RefreshEquipedGems();

        SaveClass(true);
        PackGemDatas.SaveClass(true);

        GemSuit.Instance.IsActSet();

        RoleData.SelectRole.CalculateAttr();
        return true;
    }

    public bool PutOff(ItemGem gem)
    {

        int equipSlot = -1;
        for (int i = 0; i < _EquipedGemDatas.Count; ++i)
        {
            if (_EquipedGemDatas[i] == gem)
            {
                equipSlot = i;
                break;
            }
        }
        if (equipSlot < 0)
            return false;

        _EquipGemDataID[equipSlot] = "-1";
        RefreshEquipedGems();

        PackGemDatas.SaveClass(true);
        SaveClass(true);

        RoleData.SelectRole.CalculateAttr();
        return true;
    }

    public void ExchangeGem(ItemGem gem1, ItemGem gem2)
    {
        gem1.ExchangeInfo(gem2);
        GemSuit.Instance.IsActSet();

        RoleData.SelectRole.CalculateAttr();
    }

    public bool IsEquipedGem(string gemDataID)
    {
        for (int i = 0; i < _EquipGemDataID.Count; ++i)
        {
            if (_EquipGemDataID[i] == gemDataID)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region gem container

    public const int MAX_GEM_PACK = -1;

    private ItemPackBase<ItemGem> _PackGemDatas;
    public ItemPackBase<ItemGem> PackGemDatas
    {
        get
        {
            return _PackGemDatas;
        }
    }

    private void InitGemContainer()
    {
        _PackGemDatas = new ItemPackBase<ItemGem>();
        _PackGemDatas._SaveFileName = "GemDataPack";
        _PackGemDatas._PackSize = MAX_GEM_PACK;
        _PackGemDatas.LoadClass(true);

        if (_PackGemDatas._PackItems == null || _PackGemDatas._PackItems.Count == 0)
        {
            _PackGemDatas._PackItems = new List<ItemGem>();

            var gemRecordTabs = TableReader.GemTable.Records.Values;
            foreach (var gemRecord in gemRecordTabs)
            {
                ItemGem newItemGem = new ItemGem(gemRecord.Id);
                newItemGem.Level = 0;
                _PackGemDatas._PackItems.Add(newItemGem);
            }

            _PackGemDatas.SaveClass(true);
        }
    }

    public void CreateGem(string gemDataID, int gemCnt)
    {
        PackGemDatas.AddItem(gemDataID, gemCnt);
        PackGemDatas.SaveClass(true);
    }

    public void PacketSort()
    {
        PackGemDatas.SortStack();
        PackGemDatas.SortEmpty();
        PackGemDatas.SaveClass(true);
    }

    public void DecGem(ItemGem itemGem, int num = 1)
    {

    }

    #endregion

    #region combine

    private Dictionary<GemTableRecord, List<int>> _GemFormulas;
    public Dictionary<GemTableRecord, List<int>> GemFormulas
    {
        get
        {
            InitGemFormulas();
            return _GemFormulas;
        }
    }

    private void InitGemFormulas()
    {
        if (_GemFormulas != null)
            return;

        _GemFormulas = new Dictionary<GemTableRecord, List<int>>();
        foreach (var gemRecord in TableReader.GemTable.Records)
        {
            if (gemRecord.Value.Combine[0] < 0)
                continue;

            List<int> gemIds = new List<int>();
            List<int> combines = new List<int>(gemRecord.Value.Combine);
            combines.Sort((idA, idB) =>
            {
                if (idA < idB)
                {
                    return 1;
                }
                else if (idA > idB)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });

            for (int i = 0; i < combines.Count; ++i)
            {
                if (combines[i] > 0)
                {
                    gemIds.Add(combines[i]);
                }
            }

            _GemFormulas.Add(gemRecord.Value, gemIds);
        }
    }

    public bool GemCombine(List<ItemGem> combineGem)
    {
        if (combineGem.Count == 3 && combineGem[0].ItemDataID == combineGem[1].ItemDataID
            && combineGem[0].ItemDataID == combineGem[2].ItemDataID)
        {
            return GemCombineSame(combineGem);
        }
        else
        {
            return GemCombineFormula(combineGem);
        }
    }

    private static List<int> _CombineSameNeedNum = new List<int>() { 3, 9, 27, 81 };
    public void GemCombineSameAll(ItemGem gemDataID)
    {
        int gemMatCnt = PackGemDatas.GetItemCnt(gemDataID.ItemDataID);
        int combineLevel = GemCombineSameLevel(gemMatCnt);
        while (combineLevel > 0)
        {
            var nextGem = GetNextLevelGem(gemDataID.GemRecord.Class, combineLevel);
            if (nextGem == null)
            {
                UIMessageTip.ShowMessageTip(30006);
                return;
            }
            PackGemDatas.DecItem(gemDataID, _CombineSameNeedNum[combineLevel - 1]);
            CreateGem(nextGem.Id, 1);

            gemMatCnt = PackGemDatas.GetItemCnt(gemDataID.ItemDataID);
            combineLevel = GemCombineSameLevel(gemMatCnt);
        }
    }

    private int GemCombineSameLevel(int gemMatCnt)
    {
        for (int i = _CombineSameNeedNum.Count - 1; i >= 0; --i)
        {
            if (gemMatCnt >= _CombineSameNeedNum[i])
                return i + 1;
        }

        return -1;
    }

    private bool GemCombineSame(List<ItemGem> combineGems)
    {
        var nextGem = GetNextLevelGem(combineGems[0].GemRecord.Class, combineGems[0].GemRecord.Level);
        if (nextGem == null)
        {
            UIMessageTip.ShowMessageTip(30006);
            return false;
        }

        if (combineGems[0] == combineGems[1]
            && combineGems[0] == combineGems[2])
        {
            if (combineGems[0].ItemStackNum < 3)
                return false;

            PackGemDatas.DecItem(combineGems[0], 3);
        }
        else if (combineGems[0] == combineGems[1])
        {
            if (combineGems[0].ItemStackNum < 2 || combineGems[2].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[0],2);
            PackGemDatas.DecItem(combineGems[2],1);
        }
        else if (combineGems[0] == combineGems[2])
        {
            if (combineGems[0].ItemStackNum < 2 || combineGems[1].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[0],2);
            PackGemDatas.DecItem(combineGems[1],1);
        }
        else if (combineGems[1] == combineGems[2])
        {
            if (combineGems[1].ItemStackNum < 2 || combineGems[2].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[1],2);
            PackGemDatas.DecItem(combineGems[0],1);
        }
        else
        {
            if (combineGems[0].ItemStackNum < 1 || combineGems[1].ItemStackNum < 1 || combineGems[2].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[0],1);
            PackGemDatas.DecItem(combineGems[1], 1);
            PackGemDatas.DecItem(combineGems[2], 1);
        }
        CreateGem(nextGem.Id, 1);
        return true;
    }

    private bool GemCombineFormula(List<ItemGem> combineGems)
    {
        InitGemFormulas();

        combineGems.Sort((gemA, gemB) =>
        {
            int idA = int.Parse(gemA.ItemDataID);
            int idB = int.Parse(gemB.ItemDataID);
            if (idA < idB)
            {
                return 1;
            }
            else if (idA > idB)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        foreach (var gemFormula in _GemFormulas)
        {
            if (combineGems.Count != gemFormula.Value.Count)
                continue;

            bool fitFormula = true;
            for (int i = 0; i < gemFormula.Value.Count; ++i)
            {
                if (gemFormula.Value[i] != combineGems[i].GemRecord.Class)
                {
                    fitFormula = false;
                    break;
                }
            }

            if (fitFormula)
            {
                for (int i = 0; i < combineGems.Count; ++i)
                {
                    PackGemDatas.DecItem(combineGems[i], 1);
                }
                CreateGem(gemFormula.Key.Id, 1);
                return true;
            }
        }

        return false;
    }

    #endregion

    #region attr

    public void SetGemAttr(RoleAttrStruct roleAttr)
    {
        for (int i = 0; i < EquipedGemDatas.Count; ++i)
        {
            if (EquipedGemDatas[i] == null)
                continue;

            if (!EquipedGemDatas[i].IsVolid())
                continue;

            if (EquipedGemDatas[i].GemAttr.AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)EquipedGemDatas[i].GemAttr.AttrParams[0], EquipedGemDatas[i].GemAttr.AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(EquipedGemDatas[i].GemAttr));
            }
        }

        GemSuit.Instance.SetGemSetAttr(roleAttr);
       
    }

    public ItemGem GetGemClassMax(int classID, int minLevel, List<ItemGem> extraGems)
    {
        ItemGem classItem = null;
        foreach (var gemData in PackGemDatas._PackItems)
        {
            if (extraGems !=null && extraGems.Contains(gemData))
                continue;

            if (gemData.IsVolid() && gemData.GemRecord.Class == classID && gemData.GemRecord.Level >= minLevel)
            {
                if (classItem == null)
                {
                    classItem = gemData;
                }
                else if (classItem.GemRecord.Level < gemData.GemRecord.Level)
                {
                    classItem = gemData;
                }
            }
        }

        foreach (var gemData in EquipedGemDatas)
        {
            if (extraGems != null && extraGems.Contains(gemData))
                continue;

            if (gemData!= null && gemData.IsVolid() && gemData.GemRecord.Class == classID && gemData.GemRecord.Level >= minLevel)
            {
                if (classItem == null)
                {
                    classItem = gemData;
                }
                else if (classItem.GemRecord.Level < gemData.GemRecord.Level)
                {
                    classItem = gemData;
                }
            }
        }

        return classItem;
    }

    public GemTableRecord GetGemByClass(int classType, int level)
    {
        return TableReader.GemTable.GetGemRecordByClass(classType, level);
    }

    public GemTableRecord GetNextLevelGem(int gemClass, int curLevel)
    {
        int nextLv = curLevel + 1;
        foreach (var gemRecord in TableReader.GemTable.Records)
        {
            if (gemRecord.Value.Class == gemClass && gemRecord.Value.Level == nextLv)
            {
                return gemRecord.Value;
            }
        }
        return null;
    }

    public List<GemTableRecord> GetAllLevelGemRecords(int gemClass)
    {
        List<GemTableRecord> gemRecords = new List<GemTableRecord>();
        foreach (var gemData in PackGemDatas._PackItems)
        {
            if (gemData.IsVolid() && gemData.GemRecord.Class == gemClass)
            {
                if (!gemRecords.Contains(gemData.GemRecord))
                {
                    gemRecords.Add(gemData.GemRecord);
                }
            }
        }

        return gemRecords;
    }


    #endregion

    #region level up

    public int GetLevelCostMoney(ItemGem itemGem)
    {
        return GameDataValue.GetGemLevelUpCostMoney(itemGem.Level);
    }

    public int GetLevelCostMat(ItemGem itemGem)
    {
        return GameDataValue.GetGemLevelUpCostMat(itemGem.Level);
    }

    public bool GemLevelUp(ItemGem itemGem)
    {
        if (itemGem.Level >= RoleData.SelectRole.TotalLevel)
        {
            UIMessageTip.ShowMessageTip(30009);
            return false;
        }

        int costMatCnt = GetLevelCostMat(itemGem);
        if (BackBagPack.Instance.PageItems.DecItem(itemGem.ItemDataID, costMatCnt))
        {
            ++itemGem.Level;
            itemGem.RefreshGemAttr();
            itemGem.SaveClass(true);
            return true;
        }
        return false;
    }

    #endregion
}
