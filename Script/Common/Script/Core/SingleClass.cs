using System;
using System.Diagnostics;

using UnityEngine;

public class SingleClass<T>:MonoBehaviour
{
    protected static T _Instance = default(T);
    public static T Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = Activator.CreateInstance<T>();
            return _Instance;
        }
    }
    protected SingleClass()
    {

    }
}