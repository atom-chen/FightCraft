using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectController : MonoBehaviour
{
    
    #region Binded Effect

    private float _LastPlaySpeed = 1;
    private ParticleSystem[] _Particles;

    public virtual void PlayEffect()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public virtual void PlayEffect(float speed)
    {
        if (speed != _LastPlaySpeed)
        {
            _LastPlaySpeed = speed;
            if (_Particles == null)
                _Particles = gameObject.GetComponentsInChildren<ParticleSystem>();

            foreach (var particle in _Particles)
            {
                particle.playbackSpeed = speed;
            }
        }
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public virtual void HideEffect()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region no Instance Effect

    public string _BindPos;
    public float _EffectLastTime;

    private static Dictionary<string, Stack<EffectController>> _IdleEffects = new Dictionary<string, Stack<EffectController>>();

    public static EffectController GetIdleEffect(EffectController effct)
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

    public static void RecvIldeEffect(EffectController effct)
    {
        string effectName = effct.name.Replace("(Clone)", "");
        if (!_IdleEffects.ContainsKey(effectName))
        {
            _IdleEffects.Add(effectName, new Stack<EffectController>());
        }

        _IdleEffects[effectName].Push(effct);
    }

    public static void ClearEffects()
    {
        _IdleEffects = new Dictionary<string, Stack<EffectController>>();
    }
    #endregion

}
