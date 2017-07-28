using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;
namespace GameUI
{
    public class UIRoleAttr : UIBase
    {

        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/RoleAttr/UIRoleAttr", UILayer.PopUI, hash);
        }

        #endregion

        #region 

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            InitRoleAttrs();
        }

        #endregion

        public RoleAttrItem _RoleLevel;
        public RoleAttrItem _AttrLevel;
        public RoleAttrItem _UnDistrubutePoint;
        public RoleAttrItem _StrengthItem;
        public RoleAttrItem _DexterityItem;
        public RoleAttrItem _VitalityItem;
        public Button _BtnAddStrength;
        public Button _BtnDexterity;
        public Button _BtnVitality;

        public UIContainerBase _AttrItemContainer;

        private void InitRoleAttrs()
        {
            _RoleLevel.Show("RoleLevel", RoleData.SelectRole._RoleLevel);
            _AttrLevel.Show("AttrLevel", RoleData.SelectRole._AttrLevel);
            _UnDistrubutePoint.Show("UnDistributePoint", RoleData.SelectRole.UnDistrubutePoint);
            _StrengthItem.Show(RoleAttrEnum.Strength.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Strength));
            _DexterityItem.Show(RoleAttrEnum.Dexterity.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Dexterity));
            _VitalityItem.Show(RoleAttrEnum.Vitality.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Vitality));

            if (RoleData.SelectRole.UnDistrubutePoint > 0)
            {
                _BtnAddStrength.gameObject.SetActive(true);
                _BtnDexterity.gameObject.SetActive(true);
                _BtnVitality.gameObject.SetActive(true);
            }
            else
            {
                _BtnAddStrength.gameObject.SetActive(false);
                _BtnDexterity.gameObject.SetActive(false);
                _BtnVitality.gameObject.SetActive(false);
            }

            List<AttrPair> pair = new List<AttrPair>();
            pair.Add(new AttrPair(RoleAttrEnum.Attack.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack).ToString()));
            pair.Add(new AttrPair(RoleAttrEnum.Defense.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Defense).ToString()));
            pair.Add(new AttrPair(RoleAttrEnum.HPMax.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.HPMax).ToString()));

            float moveSpeed = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.MoveSpeed) / 100.0f;
            string moveSpeedStr = moveSpeed + "%";
            pair.Add(new AttrPair(RoleAttrEnum.MoveSpeed.ToString(), moveSpeedStr));

            float attackSpeed = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.AttackSpeed) / 100.0f;
            string attackSpeedStr = attackSpeed + "%";
            pair.Add(new AttrPair(RoleAttrEnum.AttackSpeed.ToString(), attackSpeedStr));

            float criticalHitChance = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.CriticalHitChance) / 100.0f;
            string criticalHitChanceStr = criticalHitChance + "%";
            pair.Add(new AttrPair(RoleAttrEnum.CriticalHitChance.ToString(), criticalHitChanceStr));

            float criticalHitDamage = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.CriticalHitDamge) / 100.0f;
            string criticalHitDamageStr = criticalHitDamage + "%";
            pair.Add(new AttrPair(RoleAttrEnum.CriticalHitDamge.ToString(), criticalHitDamageStr));

            _AttrItemContainer.InitContentItem(pair);
        }

        public void OnDistrubutePoint(int idx)
        {
            RoleData.SelectRole.DistributePoint(idx);
            InitRoleAttrs();
        }

        public void OnResetPoints()
        {
            RoleData.SelectRole.ResetPoints();
            InitRoleAttrs();
        }

        #region exp text

        public InputField _InputExp;

        public void AddTestExp()
        {
            var expValue = int.Parse(_InputExp.text);
            RoleData.SelectRole.AddExp(expValue);
            InitRoleAttrs();
        }



        #endregion
    }
}
