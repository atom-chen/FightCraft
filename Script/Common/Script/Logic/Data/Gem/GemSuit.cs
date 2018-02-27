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

    private GemSetRecord _ActSet;
    public GemSetRecord ActSet
    {
        get
        {
            return _ActSet;
        }
    }

    private int _ActLevel;
    public int ActLevel
    {
        get
        {
            return _ActLevel;
        }
    }

    private List<EquipExAttr> _ActSetAttrs = new List<EquipExAttr>();
    public List<EquipExAttr> ActSetAttrs
    {
        get
        {
            if (_ActSetAttrs == null)
            {
                _ActSetAttrs = new List<EquipExAttr>();
                IsActSet();
            }
            return _ActSetAttrs;
        }
    }

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

    public void UseGemSet(GemSetRecord gemSet)
    {
        if (!IsGemSetCanUse(gemSet))
        {
            return;
        }

        for (int i = 0; i < gemSet.Gems.Count; ++i)
        {
            var gemInfo = GemData.Instance.GetGemInfo(gemSet.Gems[i].Id);
            if (gemInfo == null)
            {
                Debug.LogError("Get geminfo error");
            }

            GemData.Instance.PutOnGem(gemInfo, i);
        }
    }

    public bool IsActSet()
    {
        var gemSuitTabs = Tables.TableReader.GemSet.Records;

        _ActSet = null;
        _ActLevel = 10;
        foreach (var gemSuit in gemSuitTabs)
        {
            _ActLevel = 10;
            for (int i = 0; i < gemSuit.Value.Gems.Count; ++i)
            {
                if (GemData.Instance._EquipedGems[i].ItemDataID == gemSuit.Value.Gems[i].Id
                    && GemData.Instance._EquipedGems[i].Level >= gemSuit.Value.MinGemLv)
                {
                    _ActLevel = Mathf.Min(_ActLevel, GemData.Instance._EquipedGems[i].Level);
                }
                else
                {
                    break;
                }

                if (i == gemSuit.Value.Gems.Count - 1)
                {
                    _ActSet = gemSuit.Value;
                    break;
                }
            }
            if (_ActSet != null)
            {
                CalculateSetAttrs();

                Hashtable hash = new Hashtable();
                hash.Add("ActGemSet", _ActSet);
                GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_GEM_ACT_SUIT, this, hash);

                return true;
            }
        }

        
        return false;
    }

    private void CalculateSetAttrs()
    {
        _ActSetAttrs.Clear();
        if (_ActSet != null)
        {
            _ActSetAttrs = GameDataValue.GetGemSetAttr(_ActSet, _ActLevel);
        }
    }
}
