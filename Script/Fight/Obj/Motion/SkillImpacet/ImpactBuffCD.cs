using UnityEngine;
using System.Collections;

public class ImpactBuffCD : ImpactBuff
{
    public float _ActCD = 5;

    private float _StartTime;

    public void SetCD()
    {
        _StartTime = Time.time;
    }

    public bool IsInCD()
    {
        var cdTime = Time.time - _StartTime;
        return _ActCD > cdTime;
    }
}
