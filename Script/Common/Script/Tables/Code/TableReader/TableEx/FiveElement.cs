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
        Dictionary<FIVE_ELEMENT, FiveElementRecord> _EleTypeDict;

        public FiveElementRecord GetFiveElementByType(FIVE_ELEMENT eleType)
        {
            if (_EleTypeDict == null)
            {
                _EleTypeDict = new Dictionary<FIVE_ELEMENT, FiveElementRecord>();
                foreach (var record in Records)
                {
                    _EleTypeDict.Add(record.Value.EvelemtType, record.Value);
                }
            }

            return _EleTypeDict[eleType];
        }
    }

}