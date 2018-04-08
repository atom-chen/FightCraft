using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource _AudioSource;
    public AudioClip _LogicAudio;

    public void PlayBGMusic(string music)
    {
        var audio = ResourceManager.Instance.GetAudioClip(music);
        PlayBGMusic(audio);
    }

    public void PlayBGMusic(AudioClip logicAudio)
    {
        _AudioSource.clip = (logicAudio);
        _AudioSource.Play();
    }

    public void PlayEffectSound(AudioClip soundEffect)
    {

    }

}
