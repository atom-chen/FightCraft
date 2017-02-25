using UnityEngine;
using System.Collections;

public class ObjMotionBase : MonoBehaviour
{
    public AnimationClip _AnimationClip;
    public int _MotionPriority;
    protected MotionManager _MotionManager;

    public virtual void InitMotion(MotionManager manager)
    {
        _MotionManager = manager;
        if (_AnimationClip != null)
        {
            _MotionManager.InitAnimation(_AnimationClip);
        }
        InitEvent();
    }

    public virtual void PlayMotion(object go, Hashtable eventArgs)
    {
        _MotionManager.MotionStart(this);
    }

    public virtual bool ActiveInput(InputManager inputManager)
    {
        return false;
    }

    public virtual bool ContinueInput(InputManager inputManager)
    {
        return false;
    }

    public virtual void AnimEvent(string function, object param)
    {

    }

    protected virtual void InitEvent()
    {

    }

    public virtual void StopMotion()
    {

    }

    public virtual bool IsCanActiveMotion()
    {
        if (_MotionManager.CurMotion == null)
            return true;

        if (_MotionManager.CurMotion._MotionPriority < _MotionPriority)
        {
            return true;
        }

        return false;
    }

    public virtual void MotionFinish()
    {
        _MotionManager.MotionFinish(this);
    }


    protected virtual void AddAnimationEndEvent(AnimationClip animClip)
    {
        if (animClip == null)
            return;

        UnityEngine.AnimationEvent animEvent = new UnityEngine.AnimationEvent();
        animEvent.time = animClip.length;
        animEvent.functionName = "AnimationEnd";
        animClip.AddEvent(animEvent);
    }
}
