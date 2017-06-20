using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic
{
    public class BackBagPack : DataPackBase
    {
        #region 单例

        private static BackBagPack _Instance;
        public static BackBagPack Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BackBagPack();
                }
                return _Instance;
            }
        }

        private BackBagPack()
        {
            GuiTextDebug.debug("BackBagPack init");
        }

        #endregion

        public const int _BAG_PAGE_SLOT_CNT = 25;

        [SaveField(1)]
        private List<ItemEquip> _ItemEquips = new List<ItemEquip>();
        public List<ItemEquip> ItemEquips
        {
            get
            {
                return _ItemEquips;
            }
        }

        [SaveField(2)]
        private List<ItemBase> _ItemBases = new List<ItemBase>();
        public List<ItemBase> ItemBases
        {
            get
            {
                return _ItemBases;
            }
        }


        public bool AddEquip(ItemEquip equip)
        {
            if (ItemEquips.Count >= _BAG_PAGE_SLOT_CNT)
            {
                return false;
            }

            ItemEquips.Add(equip);
            LogicManager.Instance.SaveGame();
            return true;
        }

        public bool AddItem(ItemBase item)
        {
            if (ItemBases.Count >= _BAG_PAGE_SLOT_CNT)
            {
                return false;
            }

            ItemBases.Add(item);
            LogicManager.Instance.SaveGame();
            return true;
        }

    }
}
