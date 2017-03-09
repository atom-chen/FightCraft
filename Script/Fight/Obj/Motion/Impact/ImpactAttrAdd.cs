using UnityEngine;
using System.Collections;

public class ImpactAttrAdd : ImpactBuff
{
    public RoleAttrEnum _Attr;
    public float _AddValue;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        var value = reciverManager.RoleAttrManager.GetAttrFloat(_Attr) + _AddValue;
        reciverManager.RoleAttrManager.SetAttr(_Attr, value);
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();

        var value = _ReciverManager.RoleAttrManager.GetAttrFloat(_Attr) - _AddValue;
        _ReciverManager.RoleAttrManager.SetAttr(_Attr, value);
    }
}
