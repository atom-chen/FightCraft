
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;


public class UIBackPackItem : /*UIDragableItemBase*/ UIPackItemBase
{

    public Toggle _SellToggle;

    public ItemEquip _BackpackEquip;
    public override void ShowItem(ItemBase showItem)
    {
        base.ShowItem(showItem);

        if (showItem == null || !showItem.IsVolid())
            return;

        if (_Num != null)
        {
            if (showItem is ItemEquip)
            {
                _BackpackEquip = showItem as ItemEquip;
                _Num.text = "";
            }
            else
            {
                if (_ShowedItem.ItemStackNum > 1)
                    _Num.text = _ShowedItem.ItemStackNum.ToString();
                else
                    _Num.text = "";
            }
        }
        _Icon.gameObject.SetActive(true);

        if (_SellToggle != null)
        {
            _SellToggle.gameObject.SetActive(false);
        }
    }

    protected override void ClearItem()
    {
        base.ClearItem();
        if (_Num != null)
        {
            _Num.text = "";
        }
    }

    public override void OnItemClick()
    {
        base.OnItemClick();

        if (_SellToggle != null && _SellToggle.gameObject.activeSelf)
        {
            _SellToggle.isOn = !_SellToggle.isOn;
        }
    }

    #region interaction

    public void SetSellMode(bool isSellMode, Tables.ITEM_QUALITY sellQuality = Tables.ITEM_QUALITY.WHITE)
    {
        if (_SellToggle == null)
            return;

        if (!isSellMode)
        {
            _SellToggle.gameObject.SetActive(false);
        }
        else
        {
            if (_BackpackEquip == null)
            {
                _SellToggle.gameObject.SetActive(false);
            }
            else
            {
                _SellToggle.gameObject.SetActive(true);
                if (_BackpackEquip.EquipQuality > sellQuality)
                {
                    _SellToggle.isOn = false;
                }
                else
                {
                    _SellToggle.isOn = true;
                }
            }

        }
    }

    #endregion
}

