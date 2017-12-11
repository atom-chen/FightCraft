
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemItem : UIDragableItemBase
{
    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (ItemBase)hash["InitObj"];
        ShowGem(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_ShowedItem);
    }

    public void ShowGem(ItemBase showItem)
    {
        if (showItem == null)
        {
            ClearItem();
            return;
        }

        _ShowedItem = showItem;
        if (!showItem.IsVolid())
        {
            ClearItem();
            return;
        }

        if (_Num != null)
        {
            {
                _Num.text = _ShowedItem.ItemStackNum.ToString();
            }
        }

        if (_DisableGO != null)
        {
            if (_ShowedItem.ItemStackNum > 0)
            {
                _DisableGO.SetActive(false);
            }
            else
            {
                _DisableGO.SetActive(true);
            }
        }
        _Icon.gameObject.SetActive(true);
    }

    protected override void ClearItem()
    {
        base.ClearItem();

        if (_DisableGO != null)
        {

            _DisableGO.SetActive(false);
        }
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    protected override bool IsCanDrag()
    {
        if (_ShowedItem.IsVolid() && _ShowedItem.ItemStackNum > 0)
            return true;
        return false;
    }

    #endregion
}

