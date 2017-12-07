
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;


public class UIEquipTooltips : UIItemTooltips
{

    #region static funs

    public static void ShowAsyn(ItemEquip itemEquip, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemEquip", itemEquip);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIEquipTooltips", UILayer.MessageUI, hash);
    }

    public new static void HideAsyn()
    {
        UIManager.Instance.HideUI("LogicUI/BagPack/UIEquipTooltips");
    }

    #endregion

    #region 

    public UIEquipInfo _UIEquipInfo;
    public UIEquipInfo _CompareEquipInfo;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemEquip"] as ItemEquip;
        ToolTipFunc[] funcs = (ToolTipFunc[])hash["ToolTipFun"];
        ShowTips(_ShowItem as ItemEquip);
        ShowCompare();
        ShowFuncs(funcs);
    }
    
    private void ShowTips(ItemEquip itemEquip)
    {
        if (itemEquip == null)
        {
            _ShowItem = null;
            return;
        }
        _ShowItem = itemEquip;

        _UIEquipInfo.ShowTips(_ShowItem as ItemEquip);
    }

    private void ShowCompare()
    {
        HideCompare();
    }

    private void HideCompare()
    {
        _CompareEquipInfo.gameObject.SetActive(false);
    }

    #endregion


}

