using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PurchManager
{

    #region 唯一

    private static PurchManager _Instance = null;
    public static PurchManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new PurchManager();
            }
            return _Instance;
        }
    }

    private PurchManager() { }

    #endregion

    #region purch

    private Action _PurchCallback;

    public void Purch(string idx, Action callBack)
    {
        _PurchCallback = callBack;
        //watch movie

        var chargeRecord = Tables.TableReader.Recharge.GetRecord(idx);
        Hashtable eventHash = new Hashtable();
        eventHash.Add("OrderID", idx);
        eventHash.Add("PurchID", idx);
        eventHash.Add("PurchPrice", chargeRecord.Price);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_IAP_REQ, this, eventHash);

        UILoadingTips.ShowAsyn();

        GameCore.Instance.StartCoroutine(PurchFinish(idx));
        //PurchFinish(idx);
    }

    public IEnumerator PurchFinish(string idx)
    {
        yield return new WaitForSeconds(1.0f);

        UILoadingTips.HideAsyn();

        if (_PurchCallback != null)
            _PurchCallback.Invoke();

        var chargeRecord = Tables.TableReader.Recharge.GetRecord(idx);
        PlayerDataPack.Instance.AddDiamond(chargeRecord.Num);

        Hashtable eventHash = new Hashtable();
        eventHash.Add("OrderID", idx);
        eventHash.Add("PurchID", idx);
        eventHash.Add("PurchPrice", chargeRecord.Price);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, this, eventHash);
    }

    #endregion

    
}
