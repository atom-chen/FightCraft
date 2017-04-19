
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;
using Tables;

using GameBase;

namespace GameUI
{
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
            _Level.text = StrDictionary.GetFormatStr(5, _ShowItem.RequireLevel);
            _Value.text = StrDictionary.GetFormatStr(4, _ShowItem.EquipValue);
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
}
