using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConUseSkill : MissionConditionBase
{
    public string _UseSkillInput;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        _UseSkillInput = missionRecord.ConditionParams[0];
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_MAIN_USE_SKILL, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var skillBase = (ObjMotionSkillBase)eventArgs["MainUseSkill"];

        if (!string.Equals(skillBase._ActInput, _UseSkillInput))
            return;

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
        if (_MissionItem.MissionProcessData >= _MissionRecord.ConditionNum)
        {
            return true;
        }
        return false;
    }

    public override string GetMissionDesc()
    {
        var roleSkill = SkillData.Instance.GetSkillByInput(_UseSkillInput);
        string skillName = Tables.StrDictionary.GetFormatStr(roleSkill.SkillRecord.NameStrDict);
        return Tables.StrDictionary.GetFormatStr(_MissionRecord.DescID.Id, skillName);
    }

    public override void ConditionGoto()
    {
        UIGemPack.ShowAsyn();
    }
}
