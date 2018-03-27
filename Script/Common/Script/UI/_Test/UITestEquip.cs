
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

    int _StageIdx = 0;
    int _Diff = 1;

    int _TotalExp = 0;
    int _TotalGold = 0;

    public void OnTestPassStage()
    {
        int exp = 0;
        int gold = 0;

        ++_StageIdx;
        if (_StageIdx > 20)
        {
            _StageIdx = 1;
            ++_Diff;
        }
        TestPassNormalStage(_StageIdx, _Diff, ref exp, ref gold);

        _TotalExp += exp;
        _TotalGold += gold;

        Debug.Log("TotalDrop Exp:" + _TotalExp + ", Gold:" + _TotalGold);
        Debug.Log("Role Atk:" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack) + ", Def:" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Defense) + ", HP:" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax));
    }

    public void OnTestDelEquips()
    {
        TestFight.DelAllEquip();
    }

    private void TestPassNormalStage(int stageIdx, int diff, ref int exp, ref int gold)
    {
        var stageRecord = TableReader.StageInfo.GetRecord(stageIdx.ToString());
        int level = GameDataValue.GetStageLevel(diff, stageIdx, STAGE_TYPE.NORMAL);
        var sceneGO = ResourceManager.Instance.GetGameObject("FightSceneLogic/" + stageRecord.FightLogicPath);
        var areas = sceneGO.GetComponentsInChildren<FightSceneAreaKAllEnemy>(true);
        var bossAreas = sceneGO.GetComponentInChildren<FightSceneAreaKBossWithFish>(true);
        List<string> monsterIds = new List<string>();
        int eliteCnt = 0;
        foreach (var enemyArea in areas)
        {
            for(int i = 0; i< enemyArea._EnemyBornPos.Length - 1; ++i)
            {
                monsterIds.Add(enemyArea._EnemyBornPos[i]._EnemyDataID);
            }

            var monLastId = enemyArea._EnemyBornPos[enemyArea._EnemyBornPos.Length - 1]._EnemyDataID;
            if (diff > 1)
            {
                var monId = TableReader.MonsterBase.GetGroupElite(TableReader.MonsterBase.GetRecord(monLastId));
                monsterIds.Add(monId.Id);
                ++eliteCnt;
            }
            else
            {
                monsterIds.Add(monLastId);
            }
        }
        monsterIds.Add(bossAreas._BossMotionID);

        Dictionary<string, int> items = new Dictionary<string, int>();
        foreach (var monId in monsterIds)
        {
            var monRecord = TableReader.MonsterBase.GetRecord(monId);
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, level);
            foreach (var dropItem in monsterDrops)
            {
                if (dropItem._DropGold > 0)
                {
                    //Debug.Log("drop gold:" + dropItem._DropGold);
                    gold += dropItem._DropGold;
                }
                else if(dropItem._ItemBase != null)
                {
                    //Debug.Log("drop item:" + dropItem._ItemBase.ItemDataID + "," + dropItem._ItemBase.ItemStackNum);
                    if (!items.ContainsKey(dropItem._ItemBase.ItemDataID))
                    {
                        items.Add(dropItem._ItemBase.ItemDataID, 0);
                    }
                    items[dropItem._ItemBase.ItemDataID] += dropItem._ItemBase.ItemStackNum;
                }
                else if (dropItem._ItemEquip != null)
                {
                    MonsterDrop.PickItem(dropItem);
                }
            }
            int dropExp = GameDataValue.GetMonsterExp(monRecord.MotionType, level, 1);
            exp += dropExp;
        }
        TestFight.DelAllEquip();
        //foreach (var dropItem in items)
        //{
        //    Debug.Log("Drop Item :" + dropItem.Key + "," + dropItem.Value);
        //}
        Debug.Log("Drop Exp:" + exp + ", Gold:" + gold);
    }

    #endregion
}

