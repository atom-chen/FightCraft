using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class ConShopBuy : MissionConditionBase
{
    private string _MonsterID = "-1";

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _MonsterID = missionRecord.ConditionParams[0];

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SHOP_BUY, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {

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
