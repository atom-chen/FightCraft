
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemItem : /*UIDragableItemBase*/ UIPackItemBase
{
    public GameObject _UsingGO;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var showItem = (ItemGem)hash["InitObj"];
        ShowGem(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_ShowedItem as ItemGem);
    }

    public void ShowGem(ItemGem showItem)
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
                _Num.text = showItem.Level.ToString();
            }
        }

        if (_DisableGO != null)
        {
            if (showItem.Level > 0)
            {
                _DisableGO.SetActive(false);
            }
            else
            {
                _DisableGO.SetActive(true);
            }
        }

        if (_UsingGO != null)
        {
            if (GemData.Instance.IsEquipedGem(showItem.ItemDataID))
            {
                _UsingGO.SetActive(true);
            }
            else
            {
                _UsingGO.SetActive(false);
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

    //public override void OnItemClick()
    //{
    //    base.OnItemClick();
    //}

    //protected override bool IsCanDrag()
    //{
    //    if (_ShowedItem.IsVolid() && _ShowedItem.ItemStackNum > 0)
    //        return true;
    //    return false;
    //}

    #endregion
}

