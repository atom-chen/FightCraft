
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UIEquipRefresh : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/EquipReset/UIEquipRefresh", UILayer.PopUI, hash);
    }

    public static void Refresh()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIEquipRefresh>("LogicUI/EquipReset/UIEquipRefresh");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        if (instance._TagPanel.GetShowingPage() == 0)
        {
            instance.UpdateRefreshPanel();
        }
    }

    #endregion

    #region 

    public UIBackPack _BackPack;
    public UITagPanel _TagPanel;
    public UIContainerSelect _EquipContainer;
    public Text _EquipTag;
    public UIEquipInfoRefresh _EuipInfo;

    private ItemEquip _SelectedEuqip;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _TagPanel.ShowPage(0);
        InitEquipInPack();
        _EuipInfo.gameObject.SetActive(false);
        //UpdateRefreshPanel();
    }

    private void InitEquipInPack()
    {
        List<ItemEquip> equipList = new List<ItemEquip>();
        foreach (var equipItem in PlayerDataPack.Instance._SelectedRole._EquipList)
        {
            if (equipItem.IsVolid())
            {
                equipList.Add(equipItem);
            }
        }

        List<ItemEquip> equipInBackPack = new List<ItemEquip>();
        foreach (var equipItem in BackBagPack.Instance.PageEquips._PackItems)
        {
            if (equipItem.IsVolid())
            {
                equipInBackPack.Add(equipItem);
            }
        }
        equipInBackPack.Sort((equipA, equipB) =>
        {
            if (equipA.EquipQuality > equipB.EquipQuality)
                return 1;
            return -1;
        });
        equipList.AddRange(equipInBackPack);
        if (equipList.Count < BackBagPack._BAG_PAGE_SLOT_CNT)
        {
            for (int i = equipList.Count; i < BackBagPack._BAG_PAGE_SLOT_CNT; ++i)
            {
                equipList.Add(new ItemEquip());
            }
        }

        _EquipContainer.InitSelectContent(equipList, null, OnSelectedEquip);
    }

    private void OnSelectedEquip(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null)
            return;

        //ShowEquipInfo(equipItem, null);

        if (PlayerDataPack.Instance._SelectedRole._EquipList.Contains(equipItem))
        {
            _EquipTag.text = StrDictionary.GetFormatStr(10013);
        }
        else
        {
            _EquipTag.text = "";
        }

        if (_TagPanel.GetShowingPage() == 0)
        {
            //if (RoleData.SelectRole._EquipList.Contains(equipItem))
            //{
            //    UIEquipTooltips.ShowAsynInType(equipItem, TooltipType.Single, new ToolTipFunc[1] { new ToolTipFunc(10015, PutEnhance) });
            //}
            //else
            //{
            //    UIEquipTooltips.ShowAsynInType(equipItem, TooltipType.Single, new ToolTipFunc[2] { new ToolTipFunc(10015, PutEnhance), new ToolTipFunc(10014, PutDecompose) });
            //}
            PutEnhance(equipItem);
        }
        else if(_TagPanel.GetShowingPage() == 1)
        {
            InitExchangeEquip();
        }
    }

    private void PutEnhance(ItemBase itemBase)
    {
        _SelectedEuqip = itemBase as ItemEquip;
        UpdateRefreshPanel();
        ShowEquipInfo(_SelectedEuqip, null);
    }



    private void ShowEquipInfo(ItemEquip equipItem, ItemEquip orgEquip)
    {
        _EuipInfo.gameObject.SetActive(true);
        _EuipInfo.ShowTips(equipItem, orgEquip);
    }

    public void OnTagPage(int page)
    {
        switch (page)
        {
            case 0:
                InitEquipInPack();
                break;
            case 1:
                InitExchangeEquip();
                break;
        }
    }
    #endregion

    #region equip refresh

    public Text _MaterialTip;
    public GameObject _MaterialBtn;

    public Text _DiamondTip;
    public GameObject _DiamondBtn;

    public Text _FreeTip;
    public GameObject _FreeBtn;

    private ItemEquip _OrgBake;

    private void UpdateRefreshPanel()
    {
        

        if (_SelectedEuqip == null)
        {
            _MaterialBtn.SetActive(false);
            _DiamondBtn.SetActive(false);
            return;
        }

        var refreshCost = EquipRefresh.Instance.GetEquipRefreshCost(_SelectedEuqip);
        var matCnt = BackBagPack.Instance.PageItems.GetItemCnt(EquipRefresh._RefreshMatDataID);
        if (matCnt > refreshCost._MatCnt)
        {
            _MaterialBtn.SetActive(true);
            _DiamondBtn.SetActive(false);
            _MaterialTip.text = matCnt + "/" + refreshCost._MatCnt;
        }
        else
        { 
            _MaterialBtn.SetActive(false);
            _DiamondBtn.SetActive(true);
            _DiamondTip.text = "";
        }

        UpdateFreeBtn();
    }

    private void UpdateFreeBtn()
    {
        _FreeTip.text = EquipRefresh.Instance.LastFreeTimes.ToString();
    }

    public void OnBtnMaterial()
    {
        if (_SelectedEuqip == null)
            return;

        _OrgBake = new ItemEquip();
        _OrgBake.CopyFrom(_SelectedEuqip);

        EquipRefresh.Instance.EquipRefreshMat(_SelectedEuqip);

        UpdateRefreshPanel();
        ShowEquipInfo(_SelectedEuqip, _OrgBake);
    }

    public void OnBtnDiamond()
    {
        if (_SelectedEuqip == null)
            return;

        _OrgBake = new ItemEquip();
        _OrgBake.CopyFrom(_SelectedEuqip);

        EquipRefresh.Instance.EquipRefreshDiamond(_SelectedEuqip);

        UpdateRefreshPanel();
        ShowEquipInfo(_SelectedEuqip, _OrgBake);
    }

    public void OnBtnFree()
    {
        if (_SelectedEuqip == null)
            return;

        _OrgBake = new ItemEquip();
        _OrgBake.CopyFrom(_SelectedEuqip);

        EquipRefresh.Instance.EquipRefreshFree(_SelectedEuqip);

        UpdateRefreshPanel();
        ShowEquipInfo(_SelectedEuqip, _OrgBake);
    }

    public void OnBtnWatchVideoForRefesh()
    {
        EquipRefresh.Instance.WatchVideoForFreeRefresh();
    }

    #endregion

    #region equip destory
    
    public void DetoryEquip()
    {
        
        if (_SelectedEuqip != null && _SelectedEuqip.IsVolid())
        {
            if (RoleData.SelectRole._EquipList.Contains(_SelectedEuqip))
            {
                UIMessageTip.ShowMessageTip(40003);
            }
            else
            {
                var commonTab = TableReader.CommonItem.GetRecord(EquipRefresh._RefreshMatDataID);
                var destoryGetCnt = EquipRefresh.Instance.GetDestoryMatCnt(_SelectedEuqip);
                string tips = StrDictionary.GetFormatStr(40001, commonTab.Name, destoryGetCnt);
                UIMessageBox.Show(tips, DestoryEquipOk, null);
            }
        }
    }

    private void DestoryEquipOk()
    {
        EquipRefresh.Instance.DestoryMatCnt(_SelectedEuqip);
        InitEquipInPack();

        //InitEquipInPack();
    }
    #endregion

    #region exAttrExchange

    //public UIContainerSelect _ExchangeEquipContainer;
    public Text _ExchangeEquipTag;
    public UIEquipInfoRefresh _ExchangeEuipInfo;
    public Text _ExchangeCost;

    private ItemEquip _SelectedExchangeEuqip;

    private void InitExchangeEquip()
    {
        List<ItemEquip> equipList = new List<ItemEquip>();
        foreach (var equipItem in PlayerDataPack.Instance._SelectedRole._EquipList)
        {
            if (equipItem.IsVolid() 
                && equipItem.EquipItemRecord.Slot == _SelectedEuqip.EquipItemRecord.Slot
                && equipItem != _SelectedEuqip)
            {
                equipList.Add(equipItem);
            }
        }

        List<ItemEquip> equipInBackPack = new List<ItemEquip>();
        foreach (var equipItem in BackBagPack.Instance.PageEquips._PackItems)
        {
            if (equipItem.IsVolid() 
                && equipItem.EquipItemRecord.Slot == _SelectedEuqip.EquipItemRecord.Slot
                && equipItem != _SelectedEuqip)
            {
                equipInBackPack.Add(equipItem);
            }
        }
        equipInBackPack.Sort((equipA, equipB) =>
        {
            if (equipA.EquipQuality > equipB.EquipQuality)
                return 1;
            return -1;
        });
        equipList.AddRange(equipInBackPack);


        if (equipList.Count > 0)
        {
            List<ItemEquip> selectedEquip = new List<ItemEquip>();
            selectedEquip.Add(equipList[0]);
            _EquipContainer.InitSelectContent(equipList, selectedEquip, OnExchangeSelectedEquip);
        }
        else
        {
            _EquipContainer.InitSelectContent(equipList, null, OnExchangeSelectedEquip);
            OnExchangeSelectedEquip(null);
        }
    }

    private void OnExchangeSelectedEquip(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null)
        {
            ShowExchangeEquipInfo(null, null);
            return;
        }

        _SelectedExchangeEuqip = equipItem;
        ShowExchangeEquipInfo(equipItem, null);

        if (PlayerDataPack.Instance._SelectedRole._EquipList.Contains(equipItem))
        {
            _ExchangeEquipTag.text = StrDictionary.GetFormatStr(10013);
        }
        else
        {
            _ExchangeEquipTag.text = "";
        }
        _ExchangeCost.text = (_SelectedExchangeEuqip.EquipLevel * _SelectedEuqip.EquipLevel).ToString();
    }

    private void ShowExchangeEquipInfo(ItemEquip equipItem, ItemEquip orgEquip)
    {
        _ExchangeEuipInfo.ShowTips(equipItem, orgEquip);
    }

    public void OnBtnExchange()
    {
        if (_SelectedEuqip == null || _SelectedExchangeEuqip == null)
            return;

        if (_SelectedEuqip == _SelectedExchangeEuqip)
        {
            UIMessageTip.ShowMessageTip(40002);
            return;
        }

        EquipRefresh.Instance.ExchangeExAttr(_SelectedEuqip, _SelectedExchangeEuqip);

        ShowExchangeEquipInfo(_SelectedExchangeEuqip, null);
        ShowEquipInfo(_SelectedEuqip, null);
    }

    #endregion
}

