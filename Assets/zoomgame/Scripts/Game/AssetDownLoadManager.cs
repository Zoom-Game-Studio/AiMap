using System;
using Architecture;
using BestHTTP;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WeiXiang
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class AssetDownLoadManager : AbstractMonoController
    {
        public string url = @"https://aimap.newayz.com/aimap/ora/v1/scenes?device_type=android";
        public string token = "ade820503cc6069ff507346a6ef7fec3";
        
        [Button]
        private void StartRequest()
        {
            //查询自己的gps坐标
            // this.SendQuery<PriorTranslation>("");

            var request = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnFinishCallback);
            request.Send();
        }

        private void OnFinishCallback(HTTPRequest request, HTTPResponse response)
        {
            if (request!= null && response != null && request.State == HTTPRequestStates.Finished)
            {
                var data = response.DataAsText;
                Debug.Log(data);
            }
            else
            {
                WeiXiang.Console.Error("Request asset config error!");
            }
        }
    }
}