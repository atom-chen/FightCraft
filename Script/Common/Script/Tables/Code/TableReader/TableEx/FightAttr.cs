using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FightAttrRecord : TableRecordBase
    {

    }

    public partial class FightAttr : TableFileBase
    {

        public List<FightAttrRecord> GetAttrLevelRequire(int level)
        {
            List<FightAttrRecord> levelRecord = new List<FightAttrRecord>();
            foreach (var record in Records)
            {
                if (record.Value.LevelMin > 0 && record.Value.LevelMin > level)
                    continue;

                if (record.Value.LevelMax > 0 && record.Value.LevelMax < level)
                    continue;

                levelRecord.Add(record.Value);
            }

            return levelRecord;
        }
        
    }

}