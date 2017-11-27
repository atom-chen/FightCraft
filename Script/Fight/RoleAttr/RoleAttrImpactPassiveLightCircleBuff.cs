using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveLightCircleBuff : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var legendaryEquip = Tables.TableReader.LegendaryEquip.GetRecord(args[0].ToString());
        _ActRate = legendaryEquip.ImpactValues[0] + legendaryEquip.ImpactValueIncs[0] * args[1];
        if (legendaryEquip.ImpactValues[1] > 0)
        {
            _ActRate = Mathf.Min(_ActRate, legendaryEquip.ImpactValues[1]);
        }
        _Damage = GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValues[2]) + GameDataValue.ConfigIntToFloat(legendaryEquip.ImpactValueIncs[1] * args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
        var buffs = buffGO.GetComponents<ImpactBuffHitEnemySub>();
        foreach (var buff in buffs)
        {
            var subBuffs2 = buffGO.GetComponentsInChildren<BulletEmitterBase>();
            foreach (var subBuff in subBuffs2)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._Damage = _Damage;
            }

            buff._Rate = _ActRate;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private int _ActRate;
    private float _Damage;
    
    #endregion
}
