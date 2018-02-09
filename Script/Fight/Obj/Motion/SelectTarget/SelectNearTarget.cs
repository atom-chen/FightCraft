using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectNearTarget : SelectBase
{
    public float _SelectRange = 8;

    public override void ColliderStart()
    {
        //base.ColliderStart();
        if (_ObjMotion == null)
            return;

        var selectTarget = SelectTargetCommon.GetNearMotion(_ObjMotion, transform.position, null, SelectTargetType.Enemy);

        foreach (var impact in _ImpactList)
        {
            impact.ActImpact(_ObjMotion, selectTarget);
        }

    }
}
