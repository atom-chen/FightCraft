
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
        UpdateRefreshPanel();
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
        foreach (var equipItem in BackBagPack.Instance.PageEquips)
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

        List<ItemEquip> selectedEquip = new List<ItemEquip>();
        if (equipList.Count > 0)
        {
            selectedEquip.Add(equipList[0]);
        }
        _EquipContainer.InitSelectContent(equipList, selectedEquip, OnSelectedEquip);
    }

    private void OnSelectedEquip(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null)
            return;

        _SelectedEuqip = equipItem;
        ShowEquipInfo(equipItem, null);

        if (PlayerDataPack.Instance._SelectedRole._EquipList.Contains(equipItem))
        {
            _EquipTag.text = StrDictionary.GetFormatStr(10013);
        }
        else
        {
            _EquipTag.text = "";
        }

        UpdateRefreshPanel();
    }

    private void ShowEquipInfo(ItemEquip equipItem, ItemEquip orgEquip)
    {
        _EuipInfo.ShowTips(equipItem, orgEquip);
    }

    public void OnTagPage(int page)
    {
        switch (page)
        {
            case 0:
                
                UpdateRefreshPanel();
                break;
            case 1:
                UpdateEquipPack();
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
        var matCnt = BackBagPack.Instance.GetItemCnt(EquipRefresh._RefreshMatDataID);
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

    public UIContainerSelect _EquipPack;

    private ItemEquip _DestorySelectedEquip;

    public void UpdateEquipPack()
    {
        List<ItemEquip> destoryEquipList = new List<ItemEquip>();
        foreach (var itemEquip in BackBagPack.Instance.PageEquips)
        {
            if (itemEquip.EquipQuality == ITEM_QUALITY.PURPER || itemEquip.EquipQuality == ITEM_QUALITY.ORIGIN)
            {
                destoryEquipList.Add(itemEquip);
            }
        }
        _EquipPack.InitContentItem(destoryEquipList, ShowEquipPackTooltips);
    }

    private void ShowEquipPackTooltips(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        _DestorySelectedEquip = equipItem;
        UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10014, DetoryEquip) });
    }

    private void DetoryEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            var commonTab = TableReader.CommonItem.GetRecord(EquipRefresh._RefreshMatDataID);
            var destoryGetCnt = EquipRefresh.Instance.GetDestoryMatCnt(itemEquip);
            string tips = StrDictionary.GetFormatStr(40001, commonTab.Name, destoryGetCnt);
            UIMessageBox.Show(tips, DestoryEquipOk, null);
        }
    }

    private void DestoryEquipOk()
    {
        EquipRefresh.Instance.DestoryMatCnt(_DestorySelectedEquip);
        UpdateEquipPack();

        InitEquipInPack();
    }
    #endregion

}

