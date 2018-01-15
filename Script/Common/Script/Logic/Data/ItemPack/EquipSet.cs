using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;

public class EquipSet
{
    #region 唯一

    private static EquipSet _Instance = null;
    public static EquipSet Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new EquipSet();
            }
            return _Instance;
        }
    }

    private EquipSet()
    {
        
    }

    #endregion

    public Dictionary<EquipSpAttrRecord, int> _ActingEquipSpAttr = new Dictionary<EquipSpAttrRecord, int>();

    public void RemoveActingSpAttr(EquipSpAttrRecord spAttr)
    {
        if (_ActingEquipSpAttr.ContainsKey(spAttr))
        {
            --_ActingEquipSpAttr[spAttr];
            if (_ActingEquipSpAttr[spAttr] == 0)
            {
                _ActingEquipSpAttr.Remove(spAttr);
            }
        }
    }

    public void ActingSpAttr(EquipSpAttrRecord spAttr)
    {
        if (!_ActingEquipSpAttr.ContainsKey(spAttr))
        {
            _ActingEquipSpAttr.Add(spAttr, 0);
        }
        ++_ActingEquipSpAttr[spAttr];
    }
}
