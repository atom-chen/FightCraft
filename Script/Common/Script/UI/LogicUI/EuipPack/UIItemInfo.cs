
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIItemInfo : UIBase
{

    #region 

    public Text _Name;
    public Text _Level;
    public Text _Desc;

    #endregion

    #region 

    protected ItemBase _ShowItem;

    public virtual void ShowTips(ItemBase itemBase)
    {
        if (itemBase == null || !itemBase.IsVolid())
        {
            _ShowItem = null;
            return;
        }

        _ShowItem = itemBase;
        _Name.text = _ShowItem.CommonItemRecord.Name;

        if (_Desc != null)
        {
            _Desc.text = _ShowItem.CommonItemRecord.Desc;
        }
    }
    #endregion



}

