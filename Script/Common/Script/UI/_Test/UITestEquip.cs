
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

    public static void ActBuffInFight()
    {
        ActBuffInFightInner();
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

    #region equip/item test

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        StageEnemyCnts();
        InitBuffAttr();
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
        var equipItem = ItemEquip.CreateEquip(level, quality, legencyID);
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

    #region fight test

    public InputField _TargetLevel;

    public void AutoLevel()
    {
        int targetLevel = int.Parse(_TargetLevel.text);
        int fightTimes = 0;
        string fileName = "StagePassInfos";

#if UNITY_EDITOR
        string path = Application.dataPath + fileName + ".txt";
        var fileStream = File.Create(path);
        var streamWriter = new StreamWriter(fileStream);
#endif
        int preGold = 0;
        int lastLevel = -1;
        int monCnt = 0;
        int equipDropCnt = 0;
        TestFight._DropOrangeEquipCnt = 0;
        while (true)
        {
            var level = RoleData.SelectRole._RoleLevel + RoleData.SelectRole._AttrLevel;
            if (level >= targetLevel)
                break;

            var nextDiff = level / 20;
            var nextStage = level % 40;
            nextStage = Mathf.Clamp(nextStage, 1, 40);

            int exp = 0;
            int gold = 0;

            var passStage = TestPassNormalStage(level, nextDiff, ref exp, ref gold);
            ActData.Instance._ProcessStageDiff = nextDiff;
            ActData.Instance._ProcessStageIdx = nextStage;
            ActData.Instance.PassStage( STAGE_TYPE.NORMAL);

            var plantGO = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plantGO.AddComponent<MeshCollider>();

            ++fightTimes;

            if (level != lastLevel)
            {
                lastLevel = level;
                RoleAttrManager roleAttr = new RoleAttrManager();
                roleAttr.InitMainRoleAttr();

                RoleAttrManager monAttr = new RoleAttrManager();
                monAttr.InitEnemyAttr(TableReader.MonsterBase.GetRecord("21"), level);

                Hashtable resultHash = new Hashtable();
                float damageRage = GameDataValue.GetSkillDamageRate(2);
                resultHash.Add("SkillDamageRate", damageRage);
                resultHash.Add("DamagePos", Vector3.zero);
                resultHash.Add("ImpactBase", new ImpactDamage() { _DamageType = ElementType.Physic });
                resultHash.Add("DamageType", ElementType.Physic);

                //damage
                RoleAttrManager.DamageClass damageClass = new RoleAttrManager.DamageClass();
                monAttr.CalculateNormalDamage(roleAttr, resultHash, damageClass);
                //final
                monAttr.CaculateFinalDamage(roleAttr, resultHash, damageClass);

                Hashtable resultHash2 = new Hashtable();
                float damageRage2 = GameDataValue.GetSkillDamageRate(2, true);
                resultHash2.Add("SkillDamageRate", damageRage2);
                resultHash2.Add("DamagePos", Vector3.zero);
                resultHash2.Add("ImpactBase", new ImpactDamage() { _DamageType = ElementType.Physic });
                resultHash2.Add("DamageType", ElementType.Physic);

                //damage
                RoleAttrManager.DamageClass damageClass2 = new RoleAttrManager.DamageClass();
                monAttr.CalculateNormalDamage(roleAttr, resultHash2, damageClass2);
                //final
                monAttr.CaculateFinalDamage(roleAttr, resultHash2, damageClass2);

#if UNITY_EDITOR
                //attr
                //streamWriter.WriteLine(fightTimes + "\t" + level + "\t" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack)
                //    + "\t" + monAttr.HP + "\t" + damageClass.TotalDamageValue
                //    + "\t" + damageRage + "\t" + ((float)monAttr.HP / damageClass.TotalDamageValue)
                //    + "\t" + damageRage2 + "\t" + ((float)monAttr.HP / damageClass2.TotalDamageValue));

                streamWriter.WriteLine(fightTimes + "\t" + level + "\t" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax)
                    + "\t" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Defense)
                    + "\t" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack));

                Debug.Log("Drop orange equip cng:" + TestFight._DropOrangeEquipCnt);
                //streamWriter.WriteLine(fightTimes + "\t" + level + "\t" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax)
                //    + "\t" + monAttr.GetBaseAttr(RoleAttrEnum.Attack));
#endif
            }

            //drop
            int matCnt = BackBagPack.Instance.GetItemCnt(EquipRefresh._RefreshMatDataID);
            int gemCnt = BackBagPack.Instance.GetItemCnt(GemData._GemMaterialDataIDs[0])
                + BackBagPack.Instance.GetItemCnt(GemData._GemMaterialDataIDs[1])
                + BackBagPack.Instance.GetItemCnt(GemData._GemMaterialDataIDs[2])
                + BackBagPack.Instance.GetItemCnt(GemData._GemMaterialDataIDs[3]);
            monCnt += passStage._MonsterCnt;
            equipDropCnt += passStage._DropEquipCnt;
#if UNITY_EDITOR
            //streamWriter.WriteLine(fightTimes + "\t" + level + "\t" + RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax)
            //    + "\t" + monAttr.GetBaseAttr(RoleAttrEnum.Attack) + "\t" + ((float)RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax) / monAttr.GetBaseAttr(RoleAttrEnum.Attack)));
            //streamWriter.WriteLine(fightTimes + "\t" + level + "\t" + PlayerDataPack.Instance.Gold + "\t" + matCnt + "\t" + gemCnt + "\t" + passStage._Gold + "\t" + monCnt + "\t" + equipDropCnt);
#endif

        }

#if UNITY_EDITOR
        streamWriter.Close();
#endif
        Debug.Log("FightTimes:" + fightTimes);
    }

    private void GetLevelStage(int level, ref int diff, ref int stageIdx)
    {
        diff = level / 20 + 1;
        diff = Math.Max(diff, 1);

        stageIdx = level % 20 + 1;
        stageIdx = Math.Max(stageIdx, 1);
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
        public int goldDrop;
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
        public int equipPoint;
        public int gemCost1;
        public int gemCost2;
    }

    class PassNormalInfo
    {
        public int _Gold;
        public int _Exp;
        public int _MonsterCnt;
        public int _DropEquipCnt;
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
        int lastGem = 0;
        for (int diff = 1; diff < 7; ++diff)
        {
            for (int stageid = 1; stageid < 21; ++stageid)
            {
                int exp = 0;
                int gold = 0;
                int equipPointTotal = 0;
                var passStage = TestPassNormalStage(stageid, diff, ref exp, ref gold);
                //foreach (var itemEquip in BackBagPack.Instance.PageEquips)
                //{
                //    int equipPoint = GameDataValue.GetEquipSellGold(itemEquip);
                //    equipPointTotal += equipPoint;
                //}
                //TestFight.DelAllEquip();

                var level = GameDataValue.GetStageLevel(diff, stageid, STAGE_TYPE.NORMAL);

                RoleAttrManager roleAttr = new RoleAttrManager();
                roleAttr.InitMainRoleAttr();

                RoleAttrManager monAttr = new RoleAttrManager();
                monAttr.InitEnemyAttr(TableReader.MonsterBase.GetRecord("21"), level);

                Hashtable resultHash = new Hashtable();
                resultHash.Add("SkillDamageRate", GameDataValue.GetSkillDamageRate(TestFight.GetTestSkill(1)));
                resultHash.Add("DamagePos", Vector3.zero);
                resultHash.Add("ImpactBase", new ImpactDamage() { _DamageType = ElementType.Physic});
                resultHash.Add("DamageType", ElementType.Physic);

                //damage
                RoleAttrManager.DamageClass damageClass = new RoleAttrManager.DamageClass();
                monAttr.CalculateNormalDamage(roleAttr, resultHash, damageClass);
                //final
                monAttr.CaculateFinalDamage(roleAttr, resultHash, damageClass);

                //event
                


                PassStageInfo passInfo = new PassStageInfo();
                passInfo.diff = diff;
                passInfo.stateIdx = stageid;
                passInfo.level = GameDataValue.GetStageLevel(diff, stageid, STAGE_TYPE.NORMAL);
                passInfo.exp = exp;
                passInfo.gold = PlayerDataPack.Instance.Gold;
                passInfo.goldDrop = gold;
                passInfo.levelExp = GameDataValue.GetLvUpExp(passInfo.level, 0);
                passInfo.atk = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack);
                passInfo.damage1 = damageClass.TotalDamageValue;
                passInfo.damage2 = 1;
                passInfo.damage3 = 1;
                passInfo.def = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Defense);
                passInfo.hp = monAttr.HP;
                passInfo.monValue = monAttr.HP;
                passInfo.equipMat = BackBagPack.Instance.GetItemCnt(EquipRefresh._RefreshMatDataID);
                int gemCnt = 0;
                for (int i = 0; i < GemData._GemMaterialDataIDs.Count; ++i)
                {
                    gemCnt += BackBagPack.Instance.GetItemCnt(GemData._GemMaterialDataIDs[i]);
                }
                
                passInfo.equipGem = gemCnt - lastGem;
                lastGem = gemCnt;
                passInfo.equipPoint = equipPointTotal;

                passInfo.gemCost1 = GameDataValue.GetGemConsume(passInfo.level);
                passInfo.gemCost2 = GameDataValue.GetGemGoldConsume(passInfo.level);


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
            streamWriter.WriteLine(passInfo.level + "\t" + passInfo.atk + "\t" + passInfo.monValue + "\t" + passInfo.damage1 + "\t" + ((float)passInfo.monValue / passInfo.damage1));
            //drop
            //streamWriter.WriteLine(passInfo.level + "\t" + passInfo.gold + "\t" + passInfo.goldDrop + "\t" + passInfo.equipPoint + "\t" + passInfo.equipMat + "\t" + passInfo.equipGem + "\t" + passInfo.gemCost1 + "\t" + passInfo.gemCost2);
        }
        streamWriter.Close();
    }

    public void OnTestDelEquips()
    {
        TestFight.DelAllEquip();
    }

    private PassNormalInfo TestPassNormalStage(int level, int diff, ref int exp, ref int gold)
    {
        PassNormalInfo passInfo = new PassNormalInfo();

        
        //var areas = sceneGO.GetComponentsInChildren<FightSceneAreaKAllEnemy>(true);
        //var bossAreas = sceneGO.GetComponentInChildren<FightSceneAreaKBossWithFish>(true);
        List<string> monsterIds = new List<string>();
        int eliteCnt = 0;
        if (level <= 20)
        {
            GameObject sceneLogicGO = null;
            if (level < 10)
            {
                sceneLogicGO = ResourceManager.Instance.GetGameObject("FightSceneLogic/FightLogic_Stage_0" + level);
            }
            else
            {
                sceneLogicGO = ResourceManager.Instance.GetGameObject("FightSceneLogic/FightLogic_Stage_" + level);
            }
            var areaPass = sceneLogicGO.GetComponent<FightSceneLogicPassArea>();
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
        }
        else
        {
            for (int i = 0; i < 120; ++i)
            {
                monsterIds.Add("21");
            }
            for (int i = 0; i < 18; ++i)
            {
                monsterIds.Add("22");
            }
            monsterIds.Add("20");
        }

        
        Dictionary<string, int> items = new Dictionary<string, int>();
        foreach (var monId in monsterIds)
        {
            var monRecord = TableReader.MonsterBase.GetRecord(monId);
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, level);
            foreach (var dropItem in monsterDrops)
            {
                gold += dropItem._DropGold;
                passInfo._Gold += dropItem._DropGold;
                MonsterDrop.PickItem(dropItem);

                if (dropItem._ItemEquip != null)
                {
                    ++passInfo._DropEquipCnt;
                }
            }
            int dropExp = GameDataValue.GetMonsterExp(monRecord.MotionType, level, level);
            exp += dropExp;
            passInfo._Exp += dropExp;
            RoleData.SelectRole.AddExp(dropExp);

            Hashtable hash = new Hashtable();
            MotionManager objMotion = new MotionManager();
            objMotion.RoleAttrManager = new RoleAttrManager();
            objMotion.RoleAttrManager.InitEnemyAttr(monRecord, level);
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);

        }
        passInfo._MonsterCnt = monsterIds.Count;

        TestFight.DelAllEquip();
        TestFight.DelLevel();
        TestFight.DelSkill(2);
        //TestFight.DelRefresh();
        TestFight.DelGem();

        //foreach (var dropItem in items)
        //{
        //    Debug.Log("Drop Item :" + dropItem.Key + "," + dropItem.Value);
        //}
        //Debug.Log("Drop Exp:" + exp + ", Gold:" + gold);

        return passInfo;
    }

    public void StageEnemyCnts()
    {
        for (int i = 1; i < 21; ++i)
        {
            var stageRecord = TableReader.StageInfo.GetRecord(i.ToString());
            var sceneGO = ResourceManager.Instance.GetGameObject("FightSceneLogic/" + stageRecord.FightLogicPath);
            var fightLogic = sceneGO.GetComponent<FightSceneLogicPassArea>();
            int enemyCnt = 0;
            foreach (var area in fightLogic._FightArea)
            {
                if (area is FightSceneAreaKAllEnemy)
                {
                    enemyCnt += (area as FightSceneAreaKAllEnemy)._EnemyBornPos.Length;
                }
            }

            Debug.Log("Scene " + i + " area enemy cnt:" + enemyCnt);
        }
    }

    #endregion

    #region global buff test

    public List<string> _AttrIDs;
    public List<Toggle> _TestBuffToggles;

    public static List<EquipExAttr> _ExAttrs = new List<EquipExAttr>();
    public static List<bool> _IsToggleOn = new List<bool>();

    public void InitBuffAttr()
    {
        foreach (var attrID in _AttrIDs)
        {
            var attrRecord = Tables.TableReader.AttrValue.GetRecord(attrID);
            _ExAttrs.Add(attrRecord.GetExAttr(1));
        }

        OnToggleChange();
    }

    public void OnToggleChange()
    {
        _IsToggleOn.Clear();
        for (int i = 0; i < _TestBuffToggles.Count; ++i)
        {
            _IsToggleOn.Add(_TestBuffToggles[i].isOn);
        }
    }

    public static void ActBuffInFightInner()
    {
        for (int i = 0; i < _IsToggleOn.Count; ++i)
        {
            if (_IsToggleOn[i])
            {
                GlobalBuffData.Instance._ExAttrs.Add(_ExAttrs[i]);
            }
        }

        RoleData.SelectRole.CalculateAttr();
    }

    #endregion

    #region element test

    public InputField _FiveElementItemLevel;
    public InputField _FiveElementItemType;

    public void OnBtnElementItem()
    {
        int level = int.Parse(_FiveElementItemLevel.text);
        int type = int.Parse(_FiveElementItemType.text);
        var elementItem = FiveElementData.CreateElementItem(level, (FIVE_ELEMENT)type);
        FiveElementData.Instance.AddElementItem(elementItem);
    }

    #endregion
}

