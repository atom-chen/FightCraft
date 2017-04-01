using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;
namespace GameUI
{
    public class UIFightControl : UIBase
    {

        #region static funs

        public static List<EVENT_TYPE> GetShowEvent()
        {
            List<EVENT_TYPE> showEvents = new List<EVENT_TYPE>();

            showEvents.Add(EVENT_TYPE.EVENT_FIGHT_START);

            return showEvents;
        }

        #endregion

        #region 



        #endregion

        #region 

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            Debug.Log("Star fight");
        }

        public void OnEnable()
        {

        }

        #endregion

        #region event

        //fight
        public void BtnFight()
        {
            GameCore.Instance.UIManager.ShowUI("LogicUI/UIModeSelect");
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
