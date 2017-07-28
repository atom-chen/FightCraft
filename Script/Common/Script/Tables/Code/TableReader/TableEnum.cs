using System.Collections;

namespace Tables
{

    //
    public enum ATTR_EFFECT
    {
        None = 0, //None,枚举必须保留0值
        ATTACK_DAMAGE = 10001, //ATTACK_DAMAGE,ATTACK_DAMAGE
        ATTACK_SPEED = 10002, //ATTACK_SPEED,ATTACK_SPEED
        ATTACK_RANGE = 10003, //ATTACK_RANGE,ATTACK_RANGE
        SKILL_DAMAGE = 10101, //SKILL_DAMAGE,SKILL_DAMAGE
        SKILL_SPEED = 10102, //SKILL_SPEED,SKILL_SPEED
        SKILL_RANGE = 10103, //SKILL_RANGE,SKILL_RANGE
        SKILL_ACCUMULATE = 10111, //SKILL_ACCUMULATE,SKILL_ACCUMULATE
        SKILL_SHADOWARRIOR = 10112, //SKILL_SHADOWARRIOR,SKILL_SHADOWARRIOR
        SKILL_BACKRANGE = 10113, //SKILL_BACKRANGE,SKILL_BACKRANGE
        SKILL_ACTBUFF = 10114, //SKILL_ACTBUFF,SKILL_ACTBUFF
    }

    //
    public enum EQUIP_CLASS
    {
        None = 0, //None,枚举必须保留0值
        WEAPON_BOY_DEFENCE = 1, //WEAPON_BOY_DEFENCE,
        WEAPON_GIRL_DOUGE = 2, //WEAPON_GIRL_DOUGE,
        WEAPON_BOY_DOUGE = 3, //WEAPON_BOY_DOUGE,
        WEAPON_GIRL_DEFENCE = 4, //WEAPON_GIRL_DEFENCE,
        DEFENCE = 5, //DEFENCE,
        JEWELRY = 6, //JEWELRY,
    }

    //
    public enum EQUIP_SLOT
    {
        WEAPON = 0, //WEAPON,武器
        TORSO = 1, //TORSO,装甲
        LEGS = 2, //LEGS,护腿
        AMULET = 3, //AMULET,项链
        RING = 4, //RING,戒指
    }

    //
    public enum ITEM_QUALITY
    {
        WHITE = 0, //WHITE,枚举必须保留0值
        BLUE = 1, //BLUE,
        PURPER = 2, //PURPER,
        ORIGIN = 3, //ORIGIN,
    }

    //
    public enum MOTION_TYPE
    {
        MainChar = 0, //MainChar,枚举必须保留0值
        Hero = 1, //Hero,
        Elite = 2, //Elite,
        Normal = 3, //Normal,
    }

    //
    public enum PROFESSION
    {
        BOY_DEFENCE = 0, //BOY_DEFENCE,BOY_DEFENCE
        GIRL_DOUGE = 1, //GIRL_DOUGE,GIRL_DOUGE
        BOY_DOUGE = 2, //BOY_DOUGE,BOY_DOUGE
        GIRL_DEFENCE = 3, //GIRL_DEFENCE,GIRL_DEFENCE
        MAX = 4, //MAX,MAX
        NONE = -1, //NONE,NONE
    }

    //
    public enum SKILL_CLASS
    {
        None = 0, //None,None
        NORMAL_ATTACK = 1, //NORMAL_ATTACK,NORMAL_ATTACK
        EX_SKILL = 2, //EX_SKILL,EX_SKILL
        BUFF = 3, //BUFF,BUFF
        DEFENCE = 4, //DEFENCE,DEFENCE
        DODGE = 5, //DODGE,DODGE
        BASE_ATTRS = 6, //BASE_ATTRS,BASE_ATTRS
    }

    //
    public enum SKILL_EFFECT
    {
        None = 0, //None,枚举必须保留0值
        SPEED = 1, //SPEED,
        DAMAGE = 2, //DAMAGE,
        RANGE = 3, //RANGE,
        TIME = 4, //TIME,
    }


}