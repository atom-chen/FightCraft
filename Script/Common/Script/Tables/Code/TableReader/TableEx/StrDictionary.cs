using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class StrDictionaryRecord : TableRecordBase
    {

    }

    public partial class StrDictionary : TableFileBase
    {

        public static string GetFormatStr(int idx, params object[] param)
        {
            return GetFormatStr(idx.ToString());
        }

        public static string GetFormatStr(string idx, params object[] param)
        {
            var strRecord = TableReader.StrDictionary.GetRecord(idx.ToString());
            if (strRecord == null)
            {
                return "StrDictionary Error:" + idx;
            }
            return string.Format(strRecord.Value[GameCore.Instance._StrVersion], param);
        }
    }

}