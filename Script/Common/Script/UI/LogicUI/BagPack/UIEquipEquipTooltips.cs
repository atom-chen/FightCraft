
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
    public class UIEquipEquipTooltips : UIBase
    {

        #region static funs

        public static void ShowAsyn(ItemEquip itemEquip)
        {
            Hashtable hash = new Hashtable();
            hash.Add("ItemEquip", itemEquip);
            GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIEquipEquipTooltips", hash);
        }

        #endregion

        #region 



        #endregion

        #region 

        private ItemEquip _ShowItem;

        public override void Show(Hashtable hash)
        {
            base.Show(hash);
            ItemEquip itemEquip = hash["ItemEquip"] as ItemEquip;
            ShowTips(itemEquip);
        }

        private void ShowTips(ItemEquip itemEquip)
        {
            if (itemEquip == null)
            {
                _ShowItem = null;
                return;
            }

            _ShowItem = itemEquip;
        }
        #endregion

        

    }
}
