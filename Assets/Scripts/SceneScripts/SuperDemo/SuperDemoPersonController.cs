using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.Events;

namespace AppLogic
{
    public class SuperDemoPersonController : MonoBehaviour
    {
        bool isPlayed = false;
        string previousName = null;
        AudioSource previousAudio = null;
        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.SUPERDEMO_GUIDE_TRIGGER, HandleSuperDemoGuideTrigger);
        }

        private void HandleSuperDemoGuideTrigger(EventObject ev)
        {
            Transform waypointTran = ev.param as Transform;
            if (waypointTran == null) return;
            transform.position = waypointTran.position;
            transform.rotation = waypointTran.rotation;
            PlayWaypointVoice(waypointTran);
        }

        void PlayWaypointVoice(Transform trans)
        {
            if (trans.name.Equals(previousName)) return;
            previousName = trans.name;
            AudioSource audio = trans.GetComponent<AudioSource>();
            if (audio == null) return;

            if (previousAudio != null)
            {
                if (previousAudio.clip == audio.clip) return;
                previousAudio.Stop();
                //Debug.Log("Stop previous audio...");
            }
            previousAudio = audio;
            //Debug.Log("PlayWaypointVoice...");
            audio.Stop();
            if(trans.name.Equals("5"))
            {
                PlayCoffeShopAudio(audio);
                return;
            }
            if (trans.name.Equals("16"))
            {
                PlayAIMapAudio(audio);
                return;
            }
            if (trans.name.Equals("8"))
            {
                PlayAutoDriveAudio(audio);
                return;
            }
            audio.Play();
        }

        public void PlayCoffeShopAudio(AudioSource audio, UnityAction callback = null, bool isLoop = false)
        {

            //获取自身音频文件进行播放并且不重复播放

            audio.loop = isLoop;

            audio.Play();

            //执行协成获取音频文件的时间

            StartCoroutine(AudioCoffeShopFinished(25, callback));

        }

        private IEnumerator AudioCoffeShopFinished(float time, UnityAction callback)
        {

            yield return new WaitForSeconds(time);

            //声音播放完毕后之下往下的代码 

            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SHOW_COFFE_SHOP_MENU);

            #endregion

        }


        //接受音频文件和是否重复播放

        public void PlayAIMapAudio(AudioSource audio, UnityAction callback = null, bool isLoop = false)
        {

            //获取自身音频文件进行播放并且不重复播放

            audio.loop = isLoop;

            audio.Play();

            //执行协成获取音频文件的时间

            StartCoroutine(AudioPlayAIMapFinished(audio.clip.length, callback));

        }

        //执行协成函数 并且返回时间

        private IEnumerator AudioPlayAIMapFinished(float time, UnityAction callback)
        {

            yield return new WaitForSeconds(time);

            //声音播放完毕后之下往下的代码 

            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_PLAY_AIMAPGROUP);

            #endregion

        }

        public void PlayAutoDriveAudio(AudioSource audio, UnityAction callback = null, bool isLoop = false)
        {

            //获取自身音频文件进行播放并且不重复播放

            audio.loop = isLoop;

            audio.Play();

            //执行协成获取音频文件的时间

            StartCoroutine(AudioAutoDriveFinished(audio.clip.length, callback));

        }

        private IEnumerator AudioAutoDriveFinished(float time, UnityAction callback)
        {

            yield return new WaitForSeconds(time);

            //声音播放完毕后之下往下的代码 

            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_SUPERDEMO_CAR);

            #endregion

        }
    }
}
