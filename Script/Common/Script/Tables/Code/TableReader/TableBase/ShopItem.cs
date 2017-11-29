using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class ShopItemRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int PriceBuy { get; set; }
        public int PriceSell { get; set; }
        public int ShopRate { get; set; }
        public int ShopMaxNum { get; set; }
        public ShopItemRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(PriceBuy));
            recordStrList.Add(TableWriteBase.GetWriteStr(PriceSell));
            recordStrList.Add(TableWriteBase.GetWriteStr(ShopRate));
            recordStrList.Add(TableWriteBase.GetWriteStr(ShopMaxNum));

            return recordStrList.ToArray();
        }
    }

    public partial class ShopItem : TableFileBase
    {
        public Dictionary<string, ShopItemRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public ShopItemRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("ShopItem" + ": " + id, ex);
            }
        }

        public ShopItem(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, ShopItemRecord>();
            if(isPath)
            {
                string[] lines = File.ReadAllLines(pathOrContent);
                lines[0] = lines[0].Replace("\r\n", "\n");
                ParserTableStr(string.Join("\n", lines));
            }
            else
            {
                ParserTableStr(pathOrContent.Replace("\r\n", "\n"));
            }
        }

        private void ParserTableStr(string content)
        {
            StringReader rdr = new StringReader(content);
            using (var reader = new CsvReader(rdr))
            {
                HeaderRecord header = reader.ReadHeaderRecord();
                while (reader.HasMoreRecords)
                {
                    DataRecord data = reader.ReadDataRecord();
                    if (data[0].StartsWith("#"))
                        continue;

                    ShopItemRecord record = new ShopItemRecord(data);
                    Records.Add(record.Id, record);
                }
            }
        }

        public void CoverTableContent()
        {
            foreach (var pair in Records)
            {
                pair.Value.Name = TableReadBase.ParseString(pair.Value.ValueStr[1]);
                pair.Value.Desc = TableReadBase.ParseString(pair.Value.ValueStr[2]);
                pair.Value.PriceBuy = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.PriceSell = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.ShopRate = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.ShopMaxNum = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
            }
        }
    }

}