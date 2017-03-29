using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

using GameBase;
using System;

namespace GameUI
{
    public class UIItemBase : UIBase, IPointerClickHandler
    {
        public override void Show()
        {
            if (!gameObject.activeSelf)
            {
                //Hashtable hash = new Hashtable();
                //hash.Add("UIName", gameObject.name);
                //hash.Add("UIObj", gameObject);
                //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOWED, this, new Hashtable());

                gameObject.SetActive(true);
            }
        }

        public override void Hide()
        {
            if (gameObject.activeSelf)
            {
                //Hashtable hash = new Hashtable();
                //hash.Add("UIName", gameObject.name);
                //hash.Add("UIObj", gameObject);
                //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_HIDED, this, new Hashtable());

                gameObject.SetActive(false);
            }
        }

        #region click

        public delegate void ItemClick(object initInfo);

        public object _InitInfo;
        public ItemClick _ClickEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_ClickEvent != null)
            {
                _ClickEvent(_InitInfo);
            }
        }

        #endregion
    }
}
