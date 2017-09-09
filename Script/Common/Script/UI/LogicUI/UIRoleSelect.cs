using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRoleSelect : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/UIRoleSelect", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public Text _RoleLevel;
    public Text _AttrLevel;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        SelectRole(_SelectRoleID);
    }

    #endregion

    #region event

    private int _SelectRoleID = 0;

    public void SelectRole(int roleID)
    {
        _SelectRoleID = roleID;
        Debug.Log("_RoleList.Count:" + PlayerDataPack.Instance._RoleList.Count);
        _RoleLevel.text = PlayerDataPack.Instance._RoleList[_SelectRoleID]._RoleLevel.ToString();
        _AttrLevel.text = PlayerDataPack.Instance._RoleList[_SelectRoleID]._AttrLevel.ToString();
    }

    public void OnBtnOK()
    {
        LogicManager.Instance.StartLoadRole(_SelectRoleID);
        Hide();
    }
    #endregion
}

