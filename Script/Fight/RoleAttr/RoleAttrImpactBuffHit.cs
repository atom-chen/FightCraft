using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffHit : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<float> args)
    {
        _SkillInput = skillInput;
        if (skillInput == "5")
        {
            _ImpactName = "BuffHit";
            _ImpactEffectName = "BuffHitEffect";
        }
        else
        {
            _ImpactName = "DeBuffHit";
            _ImpactEffectName = "DeBuffHitEffect";
        }
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
        impactGO.transform.localPosition = Vector3.zero;
        impactGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
        impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\" + roleMotion._MotionAnimPath + "\\" + _ImpactEffectName);
        impactGO.transform.SetParent(skillMotion.transform);
        impactGO.transform.localPosition = Vector3.zero;
        impactGO.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }

    #region 

    public string _ImpactName;
    public string _ImpactEffectName;

    #endregion
}
