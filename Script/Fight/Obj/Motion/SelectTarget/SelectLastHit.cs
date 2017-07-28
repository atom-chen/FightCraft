using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectLastHit : SelectBase
{
    
    public override void ColliderStart()
    {
        foreach (var skillMotion in _ObjMotion.ActingSkill._SkillHitMotions)
        {

            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_ObjMotion, skillMotion);
            }

            if (_ObjMotion._IsRoleHit)
            {
                GlobalEffect.Instance.Pause(_ObjMotion._RoleHitTime);
            }

        }
    }

    public override void ColliderFinish()
    {
        _ObjMotion.ActingSkill._SkillHitMotions.Clear();
    }

}
