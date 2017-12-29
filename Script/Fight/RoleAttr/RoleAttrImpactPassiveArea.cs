using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveArea : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _Range = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] * args[1]);
        

        _DefenceValue = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1] * args[1]);
        if (attrTab.AttrParams[2] < 0)
        {
            _DefenceValue = Mathf.Max(_DefenceValue, GameDataValue.ConfigIntToFloat(attrTab.AttrParams[2]));
        }
        else if (attrTab.AttrParams[2] > 0)
        {
            _DefenceValue = Mathf.Min(_DefenceValue, GameDataValue.ConfigIntToFloat(attrTab.AttrParams[2]));
        }
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
            var subBuffs2 = buffGO.GetComponentsInChildren<ImpactBuffAttrAdd>();
            foreach (var subBuff in subBuffs2)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._AddValue = _DefenceValue;
            }

            buff._Range = _Range;
            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _Range;
    private float _DefenceValue;

    #endregion
}
