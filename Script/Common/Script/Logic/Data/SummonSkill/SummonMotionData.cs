using System.Collections;
using System.Collections.Generic;
using Tables;

public class SummonMotionData : ItemBase
{
    public string SummonRecordID
    {
        get
        {
            return ItemDataID;
        }
        set
        {
            ItemDataID = value;
        }
    }
    
    public int Exp
    {
        get
        {
            return _DynamicDataInt[1];
        }
        set
        {
            _DynamicDataInt[1] = value;
        }
    }

    public int StarExp
    {
        get
        {
            return _DynamicDataInt[2];
        }
        set
        {
            _DynamicDataInt[2] = value;
        }
    }

    public SummonMotionData()
    {
        SummonRecordID = "";
        _DynamicDataInt.Add(1);
        _DynamicDataInt.Add(0);
        _DynamicDataInt.Add(0);
    }

    public SummonMotionData(string recordID)
    {
        SummonRecordID = recordID;
        _DynamicDataInt.Add(1);
        _DynamicDataInt.Add(0);
        _DynamicDataInt.Add(0);
    }

    private SummonSkillRecord _SummonRecord;
    public SummonSkillRecord SummonRecord
    {
        get
        {
            if (_SummonRecord == null)
            {
                _SummonRecord = Tables.TableReader.SummonSkill.GetRecord(SummonRecordID);
            }
            return _SummonRecord;
        }
    }

    #region level

    private int _Level = -1;
    public int Level
    {
        get
        {
            if (_Level < 0)
            {
                CalculateLevel();
            }
            return _Level;
        }
    }

    private int _CurLvExp = -1;
    public int CurLvExp
    {
        get
        {
            if (_CurLvExp < 0)
            {
                CalculateLevel();
            }
            return _CurLvExp;
        }
    }

    public bool IsCanAddExp()
    {
        if (Level < StarLevel * 10)
            return true;

        return false;
    }

    public void AddExp(int expValue)
    {
        Exp += expValue;
        CalculateLevel();
    }

    private void CalculateLevel()
    {
        int tempExp = Exp;
        int level = 1;
        while (true)
        {
            var summonAttrRecord = TableReader.SummonSkillAttr.GetRecord(level.ToString());
            if (tempExp >= summonAttrRecord.Cost[0])
            {
                tempExp = tempExp - summonAttrRecord.Cost[0];
                ++level;

                if (level == StarLevel * 10)
                {
                    tempExp = 0;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        _Level = level;
        _CurLvExp = tempExp;

        UpdateAttrs();
    }

    public static int GetLevelByExp(int exp, int starLevel, out int lastExp)
    {
        int tempExp = exp;
        int level = 1;
        while (true)
        {
            var summonAttrRecord = TableReader.SummonSkillAttr.GetRecord(level.ToString());
            if (tempExp >= summonAttrRecord.Cost[0])
            {
                tempExp = tempExp - summonAttrRecord.Cost[0];
                ++level;

                if (level == starLevel * 10)
                {
                    tempExp = 0;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        

        lastExp = tempExp;
        return level;
    }

    #endregion

    #region star

    private int _StarLevel = -1;
    public int StarLevel
    {
        get
        {
            if (_StarLevel < 0)
            {
                CalculateStarLevel();
            }
            return _StarLevel;
        }
    }

    private int _CurStarExp = -1;
    public int CurStarExp
    {
        get
        {
            if (_CurStarExp < 0)
            {
                CalculateStarLevel();
            }
            return _CurStarExp;
        }
    }

    public int CurStarLevelExp()
    {
        if (StarLevel < SummonRecord.StarExp.Count)
        {
            return SummonRecord.StarExp[StarLevel];
        }
        return 0;
    }

    public void AddStarExp(int expValue)
    {
        StarExp += expValue;
        CalculateStarLevel();
    }

    private void CalculateStarLevel()
    {
        int tempExp = StarExp;
        int starLv = 0;
        for (int i = 0; i < SummonRecord.StarExp.Count; ++i)
        {
            if (tempExp >= SummonRecord.StarExp[i])
            {
                tempExp = tempExp - SummonRecord.StarExp[i];
                ++starLv;
            }
            else
            {
                break;
            }
        }

        _StarLevel = starLv;
        _CurStarExp = tempExp;
    }

    public static int GetStarLevelByExp(SummonMotionData motionData, int exp, out int lastExp)
    {
        int tempExp = exp;
        int starLv = 0;
        for (int i = 0; i < motionData.SummonRecord.StarExp.Count; ++i)
        {
            if (tempExp >= motionData.SummonRecord.StarExp[i])
            {
                tempExp = tempExp - motionData.SummonRecord.StarExp[i];
                ++starLv;
            }
            else
            {
                break;
            }
        }

        lastExp = tempExp;
        return starLv;
    }

    #endregion

    #region attr

    public static List<RoleAttrEnum> StageAttrEnums = new List<RoleAttrEnum>()
    { RoleAttrEnum.Attack, RoleAttrEnum.CriticalHitChance, RoleAttrEnum.CriticalHitDamge, RoleAttrEnum.RiseHandSpeed, RoleAttrEnum.AttackSpeed};

    public static List<int> StageAttrAdd = new List<int>()
    { 1, 100,100,100,100 };

    private List<EquipExAttr> _SummonAttrs;
    public List<EquipExAttr> SummonAttrs
    {
        get
        {
            if (_SummonAttrs == null)
            {
                UpdateAttrs();
            }
            return _SummonAttrs;
        }
    }

    public void UpdateAttrs()
    {
        _SummonAttrs = new List<EquipExAttr>();
        //InitStageAttr();

        var levelAttr = Tables.TableReader.SummonSkillAttr.GetRecord(Level.ToString());

        var atkValue = (int)(levelAttr.Attr[0] * SummonRecord.AttrModelfy);
        var critiDmgValue = (int)(levelAttr.Attr[1]);
        var critiRateValue = (int)(levelAttr.Attr[2]);
        var riseHandSpeedValue = (int)(levelAttr.Attr[3]);
        var atkSpeedValue = (int)(levelAttr.Attr[4]);

        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[0], atkValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[1], critiDmgValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[2], critiRateValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[3], riseHandSpeedValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[4], atkSpeedValue));
    }
    #endregion
}

