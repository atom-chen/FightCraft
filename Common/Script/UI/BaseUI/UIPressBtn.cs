using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace GameUI
{
    public class UIPressBtn : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region 

        public delegate void PressDelegate(bool isDown);

        private PressDelegate _PressAction;
        public void SetPressAction(PressDelegate action)
        {
            _PressAction = action;
        }

        #endregion

        #region  IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            //throw new NotImplementedException();
            if (_PressAction != null)
            {
                _PressAction(true);
            }
        }

        #endregion

        #region IPointerUpHandler

        public void OnPointerUp(PointerEventData eventData)
        {
            //throw new NotImplementedException();
            if (_PressAction != null)
            {
                _PressAction(false);
            }
        }

        #endregion
    }
}
