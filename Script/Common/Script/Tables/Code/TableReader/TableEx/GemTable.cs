using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemTableRecord : TableRecordBase
    {
        
    }

    public partial class GemTable : TableFileBase
    {

        public GemTableRecord GetGemRecordByClass(int classType, int level)
        {
            int recordID = classType + level - 1;
            return GetRecord(recordID.ToString());
        }

    }
}