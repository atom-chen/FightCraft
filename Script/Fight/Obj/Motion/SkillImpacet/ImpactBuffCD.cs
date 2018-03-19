using UnityEngine;
using System.Collections;

public class ImpactBuffCD : ImpactBuff
{
    public float _ActCD = 5;

    private float _StartTime = -1;

    public void SetCD()
    {
        _StartTime = Time.time;
    }

    public bool IsInCD()
    {
        if (_StartTime < 0)
            return false;

        var cdTime = Time.time - _StartTime;
        return _ActCD > cdTime;
    }

    public float GetCDProcess()
    {
        var cdTime = Time.time - _StartTime;
        if (cdTime > _ActCD)
            return 1;

        return cdTime / _ActCD;
    }
}
