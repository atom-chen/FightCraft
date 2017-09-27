using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactAccumulate : RoleAttrImpactBase
{

    public override void InitImpact(params float[] args)
    {
        _SkillInput = SkillInfo.GetSkillInputByClass((SKILL_CLASS)args[0]); ;
        _AccumulateTime = args[1];
        _DamageEnhance = args[2];
        _ImpactName = "Accumulate";
    }

    public override void FightCreateImpact(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\CommonImpact\\" + _ImpactName);
        impactGO.transform.SetParent(skillMotion.transform);

        var bulletEmitterEle = impactGO.GetComponent<ImpactAccumulate>();
        bulletEmitterEle._AccumulateTime = _AccumulateTime;
        bulletEmitterEle._AccumulateDamage = _DamageEnhance;
    }

    #region 

    public string _SkillInput;
    public float _AccumulateTime;
    public float _DamageEnhance;
    public string _ImpactName;
    
    #endregion
}
