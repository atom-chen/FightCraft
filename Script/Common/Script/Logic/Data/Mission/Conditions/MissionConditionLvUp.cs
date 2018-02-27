using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionConditionLvUp : MissionConditionBase
{
    private int _TargetLv = 0;

    public override void InitCondition(MissionProcessData missionData, List<string> conditionParams)
    {
        _TargetLv = int.Parse(conditionParams[0]);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        if (RoleData.SelectRole._RoleLevel == _TargetLv)
        {

        }
    }

    public override bool IsConditionMet()
    {
        if (RoleData.SelectRole._RoleLevel >= _TargetLv)
        {
            return true;
        }
        return false;
    }
}
