using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

namespace GameLogic
{
    public class ItemEquip : ItemBase
    {
        #region equipData

        public List<int> DynamicDataInt
        {
            get
            {
                if (_DynamicDataInt == null || _DynamicDataInt.Count == 0)
                {
                    _DynamicDataInt = new List<int>() { 0, 0, 0 };
                }
                return _DynamicDataInt;
            }
        }

        public int EquipLevel
        {
            get
            {
                return DynamicDataInt[0];
            }
            set
            {
                DynamicDataInt[0] = value;
            }
        }

        public ITEM_QUALITY EquipQuality
        {
            get
            {
                return (ITEM_QUALITY)DynamicDataInt[1];
            }
            set
            {
                DynamicDataInt[1] = (int)value;
            }
        }

        public int EquipValue
        {
            get
            {
                return DynamicDataInt[2];
            }
            set
            {
                DynamicDataInt[2] = value;
            }
        }

        public string GetEquipNameWithColor()
        {
            return CommonDefine.GetQualityColorStr(EquipQuality) + EquipItemRecord.CommonItem.Name + "</color>";
        }

        public string GetBaseAttrStr()
        {
            string attrStr = "";
            if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
            {
                string attackValue = BaseAttack.ToString();
                if (BaseAttack > EquipItemRecord.BaseAttrs[0])
                {
                    attackValue = CommonDefine.GetQualityColorStr(EquipQuality) + attackValue + "</color>";
                }
                attrStr = StrDictionary.GetFormatStr(1, attackValue);
            }
            else if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO || EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
            {
                string hpValue = BaseAttack.ToString();
                if (BaseAttack > EquipItemRecord.BaseAttrs[0])
                {
                    hpValue = CommonDefine.GetQualityColorStr(EquipQuality) + hpValue + "</color>";
                }
                attrStr = StrDictionary.GetFormatStr(2, hpValue);

                string defenceValue = BaseAttack.ToString();
                if (BaseAttack > EquipItemRecord.BaseAttrs[0])
                {
                    defenceValue = CommonDefine.GetQualityColorStr(EquipQuality) + defenceValue + "</color>";
                }
                attrStr += "\n" + StrDictionary.GetFormatStr(3, defenceValue);
            }
            return attrStr;

        }
        #endregion

        #region equipAttr

        public EquipExAttr GetExAttr(int idx)
        {
            if (_DynamicDataVector.Count > idx)
                return _DynamicDataVector[idx];
            return null;
        }

        public void AddExAttr(EquipExAttr attr)
        {
            _DynamicDataVector.Add(attr);
        }

        #endregion

        #region equipBase

        private int _BaseAttack = -1;
        public int BaseAttack
        {
            get
            {
                if (_BaseAttack < 0)
                {
                    int exValue = 0;
                    _BaseAttack = EquipItemRecord.BaseAttrs[0];
                    foreach (var exAttr in _DynamicDataVector)
                    {
                        if (exAttr.AttrID == 3)
                        {
                            exValue += exAttr.AttrValue1;
                        }
                        else if (exAttr.AttrID == 4)
                        {
                            exValue += (int)(_BaseAttack * (exAttr.AttrValue1 / 100.0f));
                        }
                    }
                    _BaseAttack += exValue;
                }
                return _BaseAttack;
            }
        }

        private int _BaseHP = -1;
        public int BaseHP
        {
            get
            {
                if (_BaseHP < 0)
                {
                    int exValue = 0;
                    _BaseHP = EquipItemRecord.BaseAttrs[1];
                    foreach (var exAttr in _DynamicDataVector)
                    {
                        if (exAttr.AttrID == 1)
                        {
                            exValue += exAttr.AttrValue1;
                        }
                        else if (exAttr.AttrID == 2)
                        {
                            exValue += (int)(_BaseHP * (exAttr.AttrValue1 / 100.0f));
                        }
                    }
                    _BaseHP += exValue;
                }
                return _BaseHP;
            }
        }

        private int _BaseDefence = -1;
        public int BaseDefence
        {
            get
            {
                if (_BaseDefence < 0)
                {
                    int exValue = 0;
                    _BaseDefence = EquipItemRecord.BaseAttrs[0];
                    foreach (var exAttr in _DynamicDataVector)
                    {
                        if (exAttr.AttrID == 5)
                        {
                            exValue += exAttr.AttrValue1;
                        }
                        else if (exAttr.AttrID == 6)
                        {
                            exValue += (int)(_BaseDefence * (exAttr.AttrValue1 / 100.0f));
                        }
                    }
                    _BaseDefence += exValue;
                }
                return _BaseDefence;
            }
        }

        private int _RequireLevel = -1;
        public int RequireLevel
        {
            get
            {
                if (_RequireLevel < 0)
                {
                    int exValue = 0;
                    _RequireLevel = EquipItemRecord.LevelLimit;
                    foreach (var exAttr in _DynamicDataVector)
                    {
                        if (exAttr.AttrID == 16)
                        {
                            exValue += exAttr.AttrValue1;
                        }
                    }
                    _RequireLevel -= exValue;
                }
                return _RequireLevel;
            }
        }
        #endregion

        #region equip reset

        private CommonItemRecord _ResetCostItem = null;
        public CommonItemRecord ResetCostItem
        {
            get
            {
                if (_ResetCostItem == null)
                {
                    InitEquipResetCostItemData();
                }
                return _ResetCostItem;
            }
        }

        private int _ResetCostItemNum = -1;
        public int ResetCostItemNum
        {
            get
            {
                if (_ResetCostItemNum < 0)
                {
                    InitEquipResetCostItemData();
                }
                return _ResetCostItemNum;
            }
        }

        private int _ResetCostMoney = -1;
        public int ResetCostMoney
        {
            get
            {
                if (_ResetCostMoney < 0)
                {
                    InitEquipResetCostItemData();
                }
                return _ResetCostMoney;
            }
        }

        private void InitEquipResetCostItemData()
        {
            if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
            {
                if (EquipLevel < 40)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20000");
                    _ResetCostItemNum = 4;
                    _ResetCostMoney = 400;
                }
                else if (EquipLevel < 70)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20001");
                    _ResetCostItemNum = 7;
                    _ResetCostMoney = 700;
                }
                else if (EquipLevel < 90)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20002");
                    _ResetCostItemNum = 9;
                    _ResetCostMoney = 900;
                }
                else if (EquipLevel < 100)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20003");
                    _ResetCostItemNum = 10;
                    _ResetCostMoney = 1000;
                }
                else
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20004");
                    _ResetCostItemNum = 10;
                    _ResetCostMoney = 1000;
                }
            }
            else if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO
                || EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
            {
                if (EquipLevel < 40)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20005");
                    _ResetCostItemNum = 4;
                    _ResetCostMoney = 400;
                }
                else if (EquipLevel < 70)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20006");
                    _ResetCostItemNum = 7;
                    _ResetCostMoney = 700;
                }
                else if (EquipLevel < 90)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20007");
                    _ResetCostItemNum = 9;
                    _ResetCostMoney = 900;
                }
                else if (EquipLevel < 100)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20008");
                    _ResetCostItemNum = 10;
                    _ResetCostMoney = 1000;
                }
                else
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20009");
                    _ResetCostItemNum = 10;
                    _ResetCostMoney = 1000;
                }
            }
            else if (EquipItemRecord.Slot == EQUIP_SLOT.AMULET
                || EquipItemRecord.Slot == EQUIP_SLOT.RING)
            {
                if (EquipLevel < 40)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20010");
                    _ResetCostItemNum = 4;
                    _ResetCostMoney = 400;
                }
                else if (EquipLevel < 70)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20011");
                    _ResetCostItemNum = 7;
                    _ResetCostMoney = 700;
                }
                else if (EquipLevel < 90)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20012");
                    _ResetCostItemNum = 9;
                    _ResetCostMoney = 900;
                }
                else if (EquipLevel < 100)
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20013");
                    _ResetCostItemNum = 10;
                    _ResetCostMoney = 1000;
                }
                else
                {
                    _ResetCostItem = TableReader.CommonItem.GetRecord("20014");
                    _ResetCostItemNum = 10;
                    _ResetCostMoney = 1000;
                }
            }
        }

        public void ResetEquipAttr()
        {
            _DynamicDataVector.Clear();
            RandomEquipAttr(this);
        }

        #endregion

        #region create equip

        public static ItemEquip CreateEquipByMonster(int level, Tables.ITEM_QUALITY quality, int value)
        {
            return null;
        }

        public static ItemEquip CreateEquip(int level, Tables.ITEM_QUALITY quality, int value)
        {
            var baseEquip = GetRandomItem(level);
            if (baseEquip == null)
                return null;

            ItemEquip itemEquip = new ItemEquip();
            itemEquip.ItemDataID = baseEquip.Id;
            itemEquip.EquipLevel = level;
            itemEquip.EquipQuality = quality;
            itemEquip.EquipValue = value;

            RandomEquipAttr(itemEquip);

            return itemEquip;
        }

        public static void RandomEquipAttr(ItemEquip itemEquip)
        {
            int attrCnt = GetRandomAttrCnt(itemEquip.EquipQuality);

            List<FightAttrRecord> canGetAttrs = new List<FightAttrRecord>();
            foreach (var record in TableReader.FightAttr.Records.Values)
            {
                if (record.LevelMin > 0 && record.LevelMin > itemEquip.EquipLevel)
                    continue;

                if (record.LevelMax > 0 && record.LevelMax < itemEquip.EquipLevel)
                    continue;

                if (record.SlotLimit >= 0)
                {
                    if (((record.SlotLimit >> (int)itemEquip.EquipItemRecord.Slot) & 1) == 0)
                    {
                        continue;
                    }
                }

                if (record.ProfessionLimit >= 0)
                {
                    if (record.ProfessionLimit != (int)itemEquip.EquipItemRecord.ProfessionLimit)
                        continue;
                }

                canGetAttrs.Add(record);
            }

            for (int i = 0; i < attrCnt; ++i)
            {
                int randomIdx = UnityEngine.Random.Range(0, canGetAttrs.Count);
                itemEquip.AddExAttr(GetRandomAttr(canGetAttrs[randomIdx], itemEquip.EquipValue));
                RemoveRandomAttr(canGetAttrs, canGetAttrs[randomIdx]);
            }
        }

        private static EquipExAttr GetRandomAttr(FightAttrRecord attrRecord, int value)
        {
            EquipExAttr attrItem = new EquipExAttr();
            attrItem.AttrID = int.Parse(attrRecord.Id);

            Vector3 attrValue = new Vector3();
            for (int i = attrRecord.Values.Count - 1; i >= 0; --i)
            {
                if (attrRecord.Values[i].z > 0 && value > attrRecord.Values[i].z)
                {
                    attrValue = attrRecord.Values[i];
                    break;
                }
            }

            attrItem.AttrValue1 = UnityEngine.Random.Range((int)attrValue.x, (int)attrValue.y + 1);
            return attrItem;
        }

        private static List<FightAttrRecord> RemoveRandomAttr(List<FightAttrRecord> attrList, FightAttrRecord attrRecord)
        {
            attrList.Remove(attrRecord);
            if (attrRecord.Conflict <= 0)
                return attrList;

            List<FightAttrRecord> conflicts = new List<FightAttrRecord>();
            for (int i = 0; i < attrList.Count; ++i)
            {
                if (attrList[i].Conflict == attrRecord.Conflict)
                {
                    conflicts.Add(attrList[i]);
                }
            }

            foreach (var conflictRecord in conflicts)
            {
                attrList.Remove(conflictRecord);
            }

            return attrList;
        }

        private static EquipItemRecord GetRandomItem(int level)
        {
            Dictionary<int, EquipItemRecord> professionEquips = new Dictionary<int, EquipItemRecord>();
            foreach (var equipRecord in TableReader.EquipItem.ClassedEquips[GetRandomItemSlot(level)])
            {
                if (!professionEquips.ContainsKey(equipRecord.ProfessionLimit))
                {
                    professionEquips.Add(equipRecord.ProfessionLimit, equipRecord);
                    continue;
                }

                if (equipRecord.LevelLimit < level
                    && equipRecord.LevelLimit > professionEquips[equipRecord.ProfessionLimit].LevelLimit)
                {
                    professionEquips[equipRecord.ProfessionLimit] = equipRecord;
                }
            }

            float singleRate = 0;
            if (professionEquips.ContainsKey((int)PROFESSION.NONE))
            {
                singleRate = (1 - 0.5f) / (professionEquips.Count - 1);
            }
            else
            {
                singleRate = 1.0f / (professionEquips.Count);
            }

            float randomRate = UnityEngine.Random.Range(0, 1.0f);
            float rateTotal = 0;
            foreach (var equipRecord in professionEquips.Values)
            {
                if (equipRecord.ProfessionLimit == (int)PROFESSION.NONE)
                {
                    rateTotal += 0.5f;
                }
                else
                {
                    rateTotal += singleRate;
                }

                if (rateTotal >= randomRate)
                    return equipRecord;
            }
            return null;
        }

        public static EQUIP_SLOT GetRandomItemSlot(int level)
        {
            int slotTypeCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
            int randomSlot = UnityEngine.Random.Range(0, slotTypeCnt);
            return (EQUIP_SLOT)randomSlot;
        }

        private static int GetRandomAttrCnt(Tables.ITEM_QUALITY quality)
        {
            switch (quality)
            {
                case Tables.ITEM_QUALITY.WHITE:
                    return 0;
                case Tables.ITEM_QUALITY.BLUE:
                    return UnityEngine.Random.Range(1, 2);
                case Tables.ITEM_QUALITY.PURPER:
                    return UnityEngine.Random.Range(3, 4);
                case Tables.ITEM_QUALITY.ORIGIN:
                    return 5;
                default:
                    return 0;
            }
        }

        #endregion
    }
}
