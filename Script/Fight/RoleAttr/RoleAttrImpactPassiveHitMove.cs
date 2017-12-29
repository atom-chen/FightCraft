using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveHitMove : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _LastTime = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] * args[1]);

        _MoveSpeed = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1]) + GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1] * args[1]);
        if (attrTab.AttrParams[1] > 0)
        {
            _MoveSpeed = Mathf.Min(_MoveSpeed, attrTab.AttrParams[2]);
        }
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
            var subBuffs = buffGO.GetComponentsInChildren<ImpactBuffAttrAdd>();
            foreach (var subBuff in subBuffs)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._LastTime = _LastTime;
                subBuff._AddValue = _MoveSpeed;
            }

            buff.ActImpact(roleMotion, roleMotion);
        }
    }


    #region 

    private float _LastTime;
    private float _MoveSpeed;
    
    #endregion
}
