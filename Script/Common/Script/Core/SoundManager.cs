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

    public void PlayBGMusic(AudioClip _LogicAudio)
    {
        _AudioSource.PlayOneShot(_LogicAudio);
    }

    public void PlayEffectSound(AudioClip soundEffect)
    {

    }

}
