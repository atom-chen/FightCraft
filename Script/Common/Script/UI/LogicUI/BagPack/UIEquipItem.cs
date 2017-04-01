
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;

namespace GameUI
{
    public class UIEquipItem : UIItemSelect
    {
        public Image _BG;
        public Image _Icon;
        public Image _Quality;
        public GameObject _DisableGO;

        private ItemBase _ShowItem;

        public override void Show(Hashtable hash)
        {
            base.Show();

            _ShowItem = (ItemBase)hash["InitObj"];
            ShowEquip(_ShowItem as ItemEquip);
        }

        public void ShowEquip(ItemEquip showItem)
        {
            if (showItem == null)
                return;

            if (showItem.IsVolid())
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
