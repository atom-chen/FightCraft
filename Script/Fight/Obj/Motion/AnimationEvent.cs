using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour
{
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
}
