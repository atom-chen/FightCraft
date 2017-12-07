
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;



public class UIPackItemBase : UIItemSelect
{
    public Image _BG;
    public Image _Icon;
    public Image _Quality;
    public Text _Num;
    public GameObject _DisableGO;

    protected ItemBase _ShowedItem;
    public ItemBase ShowedItem
    {
        get
        {
            return _ShowedItem;
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (ItemBase)hash["InitObj"];
        ShowItem(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowItem(_ShowedItem);
    }

    public virtual void ShowItem(ItemBase showItem)
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

        SetGOActive(_Icon, true);
    }

    protected virtual void ClearItem()
    {
        SetGOActive(_Icon, false);
        SetGOActive(_Quality, false);
        SetGOActive(_DisableGO, false);
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

