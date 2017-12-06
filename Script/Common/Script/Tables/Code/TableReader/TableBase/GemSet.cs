using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemSetRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public List<GemTableRecord> Gems { get; set; }
        public List<int> BaseAttr { get; set; }
        public List<int> AttrStep { get; set; }
        public List<int> AttrStep2 { get; set; }
        public GemSetRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Gems = new List<GemTableRecord>();
            BaseAttr = new List<int>();
            AttrStep = new List<int>();
            AttrStep2 = new List<int>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            foreach (var testTableItem in Gems)
            {
                if (testTableItem != null)
                {
                    recordStrList.Add(testTableItem.Id);
                }
                else
                {
                    recordStrList.Add("");
                }
            }
            foreach (var testTableItem in BaseAttr)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in AttrStep)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in AttrStep2)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class GemSet : TableFileBase
    {
        public Dictionary<string, GemSetRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GemSetRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GemSet" + ": " + id, ex);
            }
        }

        public GemSet(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GemSetRecord>();
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

                    GemSetRecord record = new GemSetRecord(data);
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
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[3]))
                {
                    pair.Value.Gems.Add( TableReader.GemTable.GetRecord(pair.Value.ValueStr[3]));
                }
                else
                {
                    pair.Value.Gems.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[4]))
                {
                    pair.Value.Gems.Add( TableReader.GemTable.GetRecord(pair.Value.ValueStr[4]));
                }
                else
                {
                    pair.Value.Gems.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[5]))
                {
                    pair.Value.Gems.Add( TableReader.GemTable.GetRecord(pair.Value.ValueStr[5]));
                }
                else
                {
                    pair.Value.Gems.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[6]))
                {
                    pair.Value.Gems.Add( TableReader.GemTable.GetRecord(pair.Value.ValueStr[6]));
                }
                else
                {
                    pair.Value.Gems.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[7]))
                {
                    pair.Value.Gems.Add( TableReader.GemTable.GetRecord(pair.Value.ValueStr[7]));
                }
                else
                {
                    pair.Value.Gems.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[8]))
                {
                    pair.Value.Gems.Add( TableReader.GemTable.GetRecord(pair.Value.ValueStr[8]));
                }
                else
                {
                    pair.Value.Gems.Add(null);
                }
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[13]));
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[14]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[15]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[16]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[17]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[18]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[19]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[20]));
                pair.Value.AttrStep2.Add(TableReadBase.ParseInt(pair.Value.ValueStr[21]));
                pair.Value.AttrStep2.Add(TableReadBase.ParseInt(pair.Value.ValueStr[22]));
                pair.Value.AttrStep2.Add(TableReadBase.ParseInt(pair.Value.ValueStr[23]));
                pair.Value.AttrStep2.Add(TableReadBase.ParseInt(pair.Value.ValueStr[24]));
                pair.Value.AttrStep2.Add(TableReadBase.ParseInt(pair.Value.ValueStr[25]));
                pair.Value.AttrStep2.Add(TableReadBase.ParseInt(pair.Value.ValueStr[26]));
            }
        }
    }

}