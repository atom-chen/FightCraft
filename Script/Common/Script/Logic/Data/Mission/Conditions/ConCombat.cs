using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class ConCombat : MissionConditionBase
{
    private int _CombatNum = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _CombatNum = int.Parse( missionRecord.ConditionParams[0]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_COMBAT, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var combatNum = (int)eventArgs["Combat"];

        if (combatNum < _MissionItem.MissionProcessData)
            return;

        _MissionItem.MissionProcessData = combatNum;
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

    public override void ConditionGoto()
    {
        UIStageSelect.ShowAsyn();
    }
}
