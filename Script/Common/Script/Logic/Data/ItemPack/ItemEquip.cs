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
            
        public int EquipLevel
        {
            get
            {
                return _DynamicDataInt[0];
            }
            set
            {
                _DynamicDataInt[0] = value;
            }
        }

        public ITEM_QUALITY EquipQuality
        {
            get
            {
                return (ITEM_QUALITY)_DynamicDataInt[1];
            }
            set
            {
                _DynamicDataInt[1] = (int)value;
            }
        }

        public int EquipValue
        {
            get
            {
                return _DynamicDataInt[2];
            }
            set
            {
                _DynamicDataInt[2] = value;
            }
        }

        #endregion

        #region equipAttr

        public Vector3 GetExAttr(int idx)
        {
            if (_DynamicDataVector.Count > idx)
                return _DynamicDataVector[idx];
            return Vector3.zero;
        }

        public void AddExAttr(Vector3 attr)
        {
            _DynamicDataVector.Add(attr);
        }

        #endregion

        #region equipBase




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
                itemEquip.AddExAttr(GetRandomAttr(canGetAttrs[i], itemEquip.EquipValue));
                RemoveRandomAttr(canGetAttrs, canGetAttrs[i]);
            }
        }

        private static Vector3 GetRandomAttr(FightAttrRecord attrRecord, int value)
        {
            Vector3 attrItem = new Vector3();
            attrItem.x = int.Parse(attrRecord.Id);

            Vector3 attrValue = new Vector3();
            for (int i = attrRecord.Values.Count; i >= 0; --i)
            {
                if (attrRecord.Values[i].z > 0 && value > attrRecord.Values[i].z)
                {
                    attrValue = attrRecord.Values[i];
                    break;
                }
            }

            attrItem.y = UnityEngine.Random.Range(attrValue.x, attrValue.y);
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
                singleRate = 1 / (professionEquips.Count);
            }

            float randomRate = UnityEngine.Random.Range(0, 1);
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
