using UnityEngine;
using System.Collections;

public class ImpactBase : MonoBehaviour
{

    public virtual void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {

    }

    public virtual void RemoveImpact(MotionManager reciverManager)
    { }

}
