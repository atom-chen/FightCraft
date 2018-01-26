﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class StageInfoRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public string FightLogicPath { get; set; }
        public List<string> ScenePath { get; set; }
        public List<Vector3> CameraOffset { get; set; }
        public List<Vector3> CameraLimit { get; set; }
        public StageInfoRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            ScenePath = new List<string>();
            CameraOffset = new List<Vector3>();
            CameraLimit = new List<Vector3>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(FightLogicPath));
            foreach (var testTableItem in ScenePath)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in CameraOffset)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in CameraLimit)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class StageInfo : TableFileBase
    {
        public Dictionary<string, StageInfoRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public StageInfoRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("StageInfo" + ": " + id, ex);
            }
        }

        public StageInfo(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, StageInfoRecord>();
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

                    StageInfoRecord record = new StageInfoRecord(data);
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
                pair.Value.FightLogicPath = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.ScenePath.Add(TableReadBase.ParseString(pair.Value.ValueStr[4]));
                pair.Value.ScenePath.Add(TableReadBase.ParseString(pair.Value.ValueStr[5]));
                pair.Value.ScenePath.Add(TableReadBase.ParseString(pair.Value.ValueStr[6]));
                pair.Value.CameraOffset.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[7]));
                pair.Value.CameraOffset.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[8]));
                pair.Value.CameraOffset.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[9]));
                pair.Value.CameraLimit.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[10]));
                pair.Value.CameraLimit.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[11]));
                pair.Value.CameraLimit.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[12]));
            }
        }
    }

}