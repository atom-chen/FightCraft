using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConSkillLvUp : MissionConditionBase
{
    public string _SkillType;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        _SkillType = missionRecord.ConditionParams[0];

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_LEVELUP_SKILL, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var skillRecord = (Tables.SkillInfoRecord)eventArgs["SkillRecord"];
        if (skillRecord.SkillType != _SkillType)
        {
            return;
        }

        ++_MissionItem.MissionProcessData;
        _MissionItem.SaveClass(true);

        _MissionItem.RefreshMissionState();
    }

    public override float GetConditionProcess()
    {
        return _MissionItem.MissionProcessData / (float)_MissionRecord.ConditionNum;
    }

    public override string GetConditionProcessText()
    {
        return _MissionItem.MissionProcessData + "/" + _MissionRecord.ConditionNum;
    }

    public override bool IsConditionMet()
    {
        if (SkillData.Instance.IsAllSkillMax())
            return true;

        if (_MissionItem.MissionProcessData >= _MissionRecord.ConditionNum)
        {
            return true;
        }
        return false;
    }

    public override void ConditionGoto()
    {
        UIGemPack.ShowAsyn();
    }
}
