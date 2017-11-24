using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveAddAttr : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord(args[0].ToString());
        _AddValue = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[0]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[0] * args[1]);
        if (legendaryEquip.ImpactValues[1] > 0)
        {
            _AddValue = Mathf.Min(_AddValue, legendaryEquip.ImpactValues[1]);
        }
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        var buffs = buffGO.GetComponents<ImpactBuffAttrAdd>();
        foreach (var buff in buffs)
        {
            buff._AddValue = _AddValue;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _AddValue;
    
    #endregion
}
