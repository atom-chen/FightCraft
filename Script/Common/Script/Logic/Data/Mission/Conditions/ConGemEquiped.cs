using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConGemEquiped : MissionConditionBase
{
    private int _EquipGemLv = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        _EquipGemLv = int.Parse(missionRecord.ConditionParams[0]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_ON, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        
        _MissionItem.RefreshMissionState();
    }

    public int GetEquipedGem()
    {
        int equipedGemCnt = 0;
        foreach (var gemData in GemData.Instance.EquipedGemDatas)
        {
            if (gemData == null || !gemData.IsVolid())
                continue;

            if (gemData.Level < _EquipGemLv)
                continue;

            ++equipedGemCnt;
        }

        return equipedGemCnt;
    }

    public override float GetConditionProcess()
    {
        return GetEquipedGem() / (float)_MissionRecord.ConditionNum;
    }

    public override string GetConditionProcessText()
    {
        return GetEquipedGem() + "/" + _MissionRecord.ConditionNum;
    }

    public override bool IsConditionMet()
    {
        if (GetEquipedGem() >= _MissionRecord.ConditionNum)
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
