using System;
using System.IO;
using System.Net;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace C_ScriptsTest
{
    public class TestCamera : MonoBehaviour
    {
        [SerializeField] private string url = "192.168.110.244:8686/logcat.txt";
        [SerializeField] private string filePath = @"E:\ZoomGameStudio\AIMAP\Assets\StreamingAssets\logcat.txt";

        [Button]
        void Test()
        {
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            using var webResponse = request.GetResponse();
            foreach (string webResponseHeader in webResponse.Headers)
            {
                Debug.Log(webResponseHeader);   
            }
            using var stream = webResponse.GetResponseStream();
            using var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();
            Debug.Log(str);
            // var stream = webResponse.GetResponseStream();
            // byte[] buffer = new byte[1024];
            // var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            // int length = stream.Read(buffer, 0, buffer.Length);
            // while (length > 0)
            // {
            //     //将内容再写入本地文件中
            //     fs.Write(buffer, 0, length);
            //     //类似尾递归
            //     length = stream.Read(buffer, 0, buffer.Length);
            // }
            // webResponse.Close();
            // stream.Dispose();
            // fs.Dispose();
            // request.Abort();
            //server list
            //client list
            //break list
            //download list
        }
    }
}