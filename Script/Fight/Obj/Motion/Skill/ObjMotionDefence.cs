using UnityEngine;
using System.Collections;

public class ObjMotionDefence : ObjMotionSkillBase
{
    

    #region unity



    #endregion

    public EffectController _DefendEffect;

    public void PlayMotionHit(object go, Hashtable eventArgs)
    {

        if ((GameBase.EVENT_TYPE)eventArgs["EVENT_TYPE"] == GameBase.EVENT_TYPE.EVENT_MOTION_HIT)
        {
            eventArgs.Add("StopEvent", true);
        }

        if (_DefendEffect != null)
        {
            _DefendEffect.PlayEffect();
        }
    }

}
