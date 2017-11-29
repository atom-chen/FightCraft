using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMainFun : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/UIMainFun", UILayer.BaseUI, hash);
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
        UIStageSelect.ShowAsyn();
        //LogicManager.Instance.EnterFight("Stage_01_01");
    }

    public void BtnBagPack()
    {
        UIEquipPack.ShowAsyn();
    }

    public void BtnTestPanel()
    {
        UITestEquip.ShowAsyn();
    }

    public void BtnResetEquip()
    {
        UIEquipResetAttr.ShowAsyn();
    }

    public void BtnSkill()
    {
        UISkillLevelUp.ShowAsyn();
    }

    public void BtnAttr()
    {
        UIRoleAttr.ShowAsyn();
    }

    public void BtnTest()
    {
        UIMessageTip.ShowMessageTip("test message");
    }
    #endregion
}

