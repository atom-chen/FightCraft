
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;



public class UIBackPackItem : UIItemSelect
{
    public Image _BG;
    public Image _Icon;
    public Image _Quality;
    public Text _Num;
    public GameObject _DisableGO;
    public GameObject _DropEnable;
    public GameObject _DropDisable;

    protected ItemBase _ShowItem;
    public ItemBase ShowItem
    {
        get
        {
            return _ShowItem;
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (ItemBase)hash["InitObj"];
        ShowEquip(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowEquip(_ShowItem);
    }

    public void ShowEquip(ItemBase showItem)
    {
        if (_DropEnable != null)
            _DropEnable.gameObject.SetActive(false);

        if (showItem == null)
        {
            ClearItem();
            return;
        }

        _ShowItem = showItem;
        if (!showItem.IsVolid())
        {
            ClearItem();
            return;
        }

        if (_Num != null)
        {
            if (showItem is ItemEquip)
            {
                _Num.text = "";
            }
            else
            {
                if (_ShowItem.ItemStackNum > 1)
                    _Num.text = _ShowItem.ItemStackNum.ToString();
                else
                    _Num.text = "";
            }
        }
        _Icon.gameObject.SetActive(true);
    }

    private void ClearItem()
    {
        _Icon.gameObject.SetActive(false);
        _Quality.gameObject.SetActive(false);
        _DisableGO.gameObject.SetActive(false);
        if (_Num != null)
        {
            _Num.text = "";
        }
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    #endregion
}

