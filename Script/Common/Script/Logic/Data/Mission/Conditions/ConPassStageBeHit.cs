using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConPassStageBeHit : MissionConditionBase
{
    private int _StageIdx = 0;
    private int _BeHitTimes = 0;
    private int _PassCnt = 0;
    private float _LastHP = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _BeHitTimes = int.Parse(missionRecord.ConditionParams[0]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var stageType = (Tables.STAGE_TYPE)eventArgs["StageType"];
        if (stageType != Tables.STAGE_TYPE.NORMAL)
        {
            return;
        }

        if (_BeHitTimes > 0 && _BeHitTimes < FightManager.Instance.BeHitTimes)
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
