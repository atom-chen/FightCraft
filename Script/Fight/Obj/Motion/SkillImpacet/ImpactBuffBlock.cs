using UnityEngine;
using System.Collections;

public class ImpactBuffBlock : ImpactBuff
{
    public EffectController _HitEffect;
    public bool _IsBlockBullet;
    public bool _IsBlockNotBullet;
    

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);
    }

    public override bool IsBuffCanHit(ImpactHit impactHit)
    {
        if (impactHit == null)
            return true;

        if (impactHit._IsBulletHit && _IsBlockBullet)
            return false;

        if (!impactHit._IsBulletHit && _IsBlockNotBullet)
            return false;

        return true;
    }

    public override int DamageModify(int orgDamage, ImpactBase damageImpact)
    {
        if (damageImpact == null)
            return orgDamage;

        var hitImpact = damageImpact as ImpactHit;
        if(hitImpact == null)
            return orgDamage;

        if (hitImpact._IsBulletHit && _IsBlockBullet)
            return 0;

        if (!hitImpact._IsBulletHit && _IsBlockNotBullet)
            return 0;

        return orgDamage;
    }
}
