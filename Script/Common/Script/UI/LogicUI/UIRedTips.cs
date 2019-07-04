using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRedTips : MonoBehaviour {

    public enum TipType
    {
        Equip,
        Gem
    }

    public TipType _TipType;
    public GameObject _TipGO;
	
	void OnEnable ()
    {
        if (_TipType == TipType.Equip)
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_PUT_ON, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_SELL, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventHandle);
        }
        else if (_TipType == TipType.Gem)
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_GET, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_ON, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_OFF, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_COMBINE, EventHandle);
        }

        RefreshTip();

    }

    void EventHandle(object go, Hashtable eventArgs)
    {
        RefreshTip();
    }

    void RefreshTip()
    {
        if (_TipType == TipType.Equip)
        {
            if (BackBagPack.Instance.IsAnyEquipBetter())
            {
                _TipGO.SetActive(true);
            }
            else
            {
                _TipGO.SetActive(false);
            }
        }
        else if (_TipType == TipType.Gem)
        {
            if (GemData.Instance.IsAnyGemGanEquip() || GemData.Instance.IsAnyGemGanLvUp())
            {
                _TipGO.SetActive(true);
            }
            else
            {
                _TipGO.SetActive(false);
            }
        }

    }
}
