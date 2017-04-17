
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;
using Tables;

using GameBase;

namespace GameUI
{
    public class UITestEquip : UIBase
    {

        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UITestEquip", hash);
        }

        #endregion

        #region 

        public InputField _InputLevel;
        public InputField _InputQuality;
        public InputField _InputValue;

        #endregion

        #region 

        public override void Show(Hashtable hash)
        {
            base.Show(hash);
        }


        public void OnBtnOk()
        {
            int level = int.Parse(_InputLevel.text);
            ITEM_QUALITY quality = (ITEM_QUALITY)(int.Parse(_InputQuality.text));
            int value = int.Parse(_InputValue.text);

            Debug.Log("Input :" + level + "," + quality + "," + value);
        }
        #endregion

        

    }
}
