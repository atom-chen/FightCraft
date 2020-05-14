using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConPassKMonsterInGold : MissionConditionBase
{

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, EventDelegate);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ENTER_STAGE, EnterStageEventDelegate);
    }

    private void EnterStageEventDelegate(object go, Hashtable eventArgs)
    {
        if (ActData.Instance._StageMode != Tables.STAGE_TYPE.ACT_GOLD)
            return;

        _MissionItem.MissionProcessData = 0;
        _MissionItem.SaveClass(true);

        _MissionItem.RefreshMissionState();
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        if (ActData.Instance._StageMode != Tables.STAGE_TYPE.ACT_GOLD)
            return;

        ++_MissionItem.MissionProcessData;
        _MissionItem.SaveClass(true);

        _MissionItem.RefreshMissionState();
    }

    private float GetPlayerLastHP()
    {
        return FightManager.Instance.MainChatMotion.RoleAttrManager.HPPersent;
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
