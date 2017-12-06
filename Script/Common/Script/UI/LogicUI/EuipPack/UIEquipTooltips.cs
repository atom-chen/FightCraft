
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;


public class UIEquipTooltips : UIItemTooltips
{

    #region static funs

    public static void ShowAsyn(ItemEquip itemEquip, ToolTipsShowType showType = ToolTipsShowType.ShowForInfo)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemEquip", itemEquip);
        hash.Add("ShowType", showType);
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

    public Button _BtnPutOn;
    public Button _BtnPutOff;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemEquip"] as ItemEquip;
        ToolTipsShowType showType = (ToolTipsShowType)hash["ShowType"];
        ShowTips(_ShowItem as ItemEquip);
        ShowCompare();
        ShowByType(showType);
    }

    protected override void ShowByType(ToolTipsShowType showType)
    {
        base.ShowByType(showType);

        _BtnPutOn.gameObject.SetActive(false);
        _BtnPutOff.gameObject.SetActive(false);

        switch (showType)
        {
            case ToolTipsShowType.ShowInBackPack:
                _BtnPutOn.gameObject.SetActive(true);
                break;
            case ToolTipsShowType.ShowInEquipPack:
                _BtnPutOff.gameObject.SetActive(true);
                break;
        }
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

    #region operate

    public void OnPutOn()
    {
        RoleData.SelectRole.PutOnEquip((_ShowItem as ItemEquip).EquipItemRecord.Slot, (_ShowItem as ItemEquip));
        Hide();
    }

    public void OnPutOff()
    {
        RoleData.SelectRole.PutOffEquip((_ShowItem as ItemEquip).EquipItemRecord.Slot, (_ShowItem as ItemEquip));
        Hide();
    }
    
    #endregion

}

