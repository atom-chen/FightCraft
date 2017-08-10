using UnityEngine;
using System.Collections;

public class ImpactBase : MonoBehaviour
{
    private ObjMotionSkillBase _SkillMotion;
    public ObjMotionSkillBase SkillMotion
    {
        get
        {
            return _SkillMotion;
        }  
        set
        {
            _SkillMotion = value;
        }
    }


    public virtual void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {

    }

    public virtual void RemoveImpact(MotionManager reciverManager)
    { }

}
