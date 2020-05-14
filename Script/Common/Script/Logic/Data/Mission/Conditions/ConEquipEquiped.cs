using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class ConEquipEquiped : MissionConditionBase
{
    private int _EquipQuality = 0;
    private int _EquipNum = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _EquipQuality = int.Parse(missionRecord.ConditionParams[0]);
        _EquipNum = int.Parse(missionRecord.ConditionParams[1]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_PUT_ON, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        _MissionItem.RefreshMissionState();

    }

    public int GetEquipedQuality()
    {
        int equipedCnt = 0;;
        foreach(var equipItem in RoleData.SelectRole.EquipList)
        {
            if (equipItem == null || !equipItem.IsVolid())
            {
                continue;
            }
            if ((int)equipItem.EquipQuality >= _EquipQuality)
                ++equipedCnt;
        }

        return equipedCnt;
    }

    public override float GetConditionProcess()
    {
        return GetEquipedQuality() / (float)_MissionRecord.ConditionNum;
    }

    public override string GetConditionProcessText()
    {
        return GetEquipedQuality() + "/" + _MissionRecord.ConditionNum;
    }

    public override bool IsConditionMet()
    {
        if (GetEquipedQuality() >= _MissionRecord.ConditionNum)
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
