using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConGemActSuit : MissionConditionBase
{
    private int _IsRoleLv = 0;
    private int _TargetLv = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_GEM_ACT_SUIT, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        _MissionItem.RefreshMissionState();

    }

    public override float GetConditionProcess()
    {
        return GemSuit.Instance.ActLevel / (float)_MissionRecord.ConditionNum;
    }

    public override string GetConditionProcessText()
    {
        return GemSuit.Instance.ActLevel + "/" + _MissionRecord.ConditionNum;
    }

    public override bool IsConditionMet()
    {
        if (GemSuit.Instance.ActLevel >= _MissionRecord.ConditionNum)
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
