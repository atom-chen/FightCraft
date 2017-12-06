using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemTableRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int Class { get; set; }
        public int BaseAttr { get; set; }
        public List<int> AttrStep { get; set; }
        public int LevelUpParam { get; set; }
        public GemTableRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            AttrStep = new List<int>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Class));
            recordStrList.Add(TableWriteBase.GetWriteStr(BaseAttr));
            foreach (var testTableItem in AttrStep)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(LevelUpParam));

            return recordStrList.ToArray();
        }
    }

    public partial class GemTable : TableFileBase
    {
        public Dictionary<string, GemTableRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GemTableRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GemTable" + ": " + id, ex);
            }
        }

        public GemTable(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GemTableRecord>();
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

                    GemTableRecord record = new GemTableRecord(data);
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
                pair.Value.Class = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.BaseAttr = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[5]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[6]));
                pair.Value.AttrStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.LevelUpParam = TableReadBase.ParseInt(pair.Value.ValueStr[8]);
            }
        }
    }

}