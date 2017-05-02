using UnityEngine;
using System.Collections;

public class ImpactAttrAdd : ImpactBuff
{
    public BaseAttr _Attr;
    public float _AddValue;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        var value = reciverManager.RoleAttrManager.GetBaseAttr(_Attr) + _AddValue;
        reciverManager.RoleAttrManager.SetBaseAttr(_Attr, value);
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        var value = reciverManager.RoleAttrManager.GetBaseAttr(_Attr) - _AddValue;
        reciverManager.RoleAttrManager.SetBaseAttr(_Attr, value);
    }
}
