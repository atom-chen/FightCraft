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
        
    }

}