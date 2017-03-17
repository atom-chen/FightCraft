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
        _LastPlaySpeed = 1;
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
            if (_Particles == null || _Particles.Length == 0)
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

    public virtual void PlayEffect(Hashtable hash)
    {
        PlayEffect();
    }

    public virtual void HideEffect()
    {
        gameObject.SetActive(false);
    }

    public virtual void PauseEffect()
    {
        if (!gameObject.activeSelf)
            return;

        if (_Particles == null || _Particles.Length == 0)
            _Particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        foreach (var particle in _Particles)
        {
            particle.Pause();
        }
        Debug.Log("PauseEffect");
    }

    public virtual void ResumeEffect()
    {
        if (!gameObject.activeSelf)
            return;

        if (_Particles == null)
            _Particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        foreach (var particle in _Particles)
        {
            particle.Play();
        }
        Debug.Log("ResumeEffect");
    }

    #endregion

    #region no Instance Effect

    public string _BindPos;
    public float _EffectLastTime;
    
    #endregion

}
