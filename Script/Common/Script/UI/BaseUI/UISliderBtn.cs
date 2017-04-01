using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;



public class UISliderBtn : MonoBehaviour, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region 

    public delegate void SliderDelegate(Vector2 delta);
    public delegate void SliderVoidDelegate();

    public SliderDelegate _DragAction;
    public SliderVoidDelegate _DragBeginAction;
    public SliderVoidDelegate _DragEndAction;

    public float _ActionTime = 0.2f;
    private float _LastActionTime = 0;
    #endregion


    #region Drag

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("DragDelta:" + eventData.delta);
        {
            if (_DragAction != null && (Time.time - _LastActionTime) > _ActionTime)
            {
                _LastActionTime = Time.time;
                _DragAction(eventData.delta);
            }
        }
        //throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_DragBeginAction != null)
        {
            _DragBeginAction();
        }
        //throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_DragEndAction != null)
        {
            _DragEndAction();
        }
        //throw new NotImplementedException();
    }

    #endregion
}

