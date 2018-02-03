using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class MonsterBaseRecord : TableRecordBase
    {
        private List<CommonItemRecord> _ValidSpDrops;
        public List<CommonItemRecord> ValidSpDrops
        {
            get
            {
                if (_ValidSpDrops == null)
                {
                    _ValidSpDrops = new List<CommonItemRecord>();
                    foreach (var spDrop in SpDrops)
                    {
                        if (spDrop != null)
                        {
                            _ValidSpDrops.Add(spDrop);
                        }
                    }
                }
                return _ValidSpDrops;
            }
        }
    }

    public partial class MonsterBase : TableFileBase
    {

        public MonsterBaseRecord GetGroupElite(MonsterBaseRecord monsterBase)
        {
            foreach (var record in Records)
            {
                if (monsterBase.MotionGroup == record.Value.MotionGroup
                    && record.Value.MotionType == MOTION_TYPE.Elite)
                {
                    return record.Value;
                }
            }

            return monsterBase;
        }

    }
}