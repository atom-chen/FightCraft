
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UITestEquip : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UITestEquip", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public InputField _LegencyID;
    public InputField _InputLevel;
    public InputField _InputQuality;
    public InputField _InputValue;

    public InputField _ItemID;
    public InputField _ItemCnt;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
    }

    public override void Hide()
    {
        base.Hide();

        UIEquipTooltips.HideAsyn();
    }

    public void OnBtnOk()
    {
        int level = int.Parse(_InputLevel.text);
        ITEM_QUALITY quality = (ITEM_QUALITY)(int.Parse(_InputQuality.text));
        int value = int.Parse(_InputValue.text);
        int legencyID = 0;
        if (!int.TryParse(_LegencyID.text, out legencyID))
        {
            legencyID = 0;
        }
        var equipItem = ItemEquip.CreateEquip(level, quality, value, legencyID);
        var newEquip = BackBagPack.Instance.AddNewEquip(equipItem);
        //UIEquipTooltips.ShowAsyn(newEquip);
        Debug.Log("test equip :" + newEquip.EquipItemRecord.Id);
    }

    public void OnBtnItem()
    {
        int itemCnt = int.Parse(_ItemCnt.text);

        ItemBase.CreateItemInPack(_ItemID.text, itemCnt);
    }
    #endregion

    #region 

    public InputField _TestLevel;

    public void OnBtnEquipNumaricTest()
    {
        int level = int.Parse(_TestLevel.text);
        RoleAttrStruct roleAttr = new RoleAttrStruct();
        roleAttr.ResetBaseAttr();
        for (int i = 0; i < (int)EQUIP_SLOT.RING + 1; ++i)
        {
            var equipItem = ItemEquip.CreateEquip(level, ITEM_QUALITY.PURPER, GameDataValue.CalLvValue(level), 0, i);
            equipItem.SetEquipAttr(roleAttr);
        }

        for (int i = 0; i < (int)RoleAttrEnum.BASE_ATTR_MAX; ++i)
        {
            var value = roleAttr.GetValue((RoleAttrEnum)i);
            if (value > 0)
            {
                Debug.Log(((RoleAttrEnum)i).ToString() + ":" + value);
            }
        }
    }

    #endregion

    #region test res

    public void OnBtnRes()
    {
        BackBagPack.Instance.AddItem("70100", 9999);
        BackBagPack.Instance.AddItem("70101", 9999);
        BackBagPack.Instance.AddItem("70102", 9999);
        BackBagPack.Instance.AddItem("70103", 9999);
        BackBagPack.Instance.AddItem("70104", 9999);
        BackBagPack.Instance.AddItem("70105", 9999);
        BackBagPack.Instance.AddItem("20000", 9999);

        PlayerDataPack.Instance.AddGold(9999999);
        PlayerDataPack.Instance.AddDiamond(9999999);
    }

    #endregion

    #region test exp

    public InputField _TestExp;

    public void OnBtnTestExp()
    {
        int level = int.Parse(_TestExp.text);
        //int monLevel = Mathf.Min(100, level);
        //var levelExp = GameDataValue.GetLvUpExp(level, 0);
        //var stageExp = 0;
        //for (int i = 0; i < 100; ++i)
        //{
        //    stageExp += GameDataValue.GetMonsterExp(MotionType.Normal, monLevel, monLevel);
        //}
        //for (int i = 0; i < 15; ++i)
        //{
        //    stageExp += GameDataValue.GetMonsterExp(MotionType.Elite, monLevel, monLevel);
        //}
        //stageExp += GameDataValue.GetMonsterExp(MotionType.Hero, monLevel, monLevel);
        //Debug.Log("LvUpExp:" + levelExp + ", StageExp:" + stageExp);

        var bossRecord = TableReader.MonsterBase.GetRecord("2");
        var normalRecord = TableReader.MonsterBase.GetRecord("21");
        //for (int i = 0; i < 100; ++i)
        //{
        //    var equipList = GameDataValue.GetMonsterDropEquipNormal(MotionType.Normal, bossRecord, level);
        //    foreach (var equip in equipList)
        //    {
        //        var newEquip = BackBagPack.Instance.AddNewEquip(equip);
        //    }
        //}
        //for (int i = 0; i < 15; ++i)
        //{
        //    var equipList2 = GameDataValue.GetMonsterDropEquipNormal(MotionType.Elite, bossRecord, level);
        //    foreach (var equip in equipList2)
        //    {
        //        var newEquip = BackBagPack.Instance.AddNewEquip(equip);
        //    }
        //}
        //var equipList3 = GameDataValue.GetMonsterDropEquipNormal(MotionType.Hero, bossRecord, level);
        //foreach (var equip in equipList3)
        //{
        //    var newEquip = BackBagPack.Instance.AddNewEquip(equip);
        //}

        int dropMatCnt = 0;
        for (int i = 0; i < 100; ++i)
        {
            dropMatCnt += GameDataValue.GetEquipMatDropCnt(MotionType.Normal, bossRecord, level);
        }
        for (int i = 0; i < 15; ++i)
        {
            dropMatCnt += GameDataValue.GetEquipMatDropCnt(MotionType.Elite, bossRecord, level);
        }
        dropMatCnt += GameDataValue.GetEquipMatDropCnt(MotionType.Special, bossRecord, level);
        dropMatCnt += GameDataValue.GetEquipMatDropCnt(MotionType.Hero, bossRecord, level);
        Debug.Log("DropMatCnt:" + dropMatCnt);

        Dictionary<string, int> _DropGemMats = new Dictionary<string, int>();
        for (int i = 0; i < 100; ++i)
        {
            var gemType = GameDataValue.GetGemMatDropItemID(normalRecord);
            var gemCnt = GameDataValue.GetGemMatDropCnt(MotionType.Normal, bossRecord, level);
            if (!_DropGemMats.ContainsKey(gemType))
            {
                _DropGemMats.Add(gemType, 0);
            }
            _DropGemMats[gemType] += gemCnt;
        }
        for (int i = 0; i < 15; ++i)
        {
            var gemType = GameDataValue.GetGemMatDropItemID(normalRecord);
            var gemCnt = GameDataValue.GetGemMatDropCnt(MotionType.Elite, bossRecord, level);
            if (!_DropGemMats.ContainsKey(gemType))
            {
                _DropGemMats.Add(gemType, 0);
            }
            _DropGemMats[gemType] += gemCnt;
        }
        var gemTypeBoss = GameDataValue.GetGemMatDropItemID(normalRecord);
        var gemCntBoss = GameDataValue.GetGemMatDropCnt(MotionType.Hero, bossRecord, level);
        if (!_DropGemMats.ContainsKey(gemTypeBoss))
        {
            _DropGemMats.Add(gemTypeBoss, 0);
        }
        _DropGemMats[gemTypeBoss] += gemCntBoss;

        foreach (var dropGem in _DropGemMats)
        {
            Debug.Log("DropGem:" + dropGem.Key + "," + dropGem.Value);
        }
    }

    #endregion

    #region 

    public void TestPassStage()
    {

    }

    #endregion
}

