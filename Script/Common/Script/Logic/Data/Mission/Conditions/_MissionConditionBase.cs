using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionConditionBase
{
    public virtual void InitCondition(MissionItem missionData, List<string> conditionParam)
    { }

    public virtual bool IsConditionMet()
    {
        return true;
    }

    public virtual float GetConditionProcess()
    {
        return 0;
    }

    public virtual string GetConditionProcessText()
    {
        return "";
    }

    public virtual void ConditionGoto()
    {

    }
}
