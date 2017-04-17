using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class EquipItemRecord : TableRecordBase
    {
        private CommonItemRecord _CommonItem;
        public CommonItemRecord CommonItem
        {
            get
            {
                if (_CommonItem == null)
                {
                    _CommonItem = TableReader.CommonItem.GetRecord(Id);
                }
                return _CommonItem;
            }
        }

        public string Model
        {
            get
            {
                return CommonItem.Model;
            }
        }
    }

    public partial class EquipItem : TableFileBase
    {
        private Dictionary<EQUIP_SLOT, List<EquipItemRecord>> _ClassedEquips = null;

        public Dictionary<EQUIP_SLOT, List<EquipItemRecord>> ClassedEquips
        {
            get
            {
                if (_ClassedEquips == null)
                {
                    InitClassedEquips();
                }
                return _ClassedEquips;
            }
        }

        private void InitClassedEquips()
        {
            _ClassedEquips = new Dictionary<EQUIP_SLOT, List<EquipItemRecord>>();

            foreach (var record in Records.Values)
            {
                if (!_ClassedEquips.ContainsKey(record.Slot))
                {
                    _ClassedEquips.Add(record.Slot, new List<EquipItemRecord>());
                }
                _ClassedEquips[record.Slot].Add(record);
            }
        }

    }

}