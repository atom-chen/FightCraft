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
                for (int i = 0; i < 10; ++i)
                {
                    _BackPackItems[i].ItemDataID = "1";
                }
            }
        }

        #region 

        public ItemEquip GetEquipItem(EQUIP_SLOT equipSlot)
        {
            return _EquipList[(int)equipSlot];
        }

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

        #endregion
    }
}
