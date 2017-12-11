﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

 



public class UITestEquip : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UITestEquip", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public InputField _LegencyID;
    public InputField _InputLevel;
    public InputField _InputQuality;
    public InputField _InputValue;

    public InputField _ItemID;
    public InputField _ItemCnt;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
    }

    public override void Hide()
    {
        base.Hide();

        UIEquipTooltips.HideAsyn();
    }

    public void OnBtnOk()
    {
        int level = int.Parse(_InputLevel.text);
        ITEM_QUALITY quality = (ITEM_QUALITY)(int.Parse(_InputQuality.text));
        int value = int.Parse(_InputValue.text);
        int legencyID = int.Parse(_LegencyID.text);
        var equipItem = ItemEquip.CreateEquip(level, quality, value, legencyID);
        var newEquip = BackBagPack.Instance.AddNewEquip(equipItem);
        //UIEquipTooltips.ShowAsyn(newEquip);
        Debug.Log("test equip :" + newEquip.EquipItemRecord.Id);
    }

    public void OnBtnItem()
    {
        int itemCnt = int.Parse(_ItemCnt.text);

        ItemBase.CreateItemInPack(_ItemID.text, itemCnt);
    }
    #endregion

}

