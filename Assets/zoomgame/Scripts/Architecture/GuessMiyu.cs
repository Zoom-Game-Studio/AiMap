using System;
using System.Linq.Expressions;
using AppLogic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace zoomgame.Scripts.Architecture
{
    public class GuessMiyu : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text puzzle;
        [SerializeField] private InputField answer;
        [SerializeField] private MiYu _currnetMiyu;
        [SerializeField] private Button btn_guess;
        [SerializeField] private Button btn_close;
        private void Start()
        {
            MessageBroker.Default.Receive<GuessEventOpenUi>().Subscribe(OpenUI).AddTo(this);
            btn_guess.onClick.AddListener(OnGuessMiyu);
            btn_close.onClick.AddListener(CloseUi);
        }

        [SerializeField] private string _answer;

        void OnGuessMiyu()
        {
            if (_currnetMiyu != null)
            {
                // if (_currnetMiyu.answer.Equals(_answer))
                if (_currnetMiyu.answer.Equals(answer.text))
                {
                    MessageBroker.Default.Publish(new GuessMiyuComplete()
                    {
                        miyu = _currnetMiyu
                    });
                    CloseUi();
                }
                else
                {
                    MakeToast("不对，再来一次");
                    Debug.Log("谜底是:" + _currnetMiyu?.answer);
                }
            }
            else{
                Debug.Log("谜语为空");
            }
        }

        void OpenUI(GuessEventOpenUi evt)
        {
            if (evt.miyu == null)
            {
                throw new ArgumentNullException("谜语不能为空");
            }
            else
            {
                _currnetMiyu = evt.miyu;
                puzzle.text = _currnetMiyu.puzzle;
                panel.SetActive(true);
            }
        }

        void CloseUi()
        {
            panel.SetActive(false);
            _currnetMiyu = null;
            answer.text = null;
        }
        
        // Unity调用安卓的土司
        public static void MakeToast(string info)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                Toast.CallStatic<AndroidJavaObject>("makeText", currentActivity, info, Toast.GetStatic<int>("LENGTH_LONG")).Call("show");
            }));
    
            /*
            // 匿名方法中第二个参数是安卓上下文对象，除了用currentActivity，还可用安卓中的GetApplicationContext()获得上下文。
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            */
        }



        

    }
}