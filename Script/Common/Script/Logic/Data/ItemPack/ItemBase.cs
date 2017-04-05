using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

namespace GameLogic
{
    public class ItemBase
    {
        [SaveField(1)]
        protected string _ItemDataID;
        public string ItemDataID
        {
            get
            {
                return _ItemDataID;
            }  
            set
            {
                _ItemDataID = value;
            }
        }

        private EquipItemRecord _EquipItemRecord;
        public EquipItemRecord EquipItemRecord
        {
            get
            {
                if (_EquipItemRecord == null)
                {
                    if (string.IsNullOrEmpty(_ItemDataID))
                        return null;

                    _EquipItemRecord = TableReader.EquipItem.GetRecord(_ItemDataID);
                }
                return _EquipItemRecord;
            }
        }

        public bool IsVolid()
        {
            if (string.IsNullOrEmpty(_ItemDataID) || _ItemDataID == "-1")
                return false;
            return true;
        }

        [SaveField(2)]
        public List<int> _DynamicData;

        #region fun

        public void ResetItem()
        {
            _EquipItemRecord = null;
        }

        public void ExchangeInfo(ItemBase itembase)
        {
            if (itembase == null)
                return;

            var tempId = itembase.ItemDataID;
            itembase.ItemDataID = ItemDataID;
            ItemDataID = tempId;

            var tempData = itembase._DynamicData;
            itembase._DynamicData = _DynamicData;
            _DynamicData = tempData;

            itembase.ResetItem();
            ResetItem();
        }

        #endregion
    }
}
