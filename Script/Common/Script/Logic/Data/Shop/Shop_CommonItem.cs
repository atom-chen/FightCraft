using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class Shop_CommonItem
{
    public static void BuyItem(ItemShop itemShop)
    {
        BackBagPack.Instance.PageItems.AddItem(itemShop.ShopRecord.ScriptParam[0].ToString(), itemShop.ShopRecord.ScriptParam[1]);
    }
}
