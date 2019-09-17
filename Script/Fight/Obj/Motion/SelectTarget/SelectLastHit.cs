using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectLastHit : SelectBase
{

    public int _HittedAudio;
    public bool _ClearLastSelect = true;
    
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

        if (_ObjMotion.ActingSkill._SkillHitMotions.Count > 0)
        {
            if (_HittedAudio > 0)
            {
                _ObjMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_HittedAudio]);
            }
        }
    }

    public override void ColliderFinish()
    {
        _ObjMotion.ActingSkill._SkillHitMotions.Clear();
    }

}
