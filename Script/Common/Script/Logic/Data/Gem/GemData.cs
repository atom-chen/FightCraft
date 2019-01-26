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

public class GemData
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
        
    }

    #endregion

    public void InitGemData()
    {
        InitGemContainer();
        InitGemPack();
        
    }

    #region gem pack

    public const int MAX_GEM_EQUIP = 6;

    private ItemPackBase<ItemGem> _EquipedGemDatas;
    public ItemPackBase<ItemGem> EquipedGemDatas
    {
        get
        {
            return _EquipedGemDatas;
        }
    }

    public void SaveEquipGem()
    {
        EquipedGemDatas.SaveClass(true);
    }

    public bool InitGemPack()
    {
        _EquipedGemDatas = new ItemPackBase<ItemGem>();
        _EquipedGemDatas._SaveFileName = "GemDataEquiped";
        _EquipedGemDatas._PackSize = MAX_GEM_EQUIP;
        _EquipedGemDatas.LoadClass(true);
        
        if (_EquipedGemDatas._PackItems == null || _EquipedGemDatas._PackItems.Count < MAX_GEM_EQUIP)
        {
            if (_EquipedGemDatas._PackItems == null)
            {
                _EquipedGemDatas._PackItems = new List<ItemGem>();
                _EquipedGemDatas._PackSize = MAX_GEM_EQUIP;
            }
            int equipSlotCnt = MAX_GEM_EQUIP;
            int startIdx = _EquipedGemDatas._PackItems.Count;
            for (int i = startIdx; i < equipSlotCnt; ++i)
            {
                ItemGem newItemGem = new ItemGem("-1");
                _EquipedGemDatas._PackItems.Add(newItemGem);
            }
            _EquipedGemDatas.SaveClass(true);
        }

        GemSuit.Instance.IsActSet();
        return false;
    }

    public int GetPutOnIdx()
    {
        for (int i = 0; i < EquipedGemDatas._PackItems.Count; ++i)
        {
            if (!EquipedGemDatas._PackItems[i].IsVolid())
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

        var equipedGem = EquipedGemDatas._PackItems.Find((gemItem) =>
        {
            if (gemItem.IsVolid() && gemItem.GemRecord.Class == gem.GemRecord.Class)
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

        var putOnSlot = EquipedGemDatas._PackItems[slot];
        if (putOnSlot == null || putOnSlot.IsVolid())
        {
            UIMessageTip.ShowMessageTip(30001);
            return false;
        }

        if (gem.ItemStackNum < 1)
            return false;

        putOnSlot.ItemDataID = gem.ItemDataID;
        putOnSlot.AddStackNum(1);

        PackGemDatas.DecItem(gem, 1);

        PackGemDatas.SaveClass(true);

        GemSuit.Instance.IsActSet();

        RoleData.SelectRole.CalculateAttr();
        return true;
    }

    public bool PutOff(ItemGem gem)
    {
        if (!PackGemDatas.AddItem(gem.ItemDataID, 1))
        {
            UIMessageTip.ShowMessageTip(10002);
            return false;
        }

        gem.DecStackNum(1);
        PackGemDatas.SaveClass(true);

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
        var gemData = EquipedGemDatas.GetItem(gemDataID);
        return gemData != null;
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

        if (_PackGemDatas._PackItems == null)
        {
            _PackGemDatas._PackItems = new List<ItemGem>();
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
        for (int i = 0; i < EquipedGemDatas._PackItems.Count; ++i)
        {
            if (EquipedGemDatas._PackItems[i] == null)
                continue;

            if (!EquipedGemDatas._PackItems[i].IsVolid())
                continue;

            if (EquipedGemDatas._PackItems[i].GemAttr.AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)EquipedGemDatas._PackItems[i].GemAttr.AttrParams[0], EquipedGemDatas._PackItems[i].GemAttr.AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(EquipedGemDatas._PackItems[i].GemAttr));
            }
        }

        GemSuit.Instance.SetGemSetAttr(roleAttr);
       
    }

    public ItemGem GetGemClassMax(int classID, int minLevel)
    {
        ItemGem classItem = null;
        foreach (var gemData in PackGemDatas._PackItems)
        {
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

        foreach (var gemData in EquipedGemDatas._PackItems)
        {
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
}
