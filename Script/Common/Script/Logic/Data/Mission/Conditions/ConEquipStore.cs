using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConEquipStore : MissionConditionBase
{
    private string _EquipID = "-1";
    private int _EquipQuality = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        _MissionItem.RefreshMissionState();
    }

    private int GetCollectEquipCnt()
    {
        return LegendaryData.Instance._LegendaryEquips.Count;
    }

    public override float GetConditionProcess()
    {
        return GetCollectEquipCnt() / (float)_MissionRecord.ConditionNum;
    }

    public override string GetConditionProcessText()
    {
        return GetCollectEquipCnt() + "/" + _MissionRecord.ConditionNum;
    }

    public override bool IsConditionMet()
    {
        if (GetCollectEquipCnt() >= _MissionRecord.ConditionNum)
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
