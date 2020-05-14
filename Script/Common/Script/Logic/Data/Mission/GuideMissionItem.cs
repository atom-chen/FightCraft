using UnityEngine;
using System.Collections;
using Tables;
using System;

public class GuideMissionItem : SaveItemBase
{
    public enum GuideType
    {
        Skill,
        Debuff,
        Buff,
        Defence,
    }

    public GuideMissionItem()
    {

    }    

    public int ID;
    public GuideType SkillType;
    public int SkillIdx;
    public int GoldAward = 0;
    public int DiamondAward = 0;
    public int ItemAward = 0;
    public int ItemAwardCnt = 0;

}

