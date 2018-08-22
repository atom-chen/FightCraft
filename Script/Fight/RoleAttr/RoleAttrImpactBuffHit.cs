﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffHit : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
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

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        //if (!_SkillInput.Equals("-1") && !roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
        //    return;

        if (roleMotion._StateSkill._SkillMotions.ContainsKey("5"))
        {
            var skillMotion = roleMotion._StateSkill._SkillMotions["5"];
            _ImpactName = "BuffHit";
            _ImpactEffectName = "BuffHitEffect";
            var impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\" + roleMotion._MotionAnimPath + "\\" + _ImpactName);
            impactGO.transform.SetParent(skillMotion.transform);
            impactGO.transform.localPosition = Vector3.zero;
            impactGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
            impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\" + roleMotion._MotionAnimPath + "\\" + _ImpactEffectName);
            impactGO.transform.SetParent(skillMotion.transform);
            impactGO.transform.localPosition = Vector3.zero;
            impactGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (roleMotion._StateSkill._SkillMotions.ContainsKey("6"))
        {
            var skillMotion = roleMotion._StateSkill._SkillMotions["6"];
            _ImpactName = "DeBuffHit";
            _ImpactEffectName = "DeBuffHitEffect";
            var impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\" + roleMotion._MotionAnimPath + "\\" + _ImpactName);
            impactGO.transform.SetParent(skillMotion.transform);
            impactGO.transform.localPosition = Vector3.zero;
            impactGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
            impactGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion\\" + roleMotion._MotionAnimPath + "\\" + _ImpactEffectName);
            impactGO.transform.SetParent(skillMotion.transform);
            impactGO.transform.localPosition = Vector3.zero;
            impactGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }

    #region 

    public string _ImpactName;
    public string _ImpactEffectName;

    #endregion
}
