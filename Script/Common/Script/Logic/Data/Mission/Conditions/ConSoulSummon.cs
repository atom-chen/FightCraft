using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConSoulSummon : MissionConditionBase
{
    private int _LotteryType = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _LotteryType = int.Parse(missionRecord.ConditionParams[0]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SOUL_LOTTERY, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var lotteryType = (int)eventArgs["LotteryType"];
        if (lotteryType != _LotteryType)
        {
            return;
        }
        
        var lotteryResult = (SummonSkillData.LotteryResult)eventArgs["LotteryResult"];


        _MissionItem.MissionProcessData += lotteryResult._SummonData.Count;
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
        UIGemPack.ShowAsyn();
    }
}
