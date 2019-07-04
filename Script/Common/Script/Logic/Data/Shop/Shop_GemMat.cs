using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class Shop_GemMat
{
    public static void BuyItem(ItemShop itemShop)
    {
        if (itemShop.ShopRecord.ScriptParam[0] < 0)
        {
            foreach (var gemRecord in TableReader.GemTable.Records.Values)
            {
                GemData.Instance.CreateGem(gemRecord.Id, 1);
            }
            
        }
        else
        {
            var gemRecord = TableReader.GemTable.GetRecord(itemShop.ShopRecord.ScriptParam[0].ToString());
            GemData.Instance.CreateGem(gemRecord.Id, 1);
        }
    }
}
