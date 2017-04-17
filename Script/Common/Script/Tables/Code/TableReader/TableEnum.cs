using System.Collections;

namespace Tables
{

    //
    public enum EQUIP_CLASS
    {
        None = 0, //,枚举必须保留0值
        WEAPON_BOY_DEFENCE = 1, //,
        WEAPON_GIRL_DOUGE = 2, //,
        WEAPON_BOY_DOUGE = 3, //,
        WEAPON_GIRL_DEFENCE = 4, //,
        DEFENCE = 5, //,
        JEWELRY = 6, //,
    }

    //
    public enum EQUIP_SLOT
    {
        WEAPON = 0, //,武器
        TORSO = 1, //,装甲
        LEGS = 2, //,护腿
        AMULET = 3, //,项链
        RING = 4, //,戒指
    }

    //
    public enum ITEM_QUALITY
    {
        WHITE = 0, //,枚举必须保留0值
        BLUE = 1, //,
        PURPER = 2, //,
        ORIGIN = 3, //,
    }

    //
    public enum PROFESSION
    {
        BOY_DEFENCE = 0, //,枚举必须保留0值
        GIRL_DOUGE = 1, //,
        BOY_DOUGE = 2, //,
        GIRL_DEFENCE = 3, //,
        MAX = 4, //,
        NONE = -1, //,
    }


}