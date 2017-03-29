using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;
namespace GameUI
{
    public class UIMainFun : UIConflictBase
    {

        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/UIMainFun", hash);
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

        //fight
        public void BtnFight()
        {
            //GameCore.Instance.UIManager.ShowUI("LogicUI/UIModeSelect");
            LogicManager.Instance.EnterFight("Stage_01_01");
        }

        //lottery
        public void BtnLottery()
        {
            GameCore.Instance.UIManager.ShowUI("LogicUI/UILottery");
        }

        //shop
        public void BtnShop()
        {
            GameCore.Instance.UIManager.ShowUI("LogicUI/UIShopRes");
        }

        //mission
        public void BtnMission()
        {
            //UIMissionPack.ShowAsyn();
        }

        #endregion
    }
}
