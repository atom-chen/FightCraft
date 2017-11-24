using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveArea : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord(args[0].ToString());
        _Range = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[0]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[0] * args[1]);
        if (legendaryEquip.ImpactValues[1] > 0)
        {
            _Range = Mathf.Max(_Range, legendaryEquip.ImpactValues[1]);
        }

        _DefenceValue = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[2]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[1] * args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        var buffs = buffGO.GetComponents<ImpactBuffIntervalRangeSub>();
        foreach (var buff in buffs)
        {
            buff._Range = _Range;
            buff.ActImpact(roleMotion, roleMotion);
        }

        var attrBuffs = buffGO.GetComponents<ImpactBuffAttrAdd>();
        foreach (var buff in attrBuffs)
        {
            buff._AddValue = _DefenceValue;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _Range;
    private float _DefenceValue;

    #endregion
}
