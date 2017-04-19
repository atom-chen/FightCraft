
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;
using Tables;

namespace GameUI
{
    public class UIEquipAttrItem : UIItemBase
    {
        public Text _AttrText;

        private ItemEquip _ItemEquip;
        private EquipExAttr _ShowAttr;

        public override void Show(Hashtable hash)
        {
            base.Show();

            var showItem = (EquipExAttr)hash["InitObj"];
            _ItemEquip = (ItemEquip)hash["ItemEquip"];

            ShowAttr(showItem);
        }

        public override void Refresh()
        {
            base.Refresh();

            ShowAttr(_ShowAttr);
        }

        public void ShowAttr(EquipExAttr attr)
        {
            if (attr.AttrID <= 0)
                return;

            _ShowAttr = attr;

            var attrTab = TableReader.FightAttr.GetRecord(_ShowAttr.AttrID.ToString());
            string attrStr = string.Format(attrTab.ShowTip, attr.AttrValue1);
            if (_ItemEquip != null)
            {
                attrStr = CommonDefine.GetQualityColorStr(_ItemEquip.EquipQuality) + attrStr + "</color>";
            }
            _AttrText.text = attrStr;
        }


    }
}
