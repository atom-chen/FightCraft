using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactAccumulate : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _AccumulateTime = args[0];
        _DamageEnhance = args[1] * 0.0001f;
        _ImpactName = "Accumulate";
    }

    public override List<int> GetSkillImpactVal(SkillInfoItem skillInfo)
    {
        var valList = new List<int>();
        valList.Add(5);
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

        var bulletEmitterEle = impactGO.GetComponent<ImpactAccumulate>();
        bulletEmitterEle._AccumulateTime = _AccumulateTime;
        bulletEmitterEle._AccumulateDamage = _DamageEnhance;
    }

    #region 

    public float _AccumulateTime;
    public float _DamageEnhance;
    public string _ImpactName;
    
    #endregion
}
