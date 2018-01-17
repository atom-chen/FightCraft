﻿
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
    public UIEquipSetInfo _EquipSetInfo;

    private ItemEquip _ShowEquip;
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemEquip"] as ItemEquip;
        ToolTipFunc[] funcs = (ToolTipFunc[])hash["ToolTipFun"];
        ShowTips(_ShowItem as ItemEquip);
        if (_ShowEquip.SpSetRecord != null)
        {
            _EquipSetInfo.gameObject.SetActive(true);
            _EquipSetInfo.ShowTips(_ShowEquip);
        }
        else
        {
            _EquipSetInfo.gameObject.SetActive(false);
        }
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
        _ShowEquip = itemEquip;

        _UIEquipInfo.ShowTips(_ShowEquip);
    }

    private void ShowCompare()
    {
        HideCompare();
        if (_ShowEquip == null)
            return;

        var equipingItem = RoleData.SelectRole.GetEquipItem(_ShowEquip.EquipItemRecord.Slot);
        if (equipingItem == null || !equipingItem.IsVolid())
            return;

        if (equipingItem == _ShowEquip)
            return;

        _CompareEquipInfo.gameObject.SetActive(true);
        _CompareEquipInfo.ShowTips(equipingItem);
    }

    private void HideCompare()
    {
        _CompareEquipInfo.gameObject.SetActive(false);
    }

    #endregion


}

