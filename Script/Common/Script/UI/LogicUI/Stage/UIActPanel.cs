﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIActPanel : UIBase
{

    #region static funs

    public static void ShowAsyn(bool isShowTip = false)
    {
        Hashtable hash = new Hashtable();
        hash.Add("IsShowTip", isShowTip);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIActPanel, UILayer.PopUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        OnHideTipTicket();
        if (hash.ContainsKey("IsShowTip"))
        {
            if ((bool)hash["IsShowTip"])
            {
                OnShowTipTicket();
            }
        }
    }

    #region 

    public GameObject _TipGetTicket;

    #endregion

    #region 

    public void OnShowTipTicket()
    {
        _TipGetTicket.SetActive(true);
    }

    public void OnHideTipTicket()
    {
        _TipGetTicket.SetActive(false);
    }

    public void OnBtnNormalEnter()
    {
        ActData.Instance.StartStage(1, STAGE_TYPE.ACT_GOLD, false);
        Hide();
    }

    public void OnBtnTicketEnter()
    {
        ActData.Instance.StartStage(1, STAGE_TYPE.ACT_GOLD, true);
        Hide();
    }

    public void OnBtnAdTicket()
    {
        ActData.Instance.AddActTicket();
    }

    public void OnBtnBuyTicket()
    {
        ActData.Instance.AddActTicket();
    }

    #endregion

}
