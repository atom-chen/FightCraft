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
        effct.transform.SetParent(null);
        _IdleEffects[effectName].Push(effct);
    }

    public void ClearEffects()
    {
        _IdleEffects = new Dictionary<string, Stack<EffectController>>();
    }

    #endregion

}
