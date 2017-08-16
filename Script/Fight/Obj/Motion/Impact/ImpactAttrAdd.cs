using UnityEngine;
using System.Collections;

public class ImpactAttrAdd : ImpactBuff
{
    #region 

    [System.Serializable]
    public enum ADDTYPE
    {
        Value,
        Persent
    }

    #endregion

    public RoleAttrEnum _Attr;
    public float _AddValue;
    public ADDTYPE _AddType;

    private int _RealAddValue;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        int originValue = reciverManager.RoleAttrManager.GetBaseAttr(_Attr);
        int value = 0;
        if (_AddType == ADDTYPE.Value)
        {
            value = (int)(originValue + _AddValue);
        }
        else if (_AddType == ADDTYPE.Persent)
        {
            value = (int)(originValue * (1 +_AddValue));
        }

        reciverManager.RoleAttrManager.SetBaseAttr(_Attr, value);
        _RealAddValue = reciverManager.RoleAttrManager.GetBaseAttr(_Attr) - originValue;
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        var value = reciverManager.RoleAttrManager.GetBaseAttr(_Attr) - _RealAddValue;
        reciverManager.RoleAttrManager.SetBaseAttr(_Attr, value);
    }
}
