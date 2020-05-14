using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConGemLvUp : MissionConditionBase
{
    private int _GemLevel = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _GemLevel = int.Parse(missionRecord.ConditionParams[0]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_COMBINE, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var gemItem = (ItemGem)eventArgs["ItemGem"];
        if (gemItem == null || !gemItem.IsVolid())
        {
            return;
        }
        if (gemItem.Level < _GemLevel)
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
        UIGemPack.ShowAsyn();
    }
}
