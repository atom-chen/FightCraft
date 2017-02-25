using UnityEngine;
using System.Collections;

public class ObjMotionDefence : ObjMotionSkillBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

        _MotionPriority = 100; 
    }

    public override void PlayMotion(object go, Hashtable eventArgs)
    {

    }

    public override bool ActiveInput(InputManager inputManager)
    {
        return base.ActiveInput(inputManager);
    }

    public override bool ContinueInput(InputManager inputManager)
    {
        return base.ContinueInput(inputManager);
    }

    protected override void InitEvent()
    {
        base.InitEvent();

        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, PlayMotionHit, 100);
    }
    #endregion

    #region unity



    #endregion

    public EffectController _DefendEffect;

    public void PlayMotionHit(object go, Hashtable eventArgs)
    {
        if (_MotionManager.CurMotion != this)
            return;

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
