
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System;

public class UIFuncInFight : UIBase
{

    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UIFuncInFight", UILayer.BaseUI, hash);

    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        StartFightTime();
    }

    public void OnBtnExit()
    {
        UIMessageBox.Show(100000, OnExitOk, null);
    }

    private void OnExitOk()
    {
        Debug.Log("exit");
        LogicManager.Instance.ExitFight();
    }

    #region fight time

    public Text _FightTime;

    private int _FightSecond = 0;

    private void StartFightTime()
    {
        InvokeRepeating("UpdateFightTime", 1, 1);
    }

    private void UpdateFightTime()
    {
        ++_FightSecond;
        DateTime dateTime = new DateTime((long)(_FightSecond * 10000000L));
        _FightTime.text = string.Format("{0:mm:ss}", dateTime);
    }

    #endregion
}

