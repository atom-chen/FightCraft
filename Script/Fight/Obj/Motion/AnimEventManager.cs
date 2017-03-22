﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimEventManager : MonoBehaviour
{
    #region skill event

    public const string NEXT_INPUT_START = "NextInputStart";
    public const string NEXT_INPUT_END = "NextInputEnd";
    public const string COLLIDER_START = "ColliderStart";
    public const string COLLIDER_END = "ColliderEnd";
    public const string KEY_FRAME = "KeyFrame";
    public const string ANIMATION_END = "AnimationEnd";

    public void Init()
    {
        _MotionManager = gameObject.GetComponentInParent<MotionManager>(); 
    }

    public void NextInputStart()
    {
        NotifyAnimEvent(NEXT_INPUT_START);
    }

    public void NextInputEnd()
    {
        NotifyAnimEvent(NEXT_INPUT_END);
    }

    public void ColliderStart(int idx)
    {
        NotifyAnimEvent(COLLIDER_START, idx);
    }

    public void CollidertEnd(int idx)
    {
        NotifyAnimEvent(COLLIDER_END, idx);
    }

    public void KeyFrame()
    {
        NotifyAnimEvent(KEY_FRAME);
    }

    public void AnimationEnd()
    {
        NotifyAnimEvent(ANIMATION_END);
    }

    private MotionManager _MotionManager;
    private void NotifyAnimEvent(string function, object param = null)
    {
        _MotionManager.NotifyAnimEvent(function, param);
    }

    #endregion

    #region specil event

    private Dictionary<string, Action> _AnimCallBack = new Dictionary<string, Action>();

    public void AddEvent(AnimationClip animClip, float animTime, Action callBack)
    {
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = animTime;
        animEvent.functionName = "SpecilEventCallBack";
        animEvent.stringParameter = callBack.Method.ToString();
        animClip.AddEvent(animEvent);

        _AnimCallBack.Add(callBack.Method.ToString(), callBack);
    }

    public float GetAnimFirstColliderEventTime(AnimationClip animClip)
    {
        foreach (var animEvent in animClip.events)
        {
            if (animEvent.functionName == "ColliderStart")
                return animEvent.time;
        }

        return -1;
    }

    public void SpecilEventCallBack(string strParam)
    {
        if (_AnimCallBack.ContainsKey(strParam))
        {
            _AnimCallBack[strParam].Invoke();
        }
    }

    #endregion
}
