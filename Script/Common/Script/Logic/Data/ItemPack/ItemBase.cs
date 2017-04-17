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

                    if (_ItemDataID == "-1")
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
        public List<int> _DynamicDataInt;

        [SaveField(3)]
        public List<Vector3> _DynamicDataVector;

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

            var tempData = itembase._DynamicDataInt;
            itembase._DynamicDataInt = _DynamicDataInt;
            _DynamicDataInt = tempData;

            var tempDataVector = itembase._DynamicDataVector;
            itembase._DynamicDataVector = _DynamicDataVector;
            _DynamicDataVector = tempDataVector;

            itembase.ResetItem();
            ResetItem();
        }

        #endregion
    }
}
