using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactBase
{
    public virtual void InitImpact(params float[] args)
    {

    }

    public virtual void AddData(RoleAttrImpactBase otherImpact)
    { }

    public virtual void FightCreateImpact(MotionManager roleMotion)
    {

    }
}
