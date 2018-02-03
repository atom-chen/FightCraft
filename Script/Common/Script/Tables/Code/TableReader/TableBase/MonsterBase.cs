using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class MonsterBaseRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int MotionGroup { get; set; }
        public string MotionPath { get; set; }
        public string AnimPath { get; set; }
        public string ModelPath { get; set; }
        public int ElementType { get; set; }
        public MOTION_TYPE MotionType { get; set; }
        public List<int> BaseAttr { get; set; }
        public List<CommonItemRecord> SpDrops { get; set; }
        public MonsterBaseRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            BaseAttr = new List<int>();
            SpDrops = new List<CommonItemRecord>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(MotionGroup));
            recordStrList.Add(TableWriteBase.GetWriteStr(MotionPath));
            recordStrList.Add(TableWriteBase.GetWriteStr(AnimPath));
            recordStrList.Add(TableWriteBase.GetWriteStr(ModelPath));
            recordStrList.Add(TableWriteBase.GetWriteStr(ElementType));
            recordStrList.Add(((int)MotionType).ToString());
            foreach (var testTableItem in BaseAttr)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in SpDrops)
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

            return recordStrList.ToArray();
        }
    }

    public partial class MonsterBase : TableFileBase
    {
        public Dictionary<string, MonsterBaseRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public MonsterBaseRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("MonsterBase" + ": " + id, ex);
            }
        }

        public MonsterBase(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, MonsterBaseRecord>();
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

                    MonsterBaseRecord record = new MonsterBaseRecord(data);
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
                pair.Value.MotionGroup = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.MotionPath = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.AnimPath = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.ModelPath = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.ElementType = TableReadBase.ParseInt(pair.Value.ValueStr[7]);
                pair.Value.MotionType =  (MOTION_TYPE)TableReadBase.ParseInt(pair.Value.ValueStr[8]);
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.BaseAttr.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[12]))
                {
                    pair.Value.SpDrops.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[12]));
                }
                else
                {
                    pair.Value.SpDrops.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[13]))
                {
                    pair.Value.SpDrops.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[13]));
                }
                else
                {
                    pair.Value.SpDrops.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[14]))
                {
                    pair.Value.SpDrops.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[14]));
                }
                else
                {
                    pair.Value.SpDrops.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[15]))
                {
                    pair.Value.SpDrops.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[15]));
                }
                else
                {
                    pair.Value.SpDrops.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[16]))
                {
                    pair.Value.SpDrops.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[16]));
                }
                else
                {
                    pair.Value.SpDrops.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[17]))
                {
                    pair.Value.SpDrops.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[17]));
                }
                else
                {
                    pair.Value.SpDrops.Add(null);
                }
            }
        }
    }

}