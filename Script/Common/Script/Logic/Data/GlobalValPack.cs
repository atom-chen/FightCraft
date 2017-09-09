using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GlobalValPack : DataPackBase
{
    #region 单例

    private static GlobalValPack _Instance;
    public static GlobalValPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GlobalValPack();
            }
            return _Instance;
        }
    }

    private GlobalValPack() { }

    #endregion

}

