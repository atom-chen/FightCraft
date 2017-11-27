using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveHitCircle : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord(args[0].ToString());
        _Range = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[0]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[0] * args[1]);


        _DamageValue = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[1]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[1] * args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
        var buffs = buffGO.GetComponents<ImpactBuffIntervalRangeSub>();
        foreach (var buff in buffs)
        {
            var subBuffs2 = buffGO.GetComponentsInChildren<ImpactDamage>();
            foreach (var subBuff in subBuffs2)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._DamageRate = _DamageValue;
            }

            buff._Range = _Range;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _Range;
    private float _DamageValue;

    #endregion
}
