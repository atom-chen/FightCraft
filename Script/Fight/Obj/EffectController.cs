using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour
{
    public float _LastTime = 0.0f;

    private float _LastPlaySpeed = 1;
    private ParticleSystem[] _Particles;

    public void PlayEffect()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        if(_LastTime > 0)
            StartCoroutine(LastEffect());
    }

    public void PlayEffect(float speed)
    {
        if (speed != _LastPlaySpeed)
        {
            _LastPlaySpeed = speed;
            if(_Particles == null)
                _Particles = gameObject.GetComponentsInChildren<ParticleSystem>();

            foreach (var particle in _Particles)
            {
                particle.playbackSpeed = speed;
            }
        }
        gameObject.SetActive(true);
        StopAllCoroutines();
        if (_LastTime > 0)
            StartCoroutine(LastEffect());
    }


    public IEnumerator LastEffect()
    {
        yield return new WaitForSeconds(_LastTime / _LastPlaySpeed);
        HideEffect();
    }

    public void HideEffect()
    {
        gameObject.SetActive(false);
    }
}
