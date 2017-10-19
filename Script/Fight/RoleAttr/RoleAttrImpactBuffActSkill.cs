using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffActSkill : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<float> args)
    {
        _SkillInput = skillInput;
        _ValueModify = (int)args[0];
    }

    public override List<float> GetSkillImpactVal(SkillInfoItem skillInfo)
    {
        var valList = new List<float>();
        valList.Add(skillInfo.SkillActureLevel);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        (skillMotion as ObjMotionSkillBuff).IsCanActAfterBuff = true;
    }

    #region 

    public int _ValueModify;
    
    #endregion
}
