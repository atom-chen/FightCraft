using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class SkillInfoRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int Profession { get; set; }
        public string SkillInput { get; set; }
        public string SkillType { get; set; }
        public string SkillAttr { get; set; }
        public int MaxLevel { get; set; }
        public List<int> EffectValue { get; set; }
        public float CostStep { get; set; }
        public int Pos { get; set; }
        public SkillInfoRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            EffectValue = new List<int>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Profession));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillInput));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillType));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillAttr));
            recordStrList.Add(TableWriteBase.GetWriteStr(MaxLevel));
            foreach (var testTableItem in EffectValue)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(CostStep));
            recordStrList.Add(TableWriteBase.GetWriteStr(Pos));

            return recordStrList.ToArray();
        }
    }

    public partial class SkillInfo : TableFileBase
    {
        public Dictionary<string, SkillInfoRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public SkillInfoRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("SkillInfo" + ": " + id, ex);
            }
        }

        public SkillInfo(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, SkillInfoRecord>();
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

                    SkillInfoRecord record = new SkillInfoRecord(data);
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
                pair.Value.Profession = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.SkillInput = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.SkillType = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.SkillAttr = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.MaxLevel = TableReadBase.ParseInt(pair.Value.ValueStr[7]);
                pair.Value.EffectValue.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.EffectValue.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.EffectValue.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.CostStep = TableReadBase.ParseFloat(pair.Value.ValueStr[11]);
                pair.Value.Pos = TableReadBase.ParseInt(pair.Value.ValueStr[12]);
            }
        }
    }

}