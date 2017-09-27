using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactExAttack : RoleAttrImpactBase
{

    public override void InitImpact(params float[] args)
    {
        _SkillInput = SkillInfo.GetSkillInputByClass((SKILL_CLASS)args[0]); ;
        _AttackTimes = (int)args[1];
        _Damage = args[2];
        _ImpactName = "ExAttack";
    }

    public override void FightCreateImpact(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\CommonImpact\\" + _ImpactName);
        impactGO.transform.SetParent(skillMotion.transform);

        var bulletEmitterEle = impactGO.GetComponent<ImpactExAttack>();
        bulletEmitterEle._AttackTimes = _AttackTimes;
        bulletEmitterEle._Damage = _Damage;
    }

    #region 

    public string _SkillInput;
    public int _AttackTimes;
    public float _Damage;
    public string _ImpactName;
    
    #endregion
}
