using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveThorn : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord(args[0].ToString());
        _Damage = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[0]) - GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[0] * args[1]);
        _Damage = Mathf.Min(_Damage, 1);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        var buffs = buffGO.GetComponents<ImpactDamage>();
        foreach (var buff in buffs)
        {
            buff._DamageRate = _Damage;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _Damage;
    
    #endregion
}
