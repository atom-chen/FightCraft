using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectQiLinIceBuff : EffectController
{
    public void Update()
    {
        UpdateRot();
    }

    #region effect
    
    public GameObject _EffectSingle;
    public float _Distance;
    public float _RotSpeed;

    private List<GameObject> _PlayingEffect = new List<GameObject>();
    private float _WorldRotY = 0;

    public void PlayEffectCnt(int count)
    {
        float angle = 360 / count;
        for (int i = count; i > 0; --i)
        {
            var newEffectInstance = GameObject.Instantiate(_EffectSingle);
            newEffectInstance.transform.SetParent(transform);
            newEffectInstance.transform.localPosition = Vector3.zero;
            var rot = new Vector3(0, newEffectInstance.transform.rotation.eulerAngles.y + angle * i, 0);
            newEffectInstance.transform.rotation = Quaternion.Euler(rot);
            Vector3 pos = newEffectInstance.transform.forward * _Distance;
            pos.y = _EffectSingle.transform.localPosition.y;
            newEffectInstance.transform.localPosition += pos;
            newEffectInstance.SetActive(true);

            _PlayingEffect.Add(newEffectInstance);
        }
    }

    public int DecEffect()
    {
        if (_PlayingEffect.Count > 0)
        {
            GameObject.Destroy(_PlayingEffect[0]);
            _PlayingEffect.RemoveAt(0);

            return _PlayingEffect.Count;
        }
        return _PlayingEffect.Count;
    }

    public void UpdateRot()
    {
        _WorldRotY += _RotSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(new Vector3(0, _WorldRotY, 0));
    }

    #endregion
}
