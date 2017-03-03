using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectSelf : SelectBase
{

    public override void ColliderStart()
    {
        //base.ColliderStart();

        if (_SkillMotion != null)
        {
            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_SkillMotion, _SkillMotion);
            }
        }
    }
}
