using UnityEngine;
using System.Collections.Generic;

namespace KleinEngine
{
    public class SoundManager:Singleton<SoundManager>
    {
        private Dictionary<string, AudioClip> soundList = new Dictionary<string, AudioClip>();
        private AudioSource soundSource;
        private bool isMute = false;

        public void setAudioSource(AudioSource source)
        {
            soundSource = source;
        }

        public void setMute(bool flag)
        {
            isMute = flag;
        }

        public void playSound(string name, bool isStop = false)
        {
            if (isMute) return;
            if (null == soundSource) return;
            AudioClip ac = null;
            string soundName = name;
            if (!soundList.TryGetValue(soundName, out ac))
            {
                ac = ResourceManager.GetInstance().loadAsset<AudioClip>("sound", soundName);
                soundList.Add(soundName, ac);
            }
            if (isStop) soundSource.Stop();
            soundSource.PlayOneShot(ac);
        }

        public void playSound(AudioClip ac, bool isStop = false, bool isLoop = false)
        {
            if (isMute) return;
            if (null == soundSource) return;
            if (isStop) soundSource.Stop();
            if(isLoop)
            {
                soundSource.clip = ac;
                soundSource.Play();
            }
            else
                soundSource.PlayOneShot(ac);
        }
    }
}
