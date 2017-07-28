using UnityEngine;
using System.Collections;

namespace GameLogic
{
    public class SkillInfoItem
    {
        [SaveField(1)]
        public string _SkillID;

        [SaveField(2)]
        public int _SkillLevel;

        public int _SkillExLevel = 0;

        public int SkillActureLevel
        {
            get
            {
                return _SkillExLevel + _SkillLevel;
            }
        }

        public SkillInfoItem()
        {
            _SkillID = "-1";
            _SkillLevel = 0;
        }

        public SkillInfoItem(string id)
        {
            _SkillID = id;
            _SkillLevel = 1;
        }
    }
}
