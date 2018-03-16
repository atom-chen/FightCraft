using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffRate : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ValueModify = args[0];
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillRecord.EffectValue[0]);
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[1]);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactBuff = skillMotion.GetComponentInChildren<ImpactBuffAttrAdd>(true);
        //for (int i = 0; i < impactBuffs.Length; ++i)
        {
            impactBuff._AddValue = _ValueModify;
        }
    }

    #region 

    public float _ValueModify;
    
    #endregion
}
