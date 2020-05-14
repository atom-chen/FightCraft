using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConRoleLevel : MissionConditionBase
{
    private int _IsRoleLv = 0;
    private int _TargetLv = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        _IsRoleLv = int.Parse(_MissionRecord.ConditionParams[0]);
        _TargetLv = int.Parse(_MissionRecord.ConditionParams[1]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        _MissionItem.RefreshMissionState();

    }

    public override float GetConditionProcess()
    {
        var roleCnt = GetRoleCnt();
        return roleCnt / (float)_MissionRecord.ConditionNum;
    }

    private int GetRoleCnt()
    {
        return RoleData.SelectRole.TotalLevel;
    }

    public override string GetConditionProcessText()
    {
        var roleCnt = GetRoleCnt();
        return roleCnt + "/" + _MissionRecord.ConditionNum;
    }

    public override bool IsConditionMet()
    {
        var roleCnt = GetRoleCnt();
        if (roleCnt >= _MissionRecord.ConditionNum)
        {
            return true;
        }
        return false;
    }

    public override void ConditionGoto()
    {
        UIStageSelect.ShowAsyn();
    }
}
