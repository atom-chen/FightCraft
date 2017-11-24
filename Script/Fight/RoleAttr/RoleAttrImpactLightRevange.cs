using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactLightRevange : RoleAttrImpactPassive
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);
        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord( args[0].ToString());

        _Damage = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[0]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[0] * args[1]);
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        var bulletScripts = buffGO.GetComponentsInChildren<BulletEmitterElement>();
        foreach (var bulletScript in bulletScripts)
        {
            bulletScript._Damage = _Damage;
        }
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    #region 

    public float _Damage;
    
    #endregion
}
