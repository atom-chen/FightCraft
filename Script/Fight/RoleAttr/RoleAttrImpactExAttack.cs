using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactExAttack : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<float> args)
    {
        _SkillInput = skillInput;
        _AttackTimes = (int)args[0];
        _Damage = args[1];
        _ImpactName = "ExAttack";
    }

    public override List<float> GetSkillImpactVal(SkillInfoItem skillInfo)
    {
        var valList = new List<float>();
        valList.Add(1.0f);
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
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

    public int _AttackTimes;
    public float _Damage;
    public string _ImpactName;
    
    #endregion
}
