using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class RoleAttrImpactSetAttrByEquip : RoleAttrImpactBase
{
    private enum SetAttrType
    {
        None,
        EleResistByArmor = 1,
        EleAttackByWeapon,
        EleEnhanceByRing,
    }

    public static EquipExAttr GetSetAttr(AttrValueRecord attrRecord, int value)
    {
        EquipExAttr equipEx = new EquipExAttr();
        switch ((SetAttrType)attrRecord.AttrParams[0])
        {
            case SetAttrType.EleResistByArmor:
                equipEx.AttrType = "RoleAttrImpactBaseAttr";
                equipEx.Value = value;
                equipEx.AttrParams.Add((int)RoleAttrEnum.FireResistan);
                var equipItem = RoleData.SelectRole.GetEquipItem(EQUIP_SLOT.TORSO);
                if (equipItem != null)
                {
                    foreach (var equipExAttr in equipItem.EquipExAttr)
                    {
                        if (equipExAttr.AttrType != "RoleAttrImpactBaseAttr")
                            continue;

                        switch ((RoleAttrEnum)equipExAttr.AttrParams[0])
                        {
                            case RoleAttrEnum.ColdResistan:
                            case RoleAttrEnum.FireResistan:
                            case RoleAttrEnum.LightingResistan:
                            case RoleAttrEnum.WindResistan:
                                equipEx.AttrParams[0] = equipExAttr.AttrParams[0];
                                break;
                        }
                    }
                }
                equipEx.AttrParams.Add(GameDataValue.GetValueAttr((RoleAttrEnum)equipEx.AttrParams[0], equipEx.Value));
                break;
            case SetAttrType.EleAttackByWeapon:
                equipEx.AttrType = "RoleAttrImpactBaseAttr";
                equipEx.Value = value;
                equipEx.AttrParams.Add((int)RoleAttrEnum.FireAttackAdd);
                var equipWeapon = RoleData.SelectRole.GetEquipItem(EQUIP_SLOT.WEAPON);
                if (equipWeapon != null)
                {
                    foreach (var equipExAttr in equipWeapon.EquipExAttr)
                    {
                        if (equipExAttr.AttrType != "RoleAttrImpactBaseAttr")
                            continue;

                        switch ((RoleAttrEnum)equipExAttr.AttrParams[0])
                        {
                            case RoleAttrEnum.FireAttackAdd:
                            case RoleAttrEnum.ColdAttackAdd:
                            case RoleAttrEnum.LightingAttackAdd:
                            case RoleAttrEnum.WindAttackAdd:
                                equipEx.AttrParams[0] = equipExAttr.AttrParams[0];
                                break;
                        }
                    }
                }
                equipEx.AttrParams.Add(GameDataValue.GetValueAttr((RoleAttrEnum)equipEx.AttrParams[0], equipEx.Value));
                break;
            case SetAttrType.EleEnhanceByRing:
                equipEx.AttrType = "RoleAttrImpactBaseAttr";
                equipEx.Value = value;
                equipEx.AttrParams.Add((int)RoleAttrEnum.FireEnhance);
                var equipRing = RoleData.SelectRole.GetEquipItem(EQUIP_SLOT.WEAPON);
                if (equipRing != null)
                {
                    foreach (var equipExAttr in equipRing.EquipExAttr)
                    {
                        if (equipExAttr.AttrType != "RoleAttrImpactBaseAttr")
                            continue;

                        switch ((RoleAttrEnum)equipExAttr.AttrParams[0])
                        {
                            case RoleAttrEnum.FireAttackAdd:
                                equipEx.AttrParams[0] = (int)RoleAttrEnum.FireEnhance;
                                break;
                            case RoleAttrEnum.ColdAttackAdd:
                                equipEx.AttrParams[0] = (int)RoleAttrEnum.ColdEnhance;
                                break;
                            case RoleAttrEnum.LightingAttackAdd:
                                equipEx.AttrParams[0] = (int)RoleAttrEnum.LightingEnhance;
                                break;
                            case RoleAttrEnum.WindAttackAdd:
                                equipEx.AttrParams[0] = (int)RoleAttrEnum.WindEnhance;
                                break;
                        }
                    }
                }
                equipEx.AttrParams.Add(GameDataValue.GetValueAttr((RoleAttrEnum)equipEx.AttrParams[0], equipEx.Value));
                break;

        }
        return equipEx;
    }
}
