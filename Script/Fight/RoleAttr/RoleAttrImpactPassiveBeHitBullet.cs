using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveBeHitBullet : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ActCD = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] * args[1]);
        if (attrTab.AttrParams[1] > 0)
        {
            _ActCD = Mathf.Max(_ActCD, GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1]));
        }

        _DamageValue = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[2]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1] * args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
        var buffs = buffGO.GetComponents<ImpactBuffBeHitSub>();
        foreach (var buff in buffs)
        {
            var subBuffs2 = buffGO.GetComponentsInChildren<BulletEmitterBase>();
            foreach (var subBuff in subBuffs2)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._Damage = _DamageValue;
            }

            buff._ActCD = _ActCD;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _ActCD;
    private float _DamageValue;

    #endregion
}
