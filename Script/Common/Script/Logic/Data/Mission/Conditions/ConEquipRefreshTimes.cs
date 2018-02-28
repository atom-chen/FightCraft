using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConEquipRefreshTimes : MissionConditionBase
{
    private int _RefreshTimes = 0;
    private MissionItem _MissionItem;

    public override void InitCondition(MissionItem missionData, List<string> conditionParams)
    {
        _RefreshTimes = int.Parse( conditionParams[0]);
        _MissionItem = missionData;
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        ++_MissionItem.MissionProcessData;
        _MissionItem.SaveClass(true);
        if (RoleData.SelectRole._RoleLevel >= _RefreshTimes)
        {

        }
    }

    public override float GetConditionProcess()
    {
        return _MissionItem.MissionProcessData / (float)_RefreshTimes;
    }

    public override string GetConditionProcessText()
    {
        return _MissionItem.MissionProcessData + "/" + _RefreshTimes;
    }

    public override bool IsConditionMet()
    {
        if (_MissionItem.MissionProcessData >= _RefreshTimes)
        {
            return true;
        }
        return false;
    }

    public override void ConditionGoto()
    {
        UIEquipRefresh.ShowAsyn();
    }
}
