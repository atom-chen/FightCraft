using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactManager
{
    public static RoleAttrImpactBase GetAttrImpact(EquipExAttr equipAttr)
    {
        if (equipAttr.AttrID >= RoleAttrEnum.Skill1FireBoom && equipAttr.AttrID <= RoleAttrEnum.Skill3WindAimTarget)
        {
            RoleAttrImpactEleBullet impactEleBullet = new RoleAttrImpactEleBullet();
            impactEleBullet.InitEleBullet(equipAttr.AttrID, equipAttr.SubClass, equipAttr.AttrValue1);
            return impactEleBullet;
        }
        return null;
    }

    public static RoleAttrImpactBase GetAttrImpact(SkillInfoItem skillInfo)
    {
        var impactType = Type.GetType(skillInfo.SkillRecord.SkillAttr);
        if (impactType == null)
            return null;

        var impactBase = Activator.CreateInstance(impactType) as RoleAttrImpactBase;
        if (impactBase == null)
            return null;

        impactBase.InitImpact(skillInfo.SkillRecord.SkillClass, impactBase.GetSkillImpactVal(skillInfo));
        return impactBase;
    }
}
