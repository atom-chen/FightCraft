using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class SummonSkillRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public ITEM_QUALITY Quality { get; set; }
        public MonsterBaseRecord MonsterBase { get; set; }
        public int AttrModelfy { get; set; }
        public List<int> StarExp { get; set; }
        public string SkillDesc { get; set; }
        public List<float> SkillRate { get; set; }
        public List<int> StageCostItems { get; set; }
        public List<int> StageCostCnt { get; set; }
        public List<int> Stage1AttrMax { get; set; }
        public List<int> Stage2AttrMax { get; set; }
        public List<int> Stage3AttrMax { get; set; }
        public List<int> Stage4AttrMax { get; set; }
        public List<int> Stage5AttrMax { get; set; }
        public SummonSkillRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            StarExp = new List<int>();
            SkillRate = new List<float>();
            StageCostItems = new List<int>();
            StageCostCnt = new List<int>();
            Stage1AttrMax = new List<int>();
            Stage2AttrMax = new List<int>();
            Stage3AttrMax = new List<int>();
            Stage4AttrMax = new List<int>();
            Stage5AttrMax = new List<int>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(((int)Quality).ToString());
            if (MonsterBase != null)
            {
                recordStrList.Add(MonsterBase.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(AttrModelfy));
            foreach (var testTableItem in StarExp)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillDesc));
            foreach (var testTableItem in SkillRate)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in StageCostItems)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in StageCostCnt)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in Stage1AttrMax)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in Stage2AttrMax)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in Stage3AttrMax)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in Stage4AttrMax)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in Stage5AttrMax)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class SummonSkill : TableFileBase
    {
        public Dictionary<string, SummonSkillRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public SummonSkillRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("SummonSkill" + ": " + id, ex);
            }
        }

        public SummonSkill(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, SummonSkillRecord>();
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

                    SummonSkillRecord record = new SummonSkillRecord(data);
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
                pair.Value.Icon = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.Quality =  (ITEM_QUALITY)TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[5]))
                {
                    pair.Value.MonsterBase =  TableReader.MonsterBase.GetRecord(pair.Value.ValueStr[5]);
                }
                else
                {
                    pair.Value.MonsterBase = null;
                }
                pair.Value.AttrModelfy = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
                pair.Value.StarExp.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.StarExp.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.StarExp.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.StarExp.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.StarExp.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.SkillDesc = TableReadBase.ParseString(pair.Value.ValueStr[12]);
                pair.Value.SkillRate.Add(TableReadBase.ParseFloat(pair.Value.ValueStr[13]));
                pair.Value.SkillRate.Add(TableReadBase.ParseFloat(pair.Value.ValueStr[14]));
                pair.Value.SkillRate.Add(TableReadBase.ParseFloat(pair.Value.ValueStr[15]));
                pair.Value.SkillRate.Add(TableReadBase.ParseFloat(pair.Value.ValueStr[16]));
                pair.Value.SkillRate.Add(TableReadBase.ParseFloat(pair.Value.ValueStr[17]));
                pair.Value.SkillRate.Add(TableReadBase.ParseFloat(pair.Value.ValueStr[18]));
                pair.Value.StageCostItems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[19]));
                pair.Value.StageCostItems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[20]));
                pair.Value.StageCostItems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[21]));
                pair.Value.StageCostCnt.Add(TableReadBase.ParseInt(pair.Value.ValueStr[22]));
                pair.Value.StageCostCnt.Add(TableReadBase.ParseInt(pair.Value.ValueStr[23]));
                pair.Value.StageCostCnt.Add(TableReadBase.ParseInt(pair.Value.ValueStr[24]));
                pair.Value.StageCostCnt.Add(TableReadBase.ParseInt(pair.Value.ValueStr[25]));
                pair.Value.Stage1AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[26]));
                pair.Value.Stage1AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[27]));
                pair.Value.Stage1AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[28]));
                pair.Value.Stage1AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[29]));
                pair.Value.Stage1AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[30]));
                pair.Value.Stage2AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[31]));
                pair.Value.Stage2AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[32]));
                pair.Value.Stage2AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[33]));
                pair.Value.Stage2AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[34]));
                pair.Value.Stage2AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[35]));
                pair.Value.Stage3AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[36]));
                pair.Value.Stage3AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[37]));
                pair.Value.Stage3AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[38]));
                pair.Value.Stage3AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[39]));
                pair.Value.Stage3AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[40]));
                pair.Value.Stage4AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[41]));
                pair.Value.Stage4AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[42]));
                pair.Value.Stage4AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[43]));
                pair.Value.Stage4AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[44]));
                pair.Value.Stage4AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[45]));
                pair.Value.Stage5AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[46]));
                pair.Value.Stage5AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[47]));
                pair.Value.Stage5AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[48]));
                pair.Value.Stage5AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[49]));
                pair.Value.Stage5AttrMax.Add(TableReadBase.ParseInt(pair.Value.ValueStr[50]));
            }
        }
    }

}