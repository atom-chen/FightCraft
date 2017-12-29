using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveHitEnemy : RoleAttrImpactPassive
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ActCD = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] * args[1]);
        _ActRate = attrTab.AttrParams[1] + attrTab.AttrParams[1] * args[1];
        _Damage = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[2]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[2] * args[1]);

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
                subBuff._Damage= _Damage;
            }

            buff._ActCD = _ActCD;
            buff._Rate = _ActRate;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _ActCD;
    private int _ActRate;
    private float _Damage;
    
    #endregion
}
