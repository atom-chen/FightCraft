﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.Events;

public class UIBackRayBehind : MonoBehaviour, IPointerClickHandler
{
    [Serializable]
    public class BackClickEvent : UnityEvent
    {
        public BackClickEvent()
        {

        }
    }

    [SerializeField]
    private BackClickEvent _BackClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_BackClick != null)
            _BackClick.Invoke();

        GameUI.UIManager.Instance.RayCastBebind(eventData);
    }
}