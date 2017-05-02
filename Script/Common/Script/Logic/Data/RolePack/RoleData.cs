using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

namespace GameLogic
{
    public class RoleData
    {
        [SaveField(1)]
        public List<ItemEquip> _EquipList;

        [SaveField(2)]
        public int _Level;

        [SaveField(3)]
        public List<ItemEquip> _BackPackItems;
        public const int MAX_BACK_PACK_ITEM_CNT = 25;

        //default info
        public string MainBaseName;
        public string ModelName;
        public PROFESSION Profession;
        public string DefaultWeaponModel;

        public void InitRoleData()
        {
            if (_Level < 0)
            {
                _Level = 0;
            }

            if (_EquipList == null || _EquipList.Count == 0)
            {
                int equipSlotCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
                _EquipList = new List<ItemEquip>();
                for (int i = 0; i< equipSlotCnt; ++i)
                {
                    _EquipList.Add(new ItemEquip() { ItemDataID = "-1"});
                }
            }

            if (_BackPackItems == null || _BackPackItems.Count == 0)
            {
                _BackPackItems = new List<ItemEquip>();
                for (int i = 0; i < MAX_BACK_PACK_ITEM_CNT; ++i)
                {
                    _BackPackItems.Add(new ItemEquip() { ItemDataID = "-1" });
                }
            }

        }

        #region equipManager

        public ItemEquip GetEquipItem(EQUIP_SLOT equipSlot)
        {
            return _EquipList[(int)equipSlot];
        }

        public bool IsCanEquipItem(EQUIP_SLOT equipSlot, ItemEquip equipItem)
        {
            if (equipItem == null)
                return false;

            if (equipItem.EquipItemRecord == null)
                return false;

            if (equipItem.EquipItemRecord.Slot != equipSlot)
                return false;

            if (equipItem.EquipItemRecord.LevelLimit > _Level)
                return false;

            if (equipItem.EquipItemRecord.ProfessionLimit > 0 &&
                ((equipItem.EquipItemRecord.ProfessionLimit >> (int)Profession) & 1) == 0)
            {
                return false;
            }

            return true;
        }

        public void PutOnEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
        { }

        public string GetWeaponModelName()
        {
            var equip = GetEquipItem(EQUIP_SLOT.WEAPON);
            if (equip != null && equip.EquipItemRecord != null)
            {
                return equip.EquipItemRecord.Model;
            }
            else
            {
                return DefaultWeaponModel;
            }
        }

        public List<string> GetRoleSkills()
        {
            List<string> skillMotions = new List<string>() { "Attack", "Buff1", "Buff2", "Defence", "Dush", "Skill1", "Skill2", "Skill3" };
            return skillMotions;
        }

        public ItemEquip AddNewEquip(ItemEquip itemEquip)
        {
            for (int i = 0; i < _BackPackItems.Count; ++i)
            {
                if (string.IsNullOrEmpty(_BackPackItems[i].ItemDataID) || _BackPackItems[i].ItemDataID == "-1")
                {
                    _BackPackItems[i].ExchangeInfo(itemEquip);
                    return _BackPackItems[i];
                }
            }
            return null;
        }

        #endregion

        #region role attr

        [SaveField(4)]
        public int _CurExp;

        public int GetBaseMoveSpeed()
        {
            return 1;
        }

        public int GetBaseAttackSpeed()
        {
            return 1;
        }

        public int GetBaseAttack()
        {
            return _Level * 10 + 10;
        }

        public int GetBaseHP()
        {
            return _Level * 100 + 100;
        }

        public int GetBaseDefence()
        {
            return _Level * 5 + 5;
        }

        //exAttr
        public Dictionary<FightAttr.FightAttrType, int> _ExAttrs = new Dictionary<FightAttr.FightAttrType, int>();

        public void InitExAttrs()
        {
            foreach (var equip in _EquipList)
            {
                if (!equip.IsVolid())
                    continue;

                foreach (var exAttr in equip._DynamicDataVector)
                {
                    if (_ExAttrs.ContainsKey(exAttr.AttrID))
                    {
                        _ExAttrs[exAttr.AttrID] += exAttr.AttrValue1;
                    }
                    else
                    {
                        _ExAttrs.Add(exAttr.AttrID, exAttr.AttrValue1);
                    }
                }
            }
        } 
        #endregion
    }
}
