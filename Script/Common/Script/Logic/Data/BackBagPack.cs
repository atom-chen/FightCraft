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

        [SaveField(1)]
        private List<ItemEquip> _ItemEquips = new List<ItemEquip>();

        public List<ItemEquip> ItemEquips
        {
            get
            {
                return _ItemEquips;
            }
        }

        public void AddEquip()
        {
            var newItem = new ItemEquip();
            newItem.ItemDataID = _ItemEquips.Count.ToString();
            _ItemEquips.Add(newItem);
        }

    }
}
