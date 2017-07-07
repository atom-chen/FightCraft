using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectLastHit : SelectBase
{
    
    public override void ColliderStart()
    {
        foreach (var skillMotion in _SkillMotion.ActingSkill._SkillHitMotions)
        {

            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_SkillMotion, skillMotion);
            }

            if (_SkillMotion._IsRoleHit)
            {
                GlobalEffect.Instance.Pause(_SkillMotion._RoleHitTime);
            }

        }
    }

    public override void ColliderFinish()
    {
        _SkillMotion.ActingSkill._SkillHitMotions.Clear();
    }

}
