using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveAttackBless : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ActRate = attrTab.AttrParams[0] + attrTab.AttrParams[0] * args[1];
        if (attrTab.AttrParams[1] > 0)
        {
            _ActRate = Mathf.Min(_ActRate, attrTab.AttrParams[1]);
        }
        _AttrValue = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[2]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1] * args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
        var buffs = buffGO.GetComponents<ImpactBuffAttackSub>();
        foreach (var buff in buffs)
        {
            var subBuffs2 = buffGO.GetComponentsInChildren<ImpactBuffAttrAdd>();
            foreach (var subBuff in subBuffs2)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._AddValue = _AttrValue;
            }

            buff._Rate = _ActRate;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private int _ActRate;
    private float _AttrValue;

    #endregion
}
