using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameBase
{
    public class AudioManager
    {
        #region 单例

        private static AudioManager m_Instance;
        public static AudioManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new AudioManager();
                }
                return m_Instance;
            }
        }

        private AudioManager() { }

        #endregion

        #region 

        private Dictionary<string, AudioSource> _AudioSources = new Dictionary<string, AudioSource>();

        public void SetAudioInfo(string name,float volume,float pitch)
        {
            if(_AudioSources.ContainsKey(name))
            {
                AudioSource audio = _AudioSources[name];
                if(audio!=null)
                {
                    audio.volume = volume;
                    audio.pitch = pitch;
                }
            }
        }

        public void PlayGlobal(string audioName)
        {
            if (_AudioSources.ContainsKey(audioName) && _AudioSources[audioName] == null)
            {
                _AudioSources.Remove(audioName);
            }

            if (!_AudioSources.ContainsKey(audioName))
            { 
                var audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
                _AudioSources.Add(audioName, audioSource);
                var res = ResourceManager.Instance.GetAudioClip(audioName);
                audioSource.clip = res;
            }
            AudioSource audio = _AudioSources[audioName];
            _AudioSources[audioName].Play();
        }

        public bool IsAudioIsPlaying(string audioName)
        {
            if (_AudioSources == null)
                return false;
            if(_AudioSources.ContainsKey(audioName))
            {
                AudioSource audio = _AudioSources[audioName];
                if (audio == null)
                    return false;
                return _AudioSources[audioName].isPlaying;
            }
            return false;
        }

        #endregion

    }
}
