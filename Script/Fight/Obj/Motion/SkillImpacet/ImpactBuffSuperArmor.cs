using UnityEngine;
using System.Collections;

public class ImpactBuffSuperArmor : ImpactBuff
{
    public EffectController _HitEffect;
    public float _BlockTime = 0.0f;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        if (_BuffOwner._ActionState == _BuffOwner._StateCatch
            || _BuffOwner._ActionState == _BuffOwner._StateHit)
        {
            _BuffOwner.TryEnterState(_BuffOwner._StateIdle);
        }
        base.ActBuff(senderManager, reciverManager);
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);
    }

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit impactHit)
    {
        //GlobalEffect.Instance.Pause(0.1f);
        _BuffOwner.ResetMove();
        _BuffOwner.ActionPause(_BlockTime);

        if (!impactHit._IsBulletHit)
        {
            impactSender.ActionPause(_BlockTime);
        }

        if (_HitEffect != null)
            _BuffOwner.PlaySkillEffect(_HitEffect);
        ((EffectOutLine)_DynamicEffect).PlayHitted();

        Hashtable hash = new Hashtable();
        hash.Add("Motion", _SenderMotion);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_SOMEONE_SUPER_ARMOR, this, hash);

        return false;
    }
}
