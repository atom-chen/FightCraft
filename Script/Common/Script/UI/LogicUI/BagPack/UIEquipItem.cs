
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;

namespace GameUI
{
    public class UIEquipItem : UIItemSelect
    {
        public Image _BG;
        public Image _Icon;
        public Image _Quality;
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

        protected UIBagPack _BagPack;

        public override void Show(Hashtable hash)
        {
            base.Show();

            var showItem = (ItemBase)hash["InitObj"];
            if (hash.ContainsKey("UIBagPack"))
            {
                _BagPack = (UIBagPack)hash["UIBagPack"];
            }
            ShowEquip(showItem as ItemEquip);
        }

        public override void Refresh()
        {
            base.Refresh();

            ShowEquip(_ShowItem as ItemEquip);
        }

        public void ShowEquip(ItemEquip showItem)
        {
            if(_DropEnable != null)
                _DropEnable.gameObject.SetActive(false);

            if (showItem == null)
                return;

            _ShowItem = showItem;
            if (!showItem.IsVolid())
            {
                _Icon.gameObject.SetActive(false);
                _Quality.gameObject.SetActive(false);
                _DisableGO.gameObject.SetActive(false);
                return;
            }

            _Icon.gameObject.SetActive(true);
        }


    }
}
