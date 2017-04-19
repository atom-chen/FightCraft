using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;
namespace GameUI
{
    public class UIRoleSelect : UIBase
    {

        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/UIRoleSelect", UILayer.PopUI, hash);
        }

        public static List<EVENT_TYPE> GetShowEvent()
        {
            List<EVENT_TYPE> showEvents = new List<EVENT_TYPE>();

            showEvents.Add(EVENT_TYPE.EVENT_LOGIC_LOGIC_START);

            return showEvents;
        }

        #endregion

        #region 



        #endregion

        #region 

        public override void Show(Hashtable hash)
        {
            base.Show(hash);
        }

        public void OnEnable()
        {

        }

        #endregion

        #region event

        public void SelectRole(int roleID)
        {
            PlayerDataPack.Instance.SelectRole(roleID);
            Hide();

            UIMainFun.ShowAsyn();
        }

        #endregion
    }
}
