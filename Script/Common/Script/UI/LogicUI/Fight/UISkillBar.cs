using UnityEngine;
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
            GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UISkillBar", hash);
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
    }
}
