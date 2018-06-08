using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Gambling
{
    public static void BuyItem(ItemShop itemShop)
    {
        int level = Random.Range(RoleData.SelectRole._RoleLevel - 5, RoleData.SelectRole._RoleLevel + 5);
        level = Mathf.Clamp(level, 1, RoleData.MAX_ROLE_LEVEL);

        int quality = GameRandom.GetRandomLevel(0,80,15,0);

        var equipItem = ItemEquip.CreateEquip(level, (Tables.ITEM_QUALITY)quality, -1, itemShop.ShopRecord.ScriptParam[0], itemShop.ShopRecord.ScriptParam[1]);
        var newEquip = BackBagPack.Instance.AddNewEquip(equipItem);
    }
}
