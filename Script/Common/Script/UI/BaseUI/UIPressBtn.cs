using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;



public class UIPressBtn : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
    #region 

    [Serializable]
    public class PressAction : UnityEvent<bool>
    {
        public PressAction() { }
    }

    [SerializeField]
    public PressAction _PressAction;

    private bool _IsPress = false;
    public bool IsPress
    {
        get
        {
            return _IsPress;
        }
    }

    #endregion

    #region  IPointerDownHandler

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        if (_PressAction != null)
        {
            _PressAction.Invoke(true);
        }
        _IsPress = true;
    }

    #endregion

    #region IPointerUpHandler

    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        if (_PressAction != null)
        {
            _PressAction.Invoke(false);
        }
        _IsPress = false;
    }

    #endregion
}

