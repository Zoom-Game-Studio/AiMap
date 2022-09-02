using System;
using BestHTTP;
using BestHTTP.Forms;
using QFramework;
using HttpData;
using UnityEngine;
using WeiXiang;
using zoomgame.Scripts.Game;
using Console = WeiXiang.Console;

namespace Architecture
{
    /// <summary>
    /// 图像识别定位
    /// </summary>
    public class VisionLocation : ICanLocation
    {
        private string Token => _model.Token;
        private string Url => _model.Url;
        private ILocationModel _model;
        private HTTPFormUsage formUsage = HTTPFormUsage.Multipart;

        public void BindData(ILocationModel locationModel)
        {
            this._model = locationModel;
        }

        public void RequestLocation(string base64Image, Action<HTTPRequestStates, string> onFinishRequest)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                Debug.LogError("unrecognizable base 64 string,can not be null or empty!");
                return;
            }

            try
            {

                
                var bodyData = new BodyData()
                {
                    timestamp = BodyData.GetUTCTimeStamp(),
                    intrinsic = Float4.intrinsic,
                    prior_translation = PriorTranslation.GetGps(),
                    prior_rotation = PriorRotation.GetRotation(),
                    orientation = 3,
                    distortion = new Float4(),
                    UUID = SystemInfo.deviceUniqueIdentifier,
                    // UUID = "cf0cbc56-a3c9-4807-a125-796faf91c7ff"
                };
                var dataValue = DataMapper.ToJson(bodyData);
                Console.Warning(dataValue);
                RequestLocation(Url, Token, dataValue, base64Image,
                    (r, p) => { OnFinishRequest(r, p, onFinishRequest); });
            }
            catch (Exception e)
            {
                Console.Error($"Error:{e.Message}!");
                onFinishRequest?.Invoke(HTTPRequestStates.Error, null);
            }
        }

        public void RequestLocation(string image, Float4 intrinsic, PriorTranslation gps, int orientation,
            Action<HTTPRequestStates, string> onFinishRequest)
        {
            var bodyData = new BodyData()
            {
                timestamp = BodyData.GetUTCTimeStamp(),
                intrinsic = intrinsic,
                prior_translation = gps,
                prior_rotation = PriorRotation.GetRotation(),
                orientation = orientation,
                distortion = new Float4(),
                UUID = SystemInfo.deviceUniqueIdentifier,
                // UUID = "cf0cbc56-a3c9-4807-a125-796faf91c7ff"
            };
            var dataValue = DataMapper.ToJson(bodyData);
            RequestLocation(Url, Token, dataValue, image, (r, p) => { OnFinishRequest(r, p, onFinishRequest); });
        }

        public void RequestLocation(string url, string token, string data, string image,
            OnRequestFinishedDelegate onFinishRequest)
        {
            var request = new HTTPRequest(new Uri(url), HTTPMethods.Post, onFinishRequest);
            request.AddHeader("token", Token);
            request.AddField("data", data);
            Debug.Log(data);
            request.AddField("image", image);
            request.FormUsage = formUsage;
            request.Send();
        }


        public void OnFinishRequest(HTTPRequest request, HTTPResponse response,
            Action<HTTPRequestStates, string> callback)
        {
            Debug.LogWarning($"{request?.State}: {response?.DataAsText}");
            var requestStates = request?.State ?? HTTPRequestStates.Error;
            callback?.Invoke(requestStates, response?.DataAsText);
        }
    }

    public interface ICanLocation : IUtility
    {
        /// <summary>
        /// 请求定位
        /// </summary>
        /// <param name="base64Image"></param>
        /// <param name="onFinishRequest">操作结束的回调</param>
        void RequestLocation(string base64Image, Action<HTTPRequestStates, string> onFinishRequest);

        /// <summary>
        /// 请求定位
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <param name="image"></param>
        /// <param name="onFinishRequest"></param>
        void RequestLocation(string url, string token, string data, string image,
            OnRequestFinishedDelegate onFinishRequest);

        /// <summary>
        /// 请求定位
        /// </summary>
        /// <param name="image"></param>
        /// <param name="intrinsic">相机物理参数</param>
        /// <param name="gps">gps</param>
        /// <param name="orientation">手机朝向</param>
        /// <param name="onFinishRequest"></param>
        void RequestLocation(string image, Float4 intrinsic, PriorTranslation gps, int orientation,
            Action<HTTPRequestStates, string> onFinishRequest);

        /// <summary>
        /// 请求结束
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="callback"></param>
        void OnFinishRequest(HTTPRequest request, HTTPResponse response, Action<HTTPRequestStates, string> callback);
    }
}