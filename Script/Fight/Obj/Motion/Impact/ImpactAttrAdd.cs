using UnityEngine;
using System.Collections;

public class ImpactAttrAdd : ImpactBuff
{
    public RoleAttrEnum _Attr;
    public float _AddValue;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var value = reciverManager._RoleAttrManager.GetAttrFloat(_Attr) + _AddValue;
        reciverManager._RoleAttrManager.SetAttr(_Attr, value);
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();

        var value = _ReciverManager._RoleAttrManager.GetAttrFloat(_Attr) - _AddValue;
        _ReciverManager._RoleAttrManager.SetAttr(_Attr, value);
    }
}
