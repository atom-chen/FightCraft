using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class EquipItemRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int NameStrDict { get; set; }
        public int DescStrDict { get; set; }
        public EQUIP_SLOT Slot { get; set; }
        public EQUIP_CLASS EquipClass { get; set; }
        public List<AttrValueRecord> BaseAttrs { get; set; }
        public int LevelLimit { get; set; }
        public int ProfessionLimit { get; set; }
        public AttrValueRecord ExAttr { get; set; }
        public EquipItemRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            BaseAttrs = new List<AttrValueRecord>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(NameStrDict));
            recordStrList.Add(TableWriteBase.GetWriteStr(DescStrDict));
            recordStrList.Add(((int)Slot).ToString());
            recordStrList.Add(((int)EquipClass).ToString());
            foreach (var testTableItem in BaseAttrs)
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
            recordStrList.Add(TableWriteBase.GetWriteStr(LevelLimit));
            recordStrList.Add(TableWriteBase.GetWriteStr(ProfessionLimit));
            if (ExAttr != null)
            {
                recordStrList.Add(ExAttr.Id);
            }
            else
            {
                recordStrList.Add("");
            }

            return recordStrList.ToArray();
        }
    }

    public partial class EquipItem : TableFileBase
    {
        public Dictionary<string, EquipItemRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public EquipItemRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("EquipItem" + ": " + id, ex);
            }
        }

        public EquipItem(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, EquipItemRecord>();
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

                    EquipItemRecord record = new EquipItemRecord(data);
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
                pair.Value.NameStrDict = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.DescStrDict = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.Slot =  (EQUIP_SLOT)TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.EquipClass =  (EQUIP_CLASS)TableReadBase.ParseInt(pair.Value.ValueStr[6]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[7]))
                {
                    pair.Value.BaseAttrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[7]));
                }
                else
                {
                    pair.Value.BaseAttrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[8]))
                {
                    pair.Value.BaseAttrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[8]));
                }
                else
                {
                    pair.Value.BaseAttrs.Add(null);
                }
                pair.Value.LevelLimit = TableReadBase.ParseInt(pair.Value.ValueStr[9]);
                pair.Value.ProfessionLimit = TableReadBase.ParseInt(pair.Value.ValueStr[10]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[11]))
                {
                    pair.Value.ExAttr =  TableReader.AttrValue.GetRecord(pair.Value.ValueStr[11]);
                }
                else
                {
                    pair.Value.ExAttr = null;
                }
            }
        }
    }

}