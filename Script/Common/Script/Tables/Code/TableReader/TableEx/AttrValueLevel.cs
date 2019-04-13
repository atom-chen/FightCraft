using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class AttrValueLevelRecord : TableRecordBase
    {

    }

    public partial class AttrValueLevel : TableFileBase
    {

        public int GetBaseValue(int level)
        {
            var record = GetRecord(level.ToString());
            if (record != null)
            {
                return record.Values[0];
            }
            return 0;
        }

        public int GetSpValue(int level, int idx)
        {
            int tempLv = Mathf.Clamp(level, 1, Records.Count);
            var record = GetRecord(tempLv.ToString());
            if (record != null)
            {
                return record.Values[idx];
            }
            return 0;
        }

    }

}