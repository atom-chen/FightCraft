using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;

public class GemSuit
{
    #region 唯一

    private static GemSuit _Instance = null;
    public static GemSuit Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GemSuit();
            }
            return _Instance;
        }
    }

    private GemSuit()
    {
        
    }

    #endregion

    public bool IsGemSetCanUse(GemSetRecord gemSet)
    {
        foreach (var gemRecord in gemSet.Gems)
        {
            var gemInfo = GemData.Instance.GetGemInfo(gemRecord.Id);
            if (gemInfo.Level < gemSet.MinGemLv)
                return false;
        }

        return true;
    }

}
