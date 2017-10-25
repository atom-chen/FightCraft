using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffActSkill : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ValueModify = args[0];
    }

    public override List<int> GetSkillImpactVal(SkillInfoItem skillInfo)
    {
        var valList = new List<int>();
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
