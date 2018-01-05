
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
 



public class UIFuncInFight : UIBase
{

    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UIFuncInFight", UILayer.BaseUI, hash);
    }

    #endregion

    public void OnBtnExit()
    {
        UIMessageBox.Show(100000, OnExitOk, null);
    }

    private void OnExitOk()
    {
        Debug.Log("exit");
        LogicManager.Instance.ExitFight();
    }

}

