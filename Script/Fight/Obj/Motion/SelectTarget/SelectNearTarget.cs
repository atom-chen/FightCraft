using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectNearTarget : SelectBase
{
    public float _SelectRange = 8;

    public override void ColliderStart()
    {
        //base.ColliderStart();
        if (_SkillMotion == null)
            return;

        var selectTargets = SelectTargetCommon.GetNearMotions(_SkillMotion, _SelectRange);

        if (selectTargets.Count > 0)
        {
            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_SkillMotion, selectTargets[0]);
            }
        }
    }
}
