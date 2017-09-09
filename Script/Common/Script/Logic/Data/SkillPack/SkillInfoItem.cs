using UnityEngine;
using System.Collections;



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

    private Tables.SkillInfoRecord _SkillRecord;
    public Tables.SkillInfoRecord SkillRecord
    {
        get
        {
            if (_SkillRecord == null)
            {
                _SkillRecord = Tables.TableReader.SkillInfo.GetRecord(_SkillID);
            }
            return _SkillRecord;
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

