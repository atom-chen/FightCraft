using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;
using System;

namespace GameUI
{
    public class UIFightFinish : UIBase
    {

        #region static funs

        public static void ShowAsyn(bool isWin)
        {
            Hashtable hash = new Hashtable();
            hash.Add("IsWin", isWin);
            GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UIFightFinish", UILayer.BaseUI, hash);
        }

        #endregion

        #region 



        #endregion

        #region 

        public GameObject _WinGO;
        public GameObject _LoseGO;

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            bool isWin = (bool)hash["IsWin"];
            if (_WinGO)
            {
                _WinGO.SetActive(true);
                _LoseGO.SetActive(false);
            }
            else
            {
                _WinGO.SetActive(false);
                _LoseGO.SetActive(true);
            }
        }

        public void OnEnable()
        {

        }

        #endregion

        #region event

        public void OnBtnExitFight()
        {
            FightManager.Instance.LogicFinish(true);
        }

        #endregion
    }
}
