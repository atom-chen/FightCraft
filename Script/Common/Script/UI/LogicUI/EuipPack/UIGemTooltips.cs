
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;


public class UIGemTooltips : UIItemTooltips
{

    #region static funs

    public new static void ShowAsyn(ItemBase itemBase, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemBase", itemBase);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI("LogicUI/Gem/UIGemTooltips", UILayer.MessageUI, hash);
    }

    public new static void HideAsyn()
    {
        UIManager.Instance.HideUI("LogicUI/Gem/UIGemTooltips");
    }

    #endregion

    #region 

    #endregion

    #region 

    protected override void ShowTips(ItemBase itemBase)
    {
        if (itemBase == null)
        {
            _ShowItem = null;
            return;
        }
        _ShowItem = itemBase;

        _UIItemInfo.ShowTips(_ShowItem);
    }

    #endregion


}

