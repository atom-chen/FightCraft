using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class ConMeetBoss : MissionConditionBase
{
    private string _MonsterID = "-1";
    private float _LastHP = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _MonsterID = missionRecord.ConditionParams[0];
        _LastHP = GameDataValue.ConfigIntToFloat(int.Parse(missionRecord.ConditionParams[1]));

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_MEET_BOSS, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var monsterMotion = (MotionManager)eventArgs["MonsterInfo"];

        if (!string.Equals(_MonsterID, "-1") && monsterMotion.MonsterBase.Id != _MonsterID)
            return;

        if (_LastHP > GetPlayerLastHP())
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
