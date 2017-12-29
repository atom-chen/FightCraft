using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataValue
{
    public static float ConfigIntToFloat(int val)
    {
         return val * 0.0001f;
    }

    public static int ConfigFloatToInt(float val)
    {
        return (int)(val * 10000);
    }

    public static int GetMaxRate()
    {
        return 10000;
    }
}
