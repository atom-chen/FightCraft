using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class LegendaryEquipRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public EquipItemRecord EquipItem { get; set; }
        public string AttrImpact { get; set; }
        public string BulletName { get; set; }
        public string SkillInput { get; set; }
        public List<int> ImpactValues { get; set; }
        public List<int> ImpactValueIncs { get; set; }
        public LegendaryEquipRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            ImpactValues = new List<int>();
            ImpactValueIncs = new List<int>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            if (EquipItem != null)
            {
                recordStrList.Add(EquipItem.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(AttrImpact));
            recordStrList.Add(TableWriteBase.GetWriteStr(BulletName));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillInput));
            foreach (var testTableItem in ImpactValues)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in ImpactValueIncs)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class LegendaryEquip : TableFileBase
    {
        public Dictionary<string, LegendaryEquipRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public LegendaryEquipRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("LegendaryEquip" + ": " + id, ex);
            }
        }

        public LegendaryEquip(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, LegendaryEquipRecord>();
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

                    LegendaryEquipRecord record = new LegendaryEquipRecord(data);
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
                    pair.Value.EquipItem =  TableReader.EquipItem.GetRecord(pair.Value.ValueStr[3]);
                }
                else
                {
                    pair.Value.EquipItem = null;
                }
                pair.Value.AttrImpact = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.BulletName = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.SkillInput = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.ImpactValues.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.ImpactValues.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.ImpactValues.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.ImpactValueIncs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.ImpactValueIncs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.ImpactValueIncs.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
            }
        }
    }

}