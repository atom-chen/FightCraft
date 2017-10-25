using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactBaseAttr : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Emitter\\Element\\" + _ImpactName);
        impactGO.transform.SetParent(skillMotion.transform);

        var bulletEmitterEle = impactGO.GetComponent<BulletEmitterElement>();
        bulletEmitterEle._Rate = _Rate;
        bulletEmitterEle._Damage = _Damage;
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        return "";
    }

    #region 

    public int _Rate;
    public int _Damage;
    public string _ImpactName;

    
    #endregion
}
