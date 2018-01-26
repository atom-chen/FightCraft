using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectSingle : MonoBehaviour
{

    public float _DestoryTime;

    public void Play()
    {
        gameObject.SetActive(true);
        Invoke("DestoryDelay", _DestoryTime);
    }

    private void DestoryDelay()
    {
        GameObject.Destroy(gameObject);
    }
}
