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
        public List<ItemEquip> _BackPackItems;
        public const int MAX_BACK_PACK_ITEM_CNT = 25;

        //default info
        public string MainBaseName;
        public string ModelName;
        public PROFESSION Profession;
        public string DefaultWeaponModel;

        public static RoleData SelectRole
        {
            get
            {
                return PlayerDataPack.Instance._SelectedRole;
            }
        }

        public void InitRoleData()
        {
            if (_RoleLevel < 0)
            {
                _RoleLevel = 0;
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

            if (equipItem.EquipItemRecord.LevelLimit > _RoleLevel)
                return false;

            if (equipItem.EquipItemRecord.ProfessionLimit > 0 &&
                ((equipItem.EquipItemRecord.ProfessionLimit >> (int)Profession) & 1) == 0)
            {
                return false;
            }

            return true;
        }

        public void PutOnEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
        {
            if (!IsCanEquipItem(equipSlot, equipItem))
                return;

            _EquipList[(int)equipSlot].ExchangeInfo(equipItem);
            GameUI.UIBagPack.RefreshBagItems();

            CalculateAttr();
        }

        public void PutOffEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
        {
            var backPackPos = GetEmptyBackPack();
            backPackPos.ExchangeInfo(equipItem);

            GameUI.UIBagPack.RefreshBagItems();

            CalculateAttr();
        }

        private ItemEquip GetEmptyBackPack()
        {
            for (int i = 0; i < _BackPackItems.Count; ++i)
            {
                if (!_BackPackItems[i].IsVolid())
                {
                    return _BackPackItems[i];
                }
            }
            return null;
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

        public float GetBaseMoveSpeed()
        {
            return 4.5f;
        }

        public int GetBaseAttackSpeed()
        {
            return 1;
        }

        public int GetBaseAttack()
        {
            return _RoleLevel * 10 + 10;
        }

        public int GetBaseHP()
        {
            return _RoleLevel * 100 + 100;
        }

        public int GetBaseDefence()
        {
            return _RoleLevel * 5 + 5;
        }

        //baseAttrs
        public RoleAttrStruct _BaseAttr = new RoleAttrStruct();

        public void CalculateAttr()
        {
            _BaseAttr.ResetBaseAttr();
            SetRoleLevelAttr(_BaseAttr);
            SetEquipAttr(_BaseAttr);

            CalculateSecondAttr();
        }

        public void SetRoleLevelAttr(RoleAttrStruct _BaseAttr)
        {
            _BaseAttr.SetValue(RoleAttrEnum.Strength, Strength);
            _BaseAttr.SetValue(RoleAttrEnum.Dexterity, Dexterity);
            _BaseAttr.SetValue(RoleAttrEnum.Vitality, Vitality);
            _BaseAttr.SetValue(RoleAttrEnum.Attack, _RoleLevel * 1 + 10);
            _BaseAttr.SetValue(RoleAttrEnum.HPMax, _RoleLevel * 100 + 5000);
            _BaseAttr.SetValue(RoleAttrEnum.Defense, 10);
        }

        public void SetEquipAttr(RoleAttrStruct _BaseAttr)
        {
            foreach (var equipInfo in _EquipList)
            {
                equipInfo.SetEquipAttr(_BaseAttr);
            }
        }

        private void CalculateSecondAttr()
        {
            var strength = _BaseAttr.GetValue(RoleAttrEnum.Strength);
            var baseAttack = _BaseAttr.GetValue(RoleAttrEnum.Attack);
            float attackByStrength = (strength / 1000.0f) * baseAttack + strength * 2;
            _BaseAttr.AddValue(RoleAttrEnum.Attack, (int)attackByStrength);

            var dexteriry = _BaseAttr.GetValue(RoleAttrEnum.Dexterity);
            int criticalRate = (int)((dexteriry / 1000.0f) * 2500);
            int criticalDamage = (int)((dexteriry / 1000.0f) * 10000);
            int attackSpeed = (int)((dexteriry / 1000.0f) * 1000);
            int moveSpeed = (int)((dexteriry / 1000.0f) * 1000);
            int ignoreAttack = (int)(dexteriry * 0.5f);
            _BaseAttr.AddValue(RoleAttrEnum.CriticalHitChance, criticalRate);
            _BaseAttr.AddValue(RoleAttrEnum.CriticalHitDamge, criticalDamage);
            _BaseAttr.AddValue(RoleAttrEnum.AttackSpeed, attackSpeed);
            _BaseAttr.AddValue(RoleAttrEnum.MoveSpeed, moveSpeed);
            _BaseAttr.AddValue(RoleAttrEnum.IgnoreDefenceAttack, ignoreAttack);

            var vitality = _BaseAttr.GetValue(RoleAttrEnum.Vitality);
            int baseHP = _BaseAttr.GetValue(RoleAttrEnum.HPMax);
            int hpByVitality = (int)((vitality / 500.0f) * baseHP);
            int finalDamageReduse = (int)(vitality * 0.1f);
            _BaseAttr.AddValue(RoleAttrEnum.HPMax, hpByVitality);
            _BaseAttr.AddValue(RoleAttrEnum.FinalDamageReduse, finalDamageReduse);
        }

        #endregion

        #region attr Points

        public static int MAX_ROLE_LEVEL = 100;
        public static int POINT_PER_ROLE_LEVEL = 5;
        public static int POINT_PER_ATTR_LEVEL = 1;

        [SaveField(3)]
        public int _RoleLevel;

        [SaveField(9)]
        public int _AttrLevel;

        [SaveField(4)]
        public int _CurExp;

        [SaveField(5)]
        private int _AddStrength = 0;
        public int Strength
        {
            get
            {
                return _RoleLevel * 1 + _AddStrength;
            }
        }

        [SaveField(6)]
        private int _AddDexterity = 0;
        public int Dexterity
        {
            get
            {
                return _RoleLevel * 1 + _AddDexterity;
            }
        }

        [SaveField(7)]
        private int _AddVitality = 0;
        public int Vitality
        {
            get
            {
                return _RoleLevel * 1 + _AddVitality;
            }
        }

        [SaveField(8)]
        private int _UnDistrubutePoint = 0;
        public int UnDistrubutePoint
        {
            get
            {
                return _UnDistrubutePoint;
            }
        }

        public void AddExp(int value)
        {
            if (_RoleLevel < value)
            {
                _CurExp += value;
                var expRecord = TableReader.RoleExp.GetRecord((_RoleLevel + 1).ToString());
                if (_CurExp >= expRecord.ExpValue)
                {
                    _CurExp -= expRecord.ExpValue;
                    RoleLevelUp();
                }
            }
            else
            {
                _CurExp += value;
                var expRecord = TableReader.RoleExp.GetRecord((_AttrLevel + MAX_ROLE_LEVEL + 1).ToString());
                if (expRecord == null)
                    return;

                if (_CurExp >= expRecord.ExpValue)
                {
                    _CurExp -= expRecord.ExpValue;
                    AttrLevelUp();
                }
            }
            
        }

        private void RoleLevelUp()
        {
            ++_RoleLevel;
            _UnDistrubutePoint += 5;

            CalculateAttr();
        }

        private void AttrLevelUp()
        {
            ++_AttrLevel;
            _UnDistrubutePoint += 1;
        }

        public void ResetPoints()
        {
            _AddStrength = 0;
            _AddDexterity = 0;
            _AddVitality = 0;
            _UnDistrubutePoint = _RoleLevel * POINT_PER_ROLE_LEVEL + _AttrLevel * POINT_PER_ATTR_LEVEL;

            CalculateAttr();
        }

        public void DistributePoint(int distriAttr)
        {
            --_UnDistrubutePoint;
            switch (distriAttr)
            {
                case 1:
                    ++_AddStrength;
                    break;
                case 2:
                    ++_AddDexterity;
                    break;
                case 3:
                    ++_AddVitality;
                    break;
            }

            CalculateAttr();
        }

        #endregion
    }
}
