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

        var selectTargets = SelectTargetCommon.GetNearMotions(_ObjMotion, _SelectRange);

        if (selectTargets.Count > 0)
        {
            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_ObjMotion, selectTargets[0]);
            }
        }
    }
}
