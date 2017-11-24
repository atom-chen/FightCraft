using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveHPResume : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord(args[0].ToString());
        _ActCD = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[0]) - GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[0] * args[1]);
        if (legendaryEquip.ImpactValues[1] > 0)
        {
            _ActCD = Mathf.Max(_ActCD, legendaryEquip.ImpactValues[1]);
        }

        _ResumeHP = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[2]) - GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[1] * args[1]);
        _ResumeHP = Mathf.Min(_ResumeHP, 1);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        var buffs = buffGO.GetComponents<ImpactBuffCD>();
        foreach (var buff in buffs)
        {
            buff._ActCD = _ActCD;
            buff.ActImpact(roleMotion, roleMotion);
        }

        var hpBuffs = buffGO.GetComponents<ImpactResumeHP>();
        foreach (var buff in hpBuffs)
        {
            buff._HPPersent = _ResumeHP;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _ActCD;
    private float _ResumeHP;


    #endregion
}
