using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

namespace GameLogic
{
    public class EquipExAttr
    {
        [SaveField(1)]
        public FightAttr.FightAttrType AttrID;
        [SaveField(2)]
        public int AttrValue1;
        [SaveField(3)]
        public int AttrValue2;
    }

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
        public List<int> _DynamicDataInt = new List<int>();

        [SaveField(3)]
        public List<EquipExAttr> _DynamicDataVector = new List<EquipExAttr>();

        #region fun

        public void RefreshItemData()
        {
            _EquipItemRecord = null;
        }

        public void ResetItem()
        {
            _ItemDataID = "-1";
            _EquipItemRecord = null;
            _DynamicDataInt = new List<int>();
            _DynamicDataVector = new List<EquipExAttr>();
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

            itembase.RefreshItemData();
            RefreshItemData();
        }

        #endregion
    }
}
