using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactSkillRange : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _RangeSModify = (int)args[0] * 0.0001f;
    }

    public override List<int> GetSkillImpactVal(SkillInfoItem skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        skillMotion.ModifyColliderRange(_RangeSModify);
        skillMotion.SetEffectSize(1 + _RangeSModify);
    }

    #region 

    public float _RangeSModify;
    
    #endregion
}
