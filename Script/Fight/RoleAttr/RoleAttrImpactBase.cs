using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactBase
{
    public string _SkillInput;

    public virtual void InitImpact(string skillInput, List<float> args)
    {

    }

    public virtual List<float> GetSkillImpactVal(SkillInfoItem skillInfo)
    {
        return new List<float>();
    }

    public virtual void AddData(RoleAttrImpactBase otherImpact)
    { }

    public virtual void ModifySkillBeforeInit(MotionManager roleMotion)
    {

    }

    public virtual void ModifySkillAfterInit(MotionManager roleMotion)
    {

    }
}
