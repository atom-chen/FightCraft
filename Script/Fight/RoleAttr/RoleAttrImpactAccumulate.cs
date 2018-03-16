using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactAccumulate : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _AccumulateTime = GameDataValue.ConfigIntToFloat(args[0]);
        _DamageEnhance = GameDataValue.ConfigIntToFloat(args[1]);
        _ImpactName = "Accumulate";
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillRecord.EffectValue[0]);
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[1]);

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!_SkillInput.Equals("-1"))
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
        else
        {
            foreach (var skillMotion in roleMotion._StateSkill._SkillMotions.Values)
            {
                if (!skillMotion._ActInput.Equals("1")
                    && !skillMotion._ActInput.Equals("2")
                    && !skillMotion._ActInput.Equals("3"))
                    continue;

                var impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\CommonImpact\\" + _ImpactName);
                impactGO.transform.SetParent(skillMotion.transform);

                var bulletEmitterEle = impactGO.GetComponent<ImpactAccumulate>();
                bulletEmitterEle._AccumulateTime = _AccumulateTime;
                bulletEmitterEle._AccumulateDamage = _DamageEnhance;
            }
        }
    }

    #region 

    public float _AccumulateTime;
    public float _DamageEnhance;
    public string _ImpactName;
    
    #endregion
}
