
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

    #endregion

    #region 

    public UITagPanel _TagPanel;
    public UIContainerSelect _EquipContainer;
    public Text _EquipTag;
    public UIEquipInfo _EuipInfo;

    private ItemEquip _SelectedEuqip;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _TagPanel.ShowPage(0);
        InitEquipInPack();
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
        ShowEquipInfo(equipItem);

        if (PlayerDataPack.Instance._SelectedRole._EquipList.Contains(equipItem))
        {
            _EquipTag.text = StrDictionary.GetFormatStr(10013);
        }
        else
        {
            _EquipTag.text = "";
        }
    }

    private void ShowEquipInfo(ItemEquip equipItem)
    {
        _EuipInfo.ShowTips(equipItem);
    }
    #endregion

    #region equip refresh

    public Text _MaterialTip;
    public GameObject _MaterialBtn;

    public Text _DiamondTip;
    public GameObject _DiamondBtn;

    public Text _FreeTip;
    public GameObject _FreeBtn;

    private ItemEquip _ShowItem;
    public const string _MaterialDataID = "20000";
    public const int _MatCost = 5;

    private void UpdateRefreshPanel()
    {
        var matCnt = BackBagPack.Instance.GetItemCnt(_MaterialDataID);

    }
    

    #endregion

    #region equip destory


    
    #endregion

}

