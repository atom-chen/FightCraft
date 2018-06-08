using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_GemMat
{
    public static void BuyItem(ItemShop itemShop)
    {
        var randomVal = Random.Range(itemShop.ShopRecord.ScriptParam[1], itemShop.ShopRecord.ScriptParam[2]);
        BackBagPack.Instance.AddItem(itemShop.ShopRecord.ScriptParam[0].ToString(), randomVal);
    }
}
