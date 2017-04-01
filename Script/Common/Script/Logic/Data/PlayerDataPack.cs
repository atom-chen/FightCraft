using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

namespace GameLogic
{
    public class PlayerDataPack : DataPackBase
    {
        #region 单例

        private static PlayerDataPack _Instance;
        public static PlayerDataPack Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new PlayerDataPack();
                }
                return _Instance;
            }
        }

        private PlayerDataPack()
        {
            GuiTextDebug.debug("BackBagPack init");
        }

        #endregion

        [SaveField(1)]
        private int _Goin = 0;

        [SaveField(2)]
        private int _Diamond = 0;

        [SaveField(3)]
        private List<RoleData> _RoleList;

        public RoleData _SelectedRole;

        #region 

        public void InitPlayerData()
        {
            if (_RoleList == null || _RoleList.Count == 0)
            {
                _RoleList = new List<RoleData>();
                for (int i = 0; i < 4; ++i)
                {
                    _RoleList.Add(new RoleData());
                }
            }

            for (int i = 0; i < _RoleList.Count; ++i)
            {
                _RoleList[i].InitRoleData();
                if (i == (int)PROFESSION.BOY_DEFENCE)
                {
                    _RoleList[i].MainBaseName = "MainCharBoy";
                    _RoleList[i].ModelName = "Char_Boy_01_JL_AM";
                    _RoleList[i].Profession = PROFESSION.BOY_DEFENCE;
                    _RoleList[i].DefaultWeaponModel = "Weapon_HW_01_SM";
                }
                else if (i == 1)
                {
                    _RoleList[i].MainBaseName = "MainCharGirl";
                    _RoleList[i].ModelName = "Char_Girl_01_AM";
                    _RoleList[i].Profession = PROFESSION.GIRL_DOUGE;
                    _RoleList[i].DefaultWeaponModel = "Weapon_S_01_SM";
                }
                else if (i == 2)
                {
                    _RoleList[i].MainBaseName = "MainCharBoy";
                    _RoleList[i].ModelName = "Char_Boy_01_AM";
                    _RoleList[i].Profession = PROFESSION.BOY_DOUGE;
                    _RoleList[i].DefaultWeaponModel = "Weapon_HW_01_SM";
                }
                else if (i == 3)
                {
                    _RoleList[i].MainBaseName = "MainCharGirl";
                    _RoleList[i].ModelName = "Char_Girl_02_AM";
                    _RoleList[i].Profession = PROFESSION.GIRL_DEFENCE;
                    _RoleList[i].DefaultWeaponModel = "Weapon_S_01_SM";
                }
            }
        }

        public void SelectRole(int roleIdx)
        {
            if (roleIdx >= 0 && roleIdx < _RoleList.Count)
            {
                _SelectedRole = _RoleList[roleIdx];
            }
        }

        #endregion

    }
}
