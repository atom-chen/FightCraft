
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

 
using UnityEngine.EventSystems;
using System;
using Tables;

 



public class UIEquipResetAttr : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/EquipReset/UIEquipResetAttr", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public UIContainerSelect _EquipContainer;

    public Image _ItemImage;
    public Text _ItemName;
    public Text _ItemNum;
    public Button _ItemBtn;

    //public Image _MoneyImage;
    public Text _MoneyName;
    public Text _MoneyNum;
    public Button _MoneyBtn;

    #endregion

    #region 

    private ItemEquip _ShowItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowEquips();
        SetCostItem(null);
        SetCostMoney(null);
    }

    public override void Hide()
    {
        base.Hide();

        UIEquipTooltips.HideAsyn();
    }

    public void ShowEquips()
    {
        List<ItemEquip> equips = new List<ItemEquip>();
        foreach (var equip in PlayerDataPack.Instance._SelectedRole._EquipList)
        {
            if (equip.EquipItemRecord != null)
            {
                equips.Add(equip);
            }
        }
        foreach (var equip in BackBagPack.Instance.PageEquips)
        {
            if (equip.EquipItemRecord != null)
            {
                equips.Add(equip);
            }
        }

        for (int i = equips.Count; i < BackBagPack._BAG_PAGE_SLOT_CNT; ++i)
        {
            equips.Add(new ItemEquip() { ItemDataID = "-1" });
        }

        if (equips.Count > 0)
        {
            _EquipContainer.InitSelectContent(equips, new List<ItemEquip>() { equips[0] }, SelectedEquip);
        }
        else
        {
            _EquipContainer.InitSelectContent(equips, null);
        }
    }

    private void SelectedEquip(object selectEquip)
    {
        _ShowItem = selectEquip as ItemEquip;
        if (_ShowItem == null || _ShowItem.EquipItemRecord == null)
        {
            SetCostItem(null);
            SetCostMoney(null);
            UIEquipTooltips.HideAsyn();
            return;
        }

        SetCostItem(_ShowItem);
        SetCostMoney(_ShowItem);

        UIEquipTooltips.ShowAsyn(_ShowItem);
    }

    private void SetCostItem(ItemEquip itemEquip)
    {
        if (itemEquip == null)
        {
            _ItemImage.gameObject.SetActive(false);
            _ItemName.text = "";
            _ItemNum.text = "";
            _ItemBtn.interactable = false;
        }
        else
        {
            _ItemImage.gameObject.SetActive(true);
            _ItemName.text = itemEquip.ResetCostItem.Name;
            _ItemNum.text = itemEquip.ResetCostItemNum.ToString();
            _ItemBtn.interactable = true;
        }
    }

    private void SetCostMoney(ItemEquip itemEquip)
    {
        if (itemEquip == null)
        {
            _MoneyName.text = "";
            _MoneyNum.text = "";
            _MoneyBtn.interactable = false;
        }
        else
        {
            _MoneyName.text = itemEquip.ResetCostItem.Name;
            _MoneyNum.text = itemEquip.ResetCostItemNum.ToString();
            _MoneyBtn.interactable = true;
        }
    }

    #endregion

    #region act

    public void OnBtnItemReset()
    {
        if (_ShowItem == null)
            return;

        _ShowItem.ResetEquipAttr();
        UIEquipTooltips.ShowAsyn(_ShowItem);
    }

    public void OnBtnMoneyReset()
    {
        if (_ShowItem == null)
            return;

        _ShowItem.ResetEquipAttr();
        UIEquipTooltips.ShowAsyn(_ShowItem);
    }

    #endregion

}

