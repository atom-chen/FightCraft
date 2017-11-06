using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassive : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord( args[0].ToString());
        _ImpactName = legendaryEquip.BulletName;
        _SkillInput = legendaryEquip.SkillInput;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        var buffs = buffGO.GetComponents<ImpactBuff>();
        foreach (var buff in buffs)
        {
            buff.ActImpact(roleMotion, roleMotion);
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

    public int _Damage;
    public string _ImpactName;
    
    #endregion
}
