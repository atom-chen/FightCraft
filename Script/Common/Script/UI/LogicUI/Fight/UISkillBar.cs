using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public class UISkillBar : UIBase
    {
        #region static

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UISkillBar", UILayer.BaseUI, hash);
        }

        public static void SetAimTypeStatic(AimTarget.AimTargetType aimType)
        {
            var instance = GameCore.Instance.UIManager.GetUIInstance<UISkillBar>("LogicUI/Fight/UISkillBar");
            if (instance == null)
                return;

            if (!instance.isActiveAndEnabled)
                return;

            instance.SetAimType(aimType);
        }

        #endregion

        public UIPressBtn _ButtonJ;
        public UIPressBtn _ButtonK;
        public UIPressBtn _ButtonL;
        public UIPressBtn _ButtonU;

        private Dictionary<string, UIPressBtn> _Buttons = new Dictionary<string, UIPressBtn>();

        public override void Init()
        {
            base.Init();

            _Buttons.Add("j", _ButtonJ);
            _Buttons.Add("k", _ButtonK);
            _Buttons.Add("l", _ButtonL);
            _Buttons.Add("u", _ButtonU);
        }

        public bool IsKeyDown(string key)
        {
            if (_Buttons.ContainsKey(key))
            {
                return _Buttons[key].IsPress;
            }
            return false;
        }

        #region emulate

        public void OnEmulate()
        {
            var testFight = FightManager.Instance.MainChatMotion.gameObject.GetComponent<TestFight>();
            if(testFight == null)
                FightManager.Instance.MainChatMotion.gameObject.AddComponent<TestFight>();
        }

        #endregion

        #region switch aim

        public Text _AimText;

        private int _AimType = 0;
        public void OnSwitchAimType()
        {
            ++_AimType;
            if (_AimType >= 3)
                _AimType = 0;

            AimTarget.Instance.SwitchAimType(_AimType);
            UpdateAim();
        }

        public void SetAimType(AimTarget.AimTargetType aimType)
        {
            _AimType = (int)aimType;
            UpdateAim();
        }

        private void UpdateAim()
        {
            switch ((AimTarget.AimTargetType)_AimType)
            {
                case AimTarget.AimTargetType.None:
                    _AimText.text = "N";
                    break;
                case AimTarget.AimTargetType.Free:
                    _AimText.text = "F";
                    break;
                case AimTarget.AimTargetType.Lock:
                    _AimText.text = "L";
                    break;
            }
        }

        #endregion
    }
}
