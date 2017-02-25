using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour
{
    void Start()
    {
        _MotionManager = gameObject.GetComponentInParent<MotionManager>(); 
    }

    public void NextInputStart()
    {
        NotifyAnimEvent("NextInputStart");
    }

    public void NextInputEnd()
    {
        NotifyAnimEvent("NextInputEnd");
    }

    public void ColliderStart(int idx)
    {
        NotifyAnimEvent("ColliderStart", idx);
    }

    public void CollidertEnd(int idx)
    {
        NotifyAnimEvent("ColliderFinish", idx);
    }

    public void KeyFrame()
    {
        NotifyAnimEvent("KeyFrame");
    }

    public void AnimationEnd()
    {
        NotifyAnimEvent("AnimationEnd");
    }

    private MotionManager _MotionManager;
    private void NotifyAnimEvent(string function, object param = null)
    {
        _MotionManager.NotifyAnimEvent(function, param);
    }
}
