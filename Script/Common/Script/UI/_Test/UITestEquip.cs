﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;
using System.IO;

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

    public InputField _TargetLevel;

    public void AutoLevel()
    {
        int targetLevel = int.Parse(_TargetLevel.text);
        int fightTimes = 0;
        while (true)
        {
            var level = RoleData.SelectRole._RoleLevel + RoleData.SelectRole._AttrLevel;
            if (level >= targetLevel)
                break;

            int nextDiff = ActData.Instance._NormalStageDiff;
            if (nextDiff < 1)
            {
                nextDiff = 1;
            }
            int nextStage = ActData.Instance._NormalStageIdx;
            ++nextStage;
            if (nextStage == TableReader.StageInfo.GetMaxNormalStageID() + 1 || nextStage == 0)
            {
                ++nextDiff;
                nextStage = 1;
            }

            int gold = 0;
            int exp = 0;
            TestPassNormalStage(nextStage, nextDiff, ref exp, ref gold);
            ActData.Instance.SetPassNormalStage(nextDiff, nextStage);
            
            ++fightTimes;
        }
        Debug.Log("FightTimes:" + fightTimes);
    }

    int _StageIdx = 0;
    int _Diff = 1;

    int _TotalExp = 0;
    int _TotalGold = 0;

    List<PassStageInfo> _PassInfoList = new List<PassStageInfo>();

    class PassStageInfo
    {
        public int diff;
        public int stateIdx;
        public int level;
        public int exp;
        public int gold;
        public int levelExp;
        public int atk;
        public int damage1;
        public int damage2;
        public int damage3;
        public int def;
        public int hp;
        public int monValue;
        public int equipMat;
        public int equipGem;
    }

    public void OnTestPassStage()
    {
        //    int exp = 0;
        //    int gold = 0;

        //    ++_StageIdx;
        //    if (_StageIdx > 20)
        //    {
        //        _StageIdx = 1;
        //        ++_Diff;
        //    }
        //    TestPassNormalStage(_StageIdx, _Diff, ref exp, ref gold);

        //    _TotalExp += exp;
        //    _TotalGold += gold;

        //    Debug.Log("TotalDrop stage:" + _StageIdx  + " Exp:" + _TotalExp + ", Gold:" + _TotalGold);
        //    Debug.Log("Role Atk:" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack) + ", Def:" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Defense) + ", HP:" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax));

        for (int diff = 1; diff < 7; ++diff)
        {
            for (int stageid = 1; stageid < 21; ++stageid)
            {
                int exp = 0;
                int gold = 0;
                TestPassNormalStage(stageid, diff, ref exp, ref gold);

                PassStageInfo passInfo = new PassStageInfo();
                passInfo.diff = diff;
                passInfo.stateIdx = stageid;
                passInfo.level = GameDataValue.GetStageLevel(diff, stageid, STAGE_TYPE.NORMAL);
                passInfo.exp = exp;
                passInfo.gold = gold;
                passInfo.levelExp = GameDataValue.GetLvUpExp(passInfo.level, 0);
                passInfo.atk = GameDataValue.GetPhyDamage(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack), 1, RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.PhysicDamageEnhance), 0, passInfo.level);
                passInfo.atk += GameDataValue.GetEleDamage(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.FireAttackAdd), 1, RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.FireEnhance), 0);
                passInfo.atk += GameDataValue.GetEleDamage(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ColdAttackAdd), 1, RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ColdEnhance), 0); ;
                passInfo.atk += GameDataValue.GetEleDamage(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.LightingAttackAdd), 1, RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.LightingEnhance), 0); ;
                passInfo.atk += GameDataValue.GetEleDamage(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.WindAttackAdd), 1, RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.WindEnhance), 0); ;
                passInfo.damage1 = (int)(GameDataValue.GetSkillDamageRate(TestFight.GetTestSkill(1)) * passInfo.atk);
                passInfo.damage2 = (int)(GameDataValue.GetSkillDamageRate(TestFight.GetTestSkill(2)) * passInfo.atk);
                passInfo.damage3 = (int)(GameDataValue.GetSkillDamageRate(TestFight.GetTestSkill(3)) * passInfo.atk);
                passInfo.def = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Defense);
                passInfo.hp = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax);
                passInfo.monValue = GameDataValue.GetMonsterHP(TableReader.MonsterBase.GetRecord("21"), passInfo.level, MOTION_TYPE.Normal);
                passInfo.equipMat = BackBagPack.Instance.GetItemCnt(EquipRefresh._RefreshMatDataID);
                int gemCnt = 0;
                for (int i = 0; i < GemData._GemMaterialDataIDs.Count; ++i)
                {
                    gemCnt += BackBagPack.Instance.GetItemCnt(GemData._GemMaterialDataIDs[i]);
                }
                passInfo.equipGem = gemCnt;


                _PassInfoList.Add(passInfo);
            }
        }

        string fileName = "StagePassInfos";
        string path = Application.dataPath + fileName + ".txt";
        var fileStream = File.Create(path);
        var streamWriter = new StreamWriter(fileStream);
        foreach (var passInfo in _PassInfoList)
        {
            //attr
            //streamWriter.WriteLine(passInfo.level + "\t" + passInfo.atk + "\t" + passInfo.monValue + "\t" + passInfo.exp + "\t" + passInfo.monValue + "\t" + passInfo.hp);
            //drop
            streamWriter.WriteLine(passInfo.level + "\t" + passInfo.atk + "\t" + passInfo.monValue + "\t" + ((float)passInfo.monValue / passInfo.damage1) + "\t" + ((float)passInfo.monValue / passInfo.damage2) + "\t" + ((float)passInfo.monValue / passInfo.damage3));
        }
        streamWriter.Close();
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
        var areaPass = sceneGO.GetComponent<FightSceneLogicPassArea>();
        //var areas = sceneGO.GetComponentsInChildren<FightSceneAreaKAllEnemy>(true);
        //var bossAreas = sceneGO.GetComponentInChildren<FightSceneAreaKBossWithFish>(true);
        List<string> monsterIds = new List<string>();
        int eliteCnt = 0;
        foreach (var enemyArea in areaPass._FightArea)
        {
            if (enemyArea is FightSceneAreaKAllEnemy)
            {
                var kenemyArea = enemyArea as FightSceneAreaKAllEnemy;
                for (int i = 0; i < kenemyArea._EnemyBornPos.Length - 1; ++i)
                {
                    monsterIds.Add(kenemyArea._EnemyBornPos[i]._EnemyDataID);
                }

                var monLastId = kenemyArea._EnemyBornPos[kenemyArea._EnemyBornPos.Length - 1]._EnemyDataID;
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
            else if (enemyArea is FightSceneAreaKBossWithFish)
            {
                var bossArea = enemyArea as FightSceneAreaKBossWithFish;
                monsterIds.Add(bossArea._BossMotionID);
            }
        }
        

        Dictionary<string, int> items = new Dictionary<string, int>();
        foreach (var monId in monsterIds)
        {
            var monRecord = TableReader.MonsterBase.GetRecord(monId);
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, level);
            foreach (var dropItem in monsterDrops)
            {
                gold += dropItem._DropGold;
                MonsterDrop.PickItem(dropItem);
            }
            int dropExp = GameDataValue.GetMonsterExp(monRecord.MotionType, level, level);
            exp += dropExp;
            RoleData.SelectRole.AddExp(dropExp);
        }
        
        TestFight.DelAllEquip();
        TestFight.DelLevel();
        TestFight.DelSkill(1);
        TestFight.DelRefresh();
        TestFight.DelGem();
        //foreach (var dropItem in items)
        //{
        //    Debug.Log("Drop Item :" + dropItem.Key + "," + dropItem.Value);
        //}
        //Debug.Log("Drop Exp:" + exp + ", Gold:" + gold);
    }

    #endregion
}

