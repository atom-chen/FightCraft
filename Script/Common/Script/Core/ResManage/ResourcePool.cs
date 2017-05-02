using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcePool : InstanceBase<ResourcePool>
{

    void Start()
    {
        SetInstance(this);
        InitEffect();
    }

    void Destory()
    {
        SetInstance(null);
    }

    #region effect

    public List<EffectController> _CommonHitEffect;

    private Dictionary<string, Stack<EffectController>> _IdleEffects = new Dictionary<string, Stack<EffectController>>();

    private void InitEffect()
    {
        _CommonHitEffect = new List<EffectController>();
        var effect1 = GameBase.ResourceManager.Instance.GetGameObject("Effect/Hit/Effect_Dead_A_Hit");
        _CommonHitEffect.Add(effect1.GetComponent<EffectController>());

        var effect2 = GameBase.ResourceManager.Instance.GetGameObject("Effect/Hit/Effect_Dead_B_Hit");
        _CommonHitEffect.Add(effect2.GetComponent<EffectController>());

        var effect3 = GameBase.ResourceManager.Instance.GetGameObject("Effect/Hit/Effect_Blade_Red");
        _CommonHitEffect.Add(effect3.GetComponent<EffectController>());
    }

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
        idle.gameObject.SetActive(true);
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

    #region ui

    private Dictionary<string, Stack<UIItemBase>> _IdleUIItems = new Dictionary<string, Stack<UIItemBase>>();

    public T GetIdleUIItem<T>(UIItemBase itemPrefab)
    {
        UIItemBase idleItem = null;
        if (_IdleUIItems.ContainsKey(itemPrefab.name))
        {
            if (_IdleUIItems[itemPrefab.name].Count > 0)
            {
                idleItem = _IdleUIItems[itemPrefab.name].Pop();
            }
        }

        if (idleItem == null)
        {
            idleItem = GameObject.Instantiate<UIItemBase>(itemPrefab);
        }

        return idleItem.GetComponent<T>();
    }

    public void RecvIldeUIItem(UIItemBase itemBase)
    {
        string itemName = itemBase.name.Replace("(Clone)", "");
        if (!_IdleUIItems.ContainsKey(itemName))
        {
            _IdleUIItems.Add(itemName, new Stack<UIItemBase>());
        }
        itemBase.gameObject.SetActive(false);
        //itemBase.transform.SetParent(transform);
        _IdleUIItems[itemName].Push(itemBase);
    }

    public void ClearUIItems()
    {
        _IdleUIItems = new Dictionary<string, Stack<UIItemBase>>();
    }

    #endregion

}
