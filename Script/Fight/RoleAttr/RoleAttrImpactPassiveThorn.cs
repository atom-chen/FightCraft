using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveThorn : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _Damage = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0]) - GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] * args[1]);
        _Damage = Mathf.Min(_Damage, 1);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
        var buffs = buffGO.GetComponents<ImpactBuff>();
        foreach (var buff in buffs)
        {
            var subBuffs = buffGO.GetComponentsInChildren<ImpactDamage>();
            foreach (var subBuff in subBuffs)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._DamageRate = _Damage;
            }

            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _Damage;
    
    #endregion
}
