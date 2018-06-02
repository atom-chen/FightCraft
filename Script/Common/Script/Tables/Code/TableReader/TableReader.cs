﻿using System.Collections;

namespace Tables
{
    public class TableReader
    {

        #region 唯一实例

        private TableReader() { }

        private TableReader _Instance = null;
        public TableReader Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TableReader();

                return _Instance;
            }
        }

        #endregion
        #region Logic

//
        public static Achievement Achievement { get; internal set; }
//
        public static AttrValue AttrValue { get; internal set; }
//
        public static BossStage BossStage { get; internal set; }
//
        public static CommonItem CommonItem { get; internal set; }
//
        public static EquipItem EquipItem { get; internal set; }
//
        public static EquipSpAttr EquipSpAttr { get; internal set; }
//
        public static FightAttr FightAttr { get; internal set; }
//
        public static GemSet GemSet { get; internal set; }
//
        public static GemTable GemTable { get; internal set; }
//
        public static Mission Mission { get; internal set; }
//
        public static MonsterAttr MonsterAttr { get; internal set; }
//
        public static MonsterBase MonsterBase { get; internal set; }
//
        public static RoleExp RoleExp { get; internal set; }
//
        public static ShopItem ShopItem { get; internal set; }
//
        public static SkillInfo SkillInfo { get; internal set; }
//
        public static StageInfo StageInfo { get; internal set; }
//
        public static StrDictionary StrDictionary { get; internal set; }
//
        public static StrTable StrTable { get; internal set; }

        public static void ReadTables()
        {
            //读取所有表
            Achievement = new Achievement(TableReadBase.GetTableText("Achievement"), false);
            AttrValue = new AttrValue(TableReadBase.GetTableText("AttrValue"), false);
            BossStage = new BossStage(TableReadBase.GetTableText("BossStage"), false);
            CommonItem = new CommonItem(TableReadBase.GetTableText("CommonItem"), false);
            EquipItem = new EquipItem(TableReadBase.GetTableText("EquipItem"), false);
            EquipSpAttr = new EquipSpAttr(TableReadBase.GetTableText("EquipSpAttr"), false);
            FightAttr = new FightAttr(TableReadBase.GetTableText("FightAttr"), false);
            GemSet = new GemSet(TableReadBase.GetTableText("GemSet"), false);
            GemTable = new GemTable(TableReadBase.GetTableText("GemTable"), false);
            Mission = new Mission(TableReadBase.GetTableText("Mission"), false);
            MonsterAttr = new MonsterAttr(TableReadBase.GetTableText("MonsterAttr"), false);
            MonsterBase = new MonsterBase(TableReadBase.GetTableText("MonsterBase"), false);
            RoleExp = new RoleExp(TableReadBase.GetTableText("RoleExp"), false);
            ShopItem = new ShopItem(TableReadBase.GetTableText("ShopItem"), false);
            SkillInfo = new SkillInfo(TableReadBase.GetTableText("SkillInfo"), false);
            StageInfo = new StageInfo(TableReadBase.GetTableText("StageInfo"), false);
            StrDictionary = new StrDictionary(TableReadBase.GetTableText("StrDictionary"), false);
            StrTable = new StrTable(TableReadBase.GetTableText("StrTable"), false);

            //初始化所有表
            Achievement.CoverTableContent();
            AttrValue.CoverTableContent();
            BossStage.CoverTableContent();
            CommonItem.CoverTableContent();
            EquipItem.CoverTableContent();
            EquipSpAttr.CoverTableContent();
            FightAttr.CoverTableContent();
            GemSet.CoverTableContent();
            GemTable.CoverTableContent();
            Mission.CoverTableContent();
            MonsterAttr.CoverTableContent();
            MonsterBase.CoverTableContent();
            RoleExp.CoverTableContent();
            ShopItem.CoverTableContent();
            SkillInfo.CoverTableContent();
            StageInfo.CoverTableContent();
            StrDictionary.CoverTableContent();
            StrTable.CoverTableContent();
        }

        #endregion
    }
}
