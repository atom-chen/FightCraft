using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FiveElementRecord : TableRecordBase
    {

    }

    public partial class FiveElement : TableFileBase
    {
        List<FiveElementRecord> _RecordList;

        public FiveElementRecord GetFiveElementByIndex(int idx)
        {
            if (_RecordList == null)
            {
                _RecordList = new List<FiveElementRecord>();
                foreach (var record in Records)
                {
                    _RecordList.Add(record.Value);
                }
            }

            return _RecordList[idx];
        }
    }

}