using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class ConKillBossInTime : MissionConditionBase
{
    private float _FightTime = 0;

    private float _StartTime = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _FightTime = float.Parse(missionRecord.ConditionParams[0]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_MEET_BOSS, MeetBossEventDelegate);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, EventDelegate);
    }

    private void MeetBossEventDelegate(object go, Hashtable eventArgs)
    {
        _StartTime = TimeManager.Instance.FightTime;


    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var monsterMotion = (MotionManager)eventArgs["MonsterInfo"];

        if (monsterMotion.RoleAttrManager.MotionType != MOTION_TYPE.Hero)
            return;

        float fightTime = TimeManager.Instance.FightTime - _StartTime;
        if (fightTime > _FightTime)
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

    public override void ConditionGoto()
    {
        UIStageSelect.ShowAsyn();
    }
}
