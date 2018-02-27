using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class MissionRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public string ConditionScript { get; set; }
        public List<string> ConditionParams { get; set; }
        public int ConditionNum { get; set; }
        public int HardStar { get; set; }
        public MissionRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            ConditionParams = new List<string>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(ConditionScript));
            foreach (var testTableItem in ConditionParams)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(ConditionNum));
            recordStrList.Add(TableWriteBase.GetWriteStr(HardStar));

            return recordStrList.ToArray();
        }
    }

    public partial class Mission : TableFileBase
    {
        public Dictionary<string, MissionRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public MissionRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("Mission" + ": " + id, ex);
            }
        }

        public Mission(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, MissionRecord>();
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

                    MissionRecord record = new MissionRecord(data);
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
                pair.Value.ConditionScript = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.ConditionParams.Add(TableReadBase.ParseString(pair.Value.ValueStr[4]));
                pair.Value.ConditionParams.Add(TableReadBase.ParseString(pair.Value.ValueStr[5]));
                pair.Value.ConditionParams.Add(TableReadBase.ParseString(pair.Value.ValueStr[6]));
                pair.Value.ConditionNum = TableReadBase.ParseInt(pair.Value.ValueStr[7]);
                pair.Value.HardStar = TableReadBase.ParseInt(pair.Value.ValueStr[8]);
            }
        }
    }

}