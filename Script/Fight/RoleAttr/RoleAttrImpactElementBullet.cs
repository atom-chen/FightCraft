using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactElementBullet : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord( args[0].ToString());
        _ImpactName = legendaryEquip.BulletName;
        _SkillInput = legendaryEquip.SkillInput;
        _Damage = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[0]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[0] * args[1]);
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Emitter\\Element\\" + _ImpactName);
        impactGO.transform.SetParent(skillMotion.transform);

        var bulletEmitterEle = impactGO.GetComponentsInChildren<BulletEmitterElement>();
        foreach (var bulletEmitter in bulletEmitterEle)
        {
            bulletEmitter._Damage = _Damage;
        }
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord(attrParams[0].ToString());
        return string.Format(legendaryEquip.Desc, legendaryEquip.ImpactValues);
    }

    #region 

    public float _Damage;
    public string _ImpactName;

    
    #endregion
}
