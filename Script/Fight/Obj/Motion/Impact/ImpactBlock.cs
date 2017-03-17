using UnityEngine;
using System.Collections;

public class ImpactBlock : ImpactBuff
{
    public EffectController _HitEffect;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        reciverManager.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, HitEvent, 99);
        reciverManager.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, FlyEvent, 99);
        Debug.Log("ImpactBlock act");
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();

        _ReciverManager.EventController.UnRegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, HitEvent);
        _ReciverManager.EventController.UnRegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, FlyEvent);
        Debug.Log("ImpactBlock remove");
    }

    private void HitEvent(object sender, Hashtable eventArgs)
    {
        eventArgs.Add("StopEvent", true);
        //GlobalEffect.Instance.Pause(0.1f);
        _ReciverManager.ResetMove();
        _ReciverManager.SkillPause(0.3f);
        _ReciverManager.PlaySkillEffect(_HitEffect);
    }

    private void FlyEvent(object sender, Hashtable eventArgs)
    {
        eventArgs.Add("StopEvent", true);
        //GlobalEffect.Instance.Pause(0.1f);
        _ReciverManager.ResetMove();
        _ReciverManager.SkillPause(0.3f);
        _ReciverManager.PlaySkillEffect(_HitEffect);

    }
}
