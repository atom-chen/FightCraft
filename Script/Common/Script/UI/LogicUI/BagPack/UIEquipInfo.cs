
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

 



public class UIEquipInfo : UIBase
{

    #region 

    public Text _Name;
    public Text _Level;
    public Text _Value;
    public Text _BaseAttr;
    public UIContainerBase _AttrContainer;

    #endregion

    #region 

    private ItemEquip _ShowItem;

    public void ShowTips(ItemEquip itemEquip)
    {
        if (itemEquip == null || !itemEquip.IsVolid())
        {
            _ShowItem = null;
            return;
        }

        _ShowItem = itemEquip;
        _Name.text = _ShowItem.GetEquipNameWithColor();
        if (_ShowItem.RequireLevel > RoleData.SelectRole._RoleLevel)
        {
            _Level.text = StrDictionary.GetFormatStr(10000) + " " + CommonDefine.GetEnableColorStr(0) + _ShowItem.RequireLevel + "</color>";
        }
        else
        {
            _Level.text = StrDictionary.GetFormatStr(10000) + " " + _ShowItem.RequireLevel;
        }
        _Value.text = StrDictionary.GetFormatStr(10001) + " " + _ShowItem.EquipValue;
        string attrStr = _ShowItem.GetBaseAttrStr();
        if (string.IsNullOrEmpty(attrStr))
        {
            _BaseAttr.gameObject.SetActive(false);
        }
        else
        {
            _BaseAttr.gameObject.SetActive(true);
            _BaseAttr.text = attrStr;
        }
        Hashtable hash = new Hashtable();
        hash.Add("ItemEquip", _ShowItem);
        _AttrContainer.InitContentItem(itemEquip._DynamicDataVector, null, hash);
    }
    #endregion



}

