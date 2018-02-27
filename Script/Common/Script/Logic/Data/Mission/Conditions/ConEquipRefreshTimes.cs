using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConEquipRefreshTimes : MissionConditionBase
{
    private int _RefreshTimes = 0;
    private MissionProcessData _MissionProcessData;

    public override void InitCondition(MissionProcessData missionData, List<string> conditionParams)
    {
        _RefreshTimes = int.Parse( conditionParams[0]);
        _MissionProcessData = missionData;
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        ++_MissionProcessData.MissionProcessInt;
        if (RoleData.SelectRole._RoleLevel >= _RefreshTimes)
        {

        }
    }

    public override float GetConditionProcess()
    {
        return _MissionProcessData.MissionProcessInt / (float)_RefreshTimes;
    }

    public override string GetConditionProcessText()
    {
        return _MissionProcessData.MissionProcessInt + "/" + _RefreshTimes;
    }

    public override bool IsConditionMet()
    {
        if (_MissionProcessData.MissionProcessInt >= _RefreshTimes)
        {
            return true;
        }
        return false;
    }
}
