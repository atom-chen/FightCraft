using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Gold
{

    public static void BuyItem(ItemShop itemShop)
    {
        PlayerDataPack.Instance.AddGold(itemShop.ShopRecord.ScriptParam[0]);
    }
}
