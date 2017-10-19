using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffExEffect : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<float> args)
    {
        _SkillInput = skillInput;
        _ImpactName = "ExBuffImpact";
    }

    public override List<float> GetSkillImpactVal(SkillInfoItem skillInfo)
    {
        var valList = new List<float>();
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0] * 0.0001f);

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\" + roleMotion._MotionAnimPath + "\\" + _ImpactName);
        impactGO.transform.SetParent(skillMotion.transform);

    }

    #region 

    public string _ImpactName;

    #endregion
}
