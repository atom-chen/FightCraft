using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

namespace GameLogic
{
    public class ItemBase
    {
        [SaveField(1)]
        private string _ItemDataID;
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
        private List<int> _DynamicData;
    }
}
