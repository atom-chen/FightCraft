using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcePool : InstanceBase<ResourcePool>
{

    void Start()
    {
        SetInstance(this);
    }

    void Destory()
    {
        SetInstance(null);
    }

    #region effect

    public EffectController[] _CommonHitEffect;

    private Dictionary<string, Stack<EffectController>> _IdleEffects = new Dictionary<string, Stack<EffectController>>();

    public EffectController GetIdleEffect(EffectController effct)
    {
        EffectController idleEffect = null;
        if (_IdleEffects.ContainsKey(effct.name))
        {
            if (_IdleEffects[effct.name].Count > 0)
            {
                idleEffect = _IdleEffects[effct.name].Pop();
            }
        }

        if (idleEffect == null)
        {
            idleEffect = GameObject.Instantiate<EffectController>(effct);
        }

        return idleEffect;
    }

    public void RecvIldeEffect(EffectController effct)
    {
        string effectName = effct.name.Replace("(Clone)", "");
        if (!_IdleEffects.ContainsKey(effectName))
        {
            _IdleEffects.Add(effectName, new Stack<EffectController>());
        }
        effct.transform.SetParent(transform);
        _IdleEffects[effectName].Push(effct);
    }

    public void ClearEffects()
    {
        _IdleEffects = new Dictionary<string, Stack<EffectController>>();
    }

    public void PlaySceneEffect(EffectController effct, Vector3 position, Vector3 rotation)
    {
        var effectInstance = GetIdleEffect(effct);
        effectInstance.transform.SetParent(transform);
        effectInstance.transform.position = position;
        effectInstance.transform.rotation = Quaternion.Euler(rotation);
        effectInstance.PlayEffect();
    }
    #endregion

    #region bullet

    private Dictionary<string, Stack<BulletBase>> _IdleBullets = new Dictionary<string, Stack<BulletBase>>();

    public BulletBase GetIdleBullet(BulletBase bullet)
    {
        BulletBase idleBullet = null;
        if (_IdleBullets.ContainsKey(bullet.name))
        {
            if (_IdleBullets[bullet.name].Count > 0)
            {
                idleBullet = _IdleBullets[bullet.name].Pop();
            }
        }

        if (idleBullet == null)
        {
            idleBullet = GameObject.Instantiate<BulletBase>(bullet);
        }

        return idleBullet;
    }

    public void RecvIldeBullet(BulletBase bullet)
    {
        string BulletName = bullet.name.Replace("(Clone)", "");
        if (!_IdleBullets.ContainsKey(BulletName))
        {
            _IdleBullets.Add(BulletName, new Stack<BulletBase>());
        }
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
        _IdleBullets[BulletName].Push(bullet);
    }

    public void ClearBullets()
    {
        _IdleBullets = new Dictionary<string, Stack<BulletBase>>();
    }

    #endregion

    #region motion object

    private Dictionary<string, Stack<MotionManager>> _IdleObject = new Dictionary<string, Stack<MotionManager>>();

    public MotionManager GetIdleMotion(string motionName)
    {
        MotionManager idle = null;
        if (_IdleBullets.ContainsKey(motionName))
        {
            if (_IdleBullets[motionName].Count > 0)
            {
                idle = _IdleObject[motionName].Pop();
                idle.Reset();
            }
        }

        if (idle == null)
        {
            var obj = GameBase.ResourceManager.Instance.GetInstanceGameObject("ModelBase/" + motionName);
            idle = obj.GetComponent<MotionManager>();
        }

        return idle;
    }

    public void RecvIldeMotion(MotionManager objMotion)
    {
        string objName = objMotion.name.Replace("(Clone)", "");
        if (!_IdleObject.ContainsKey(objName))
        {
            _IdleObject.Add(objName, new Stack<MotionManager>());
        }
        objMotion.gameObject.SetActive(false);
        objMotion.transform.SetParent(transform);
        _IdleObject[objName].Push(objMotion);
    }

    public void ClearObjs()
    {
        _IdleObject = new Dictionary<string, Stack<MotionManager>>();
    }

    #endregion

}
