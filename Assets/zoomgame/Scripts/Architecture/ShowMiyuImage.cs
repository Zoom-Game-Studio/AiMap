using System;
using System.IO;
using AppLogic;
using BestHTTP;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace zoomgame.Scripts.Architecture
{
    public class ShowMiyuImage : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject saveInfoPanel;

        [SerializeField]
        private string url = "https://aimap.newayz.com/aimap/lantern_riddle/lantern-riddle-auth-image/002.jpg";
        [SerializeField] private Image img;
        [SerializeField] private Text info;
        [SerializeField] private Button btn_save;
        private byte[] data;

        private void Start()
        {
            MessageBroker.Default.Receive<GuessMiyuComplete>().Subscribe(OnGuessComplete).AddTo(this);
            btn_save.onClick.AddListener(SaveToPhone);
        }

        void OnGuessComplete(GuessMiyuComplete evt)
        {
            var r = new HTTPRequest(new Uri(evt.miyu.imageUrl), HTTPMethods.Get, OnGetImage);
            r.Send();
        }

        [Button]
        void Test()
        {
            var r = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnGetImage);
            r.Send();
        }

        private void OnGetImage(HTTPRequest r, HTTPResponse p)
        {
            if (r != null && r.State == HTTPRequestStates.Finished)
            {
                var texture = p.DataAsTexture2D;
                data = p.Data;
                img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                panel.SetActive(true);
            }

            r?.Dispose();
            p.Dispose();
        }

        void CloseUI()
        {
            panel.SetActive(false);
            saveInfoPanel.SetActive(false);
        }

        void SaveToPhone()
        {
            if (data == null)
            {
                throw new NullReferenceException("要保存的图片不能为空");
            }

            try
            {
                var now = DateTime.Now;
                string filename = string.Format("猜灯谜{0}{1}{2}{3}.png", now.Day, now.Hour, now.Minute, now.Second);
                // var destination = "/mnt/sdcard/DCIM/ArMap";
                // var destination = Application.persistentDataPath;
                // if (!Directory.Exists(destination))
                // {
                //     Directory.CreateDirectory(destination);
                //     Debug.LogWarning("新建文件夹：" + destination);
                // }
                //
                // var path = Path.Combine(destination, filename);
                // Debug.Log(path);
                // using (var fs = new FileStream(path, FileMode.Create))
                // {
                //     fs.Write(data);
                // }
                SaveImageToGallery(data, filename, String.Empty);
                info.text = "保存完成";
            }
            catch (Exception e)
            {
                info.text = e.Message;
            }
            saveInfoPanel.SetActive(true);
            Invoke(nameof(CloseUI), 1);
        }

        private void OnDisable()
        {
            data = null;
        }
        
        #region 保存图片到手机相册


        public static AndroidJavaObject _activity;
        public static AndroidJavaObject Activity
        {
            get
            {
                if (_activity == null)
                {
                    var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _activity;
            }
        }
        
        private const string MediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
        public static string SaveImageToGallery(Texture2D texture2D, string title, string description)
        {
            using (var mediaClass = new AndroidJavaClass(MediaStoreImagesMediaClass))
            {
                using (var cr = Activity.Call<AndroidJavaObject>("getContentResolver"))
                {
                    var image = Texture2DToAndroidBitmap(texture2D);
                    var imageUrl = mediaClass.CallStatic<string>("insertImage", cr, image, title, description);
                    return imageUrl;
                }
            }
        }
        
        public static void  SaveImageToGallery(byte[] data, string title, string description)
        {
            using (var mediaClass = new AndroidJavaClass(MediaStoreImagesMediaClass))
            {
                using (var cr = Activity.Call<AndroidJavaObject>("getContentResolver"))
                {
                    using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
                    {
                        var image =  bf.CallStatic<AndroidJavaObject>("decodeByteArray", data, 0, data.Length);
                        mediaClass.CallStatic<string>("insertImage", cr, image, title, description);
                    }
                }
            }
        }
  
        public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D texture2D)
        {
            byte[] encoded = texture2D.EncodeToPNG();
            using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
            {
                return bf.CallStatic<AndroidJavaObject>("decodeByteArray", encoded, 0, encoded.Length);
            }
        }
        #endregion
    }
}