using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using LitJson;

namespace AppLogic
{
    public class RenCaiGangButtonCtrl : MonoBehaviour
    {
        public GameObject[] others;

        private string file1 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/2l/2019%E5%B9%B4%E7%A6%BB%E5%B2%B8%E5%9F%BA%E5%9C%B0-%E5%A4%AA%E5%BA%93%E6%94%BF%E7%AD%96%E5%AE%A3%E8%AE%B2%E6%B4%BB%E5%8A%A8%E5%BE%AE%E4%BF%A1%E9%93%BE%E6%8E%A5-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390444&Signature=OJCL55BbwzNWnYl65ic0H9PXE9g%3D";
        private string file2 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/2l/6%E6%9C%88%E7%A6%BB%E5%B2%B8%E5%9F%BA%E5%9C%B0%E4%B8%8E%E6%B5%B7%E5%86%85%E5%A4%96%E9%AB%98%E6%A0%A1%E5%88%9B%E6%96%B0%E5%88%9B%E4%B8%9A%E6%9C%BA%E6%9E%84%E7%A7%AF%E6%9E%81%E5%BC%80%E5%B1%95%E4%BA%A4%E6%B5%81%E4%B8%8E%E5%90%88%E4%BD%9C-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390474&Signature=%2B5NkTVAkbTAC7CdmVHYRpGVSbBY%3D";
        private string file3 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/2l/CAIRDC%E6%8A%A5%E9%81%93-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390493&Signature=uNbnnD7%2BjTZBUkrfkiCI42q2F1Q%3D";
        private string file4 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/2l/%E6%B4%BB%E5%8A%A8%E6%8A%A5%E9%81%93-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390509&Signature=vOVc0WM1V9jdgZbYdhjJ43JLxUQ%3D";

        private string file5 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/3l/7-%E6%B4%BB%E5%8A%A8%E5%9B%9E%E9%A1%BE%20_%20%E5%BC%A0%E6%B1%9F%E5%9B%BD%E9%99%85%E4%BA%BA%E5%B7%A5%E6%99%BA%E8%83%BD%E6%8C%91%E6%88%98%E8%B5%9B%E9%A6%96%E5%9C%BA%E9%98%B6%E6%AE%B5%E8%B5%9B%E8%90%BD%E4%B8%8B%E5%B8%B7%E5%B9%95-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390531&Signature=7ZHRen4KbvJHLJfshIq9xm9yF6g%3D";
        private string file6 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/3l/8-%E6%B4%BB%E5%8A%A8%E5%9B%9E%E9%A1%BE%20_%20%E2%80%9C%E4%BA%AE%E5%93%81%E7%89%8C%E2%80%9D%EF%BC%8C%E2%80%9C%E6%99%92%E4%BA%A7%E5%93%81%E2%80%9D%EF%BC%8C%E6%B5%A6%E4%B8%9C%E4%BC%81%E4%B8%9A%E5%8F%91%E5%B8%83%E6%B4%BB%E5%8A%A8%E6%88%90%E5%8A%9F%E4%B8%BE%E5%8A%9E-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390547&Signature=96SmdDoe%2BUFlPG6wdjV200jRCF4%3D";
        private string file7 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/3l/%E5%BC%A0%E6%B1%9F%E5%A4%B4%E6%9D%A1%20%E6%99%9A%E9%A4%90%E4%BC%9A%20%E6%8A%A5%E9%81%93-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390566&Signature=Ht6u3SlitmOx1hKE4B2SIY0vcpA%3D";
        private string file8 = "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrencaigang/3l/%E6%B1%9F%E8%A5%BF%E4%BA%BA%E5%9C%A8%E5%BC%A0%E6%B1%9F%EF%BC%81%E2%80%9C%E5%90%8C%E9%A5%AE%E8%B5%A3%E6%B1%9F%E6%B0%B4%EF%BC%8C%E5%85%B1%E5%8F%99%E5%BC%A0%E6%B1%9F%E6%83%85%E2%80%9D%E5%BC%A0%E6%B1%9F%E4%B9%8B%E5%A4%9C%E5%AE%8C%E6%BB%A1%E8%90%BD%E5%B9%95-1.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705390580&Signature=xEAJP8ASGnK3nafbWo8rb3la7uQ%3D";



        public void HideOthersClick()
        {
            foreach (var item in others)
            {
                item.SetActive(false);
            }
        }
        public void OpenFile1Clicked()
        {
            OpenUrlFromUnityClicked("2", file1);
        }
        public void OpenFile2Clicked()
        {
            OpenUrlFromUnityClicked("2", file2);
        }
        public void OpenFile3Clicked()
        {
            OpenUrlFromUnityClicked("2", file3);
        }
        public void OpenFile4Clicked()
        {
            OpenUrlFromUnityClicked("2", file4);
        }
        public void OpenFile5Clicked()
        {
            OpenUrlFromUnityClicked("2", file5);
        }
        public void OpenFile6Clicked()
        {
            OpenUrlFromUnityClicked("2", file6);
        }
        public void OpenFile7Clicked()
        {
            OpenUrlFromUnityClicked("2", file7);
        }
        public void OpenFile8Clicked()
        {
            OpenUrlFromUnityClicked("2", file8);
        }

        public void OpenUrlFromUnityClicked(string type, string url)
        {
            Debug.Log("OpenUrlFromUnityClicked...");
            UrlFromUnity urlFromUnity = new UrlFromUnity();
            urlFromUnity.type = type;
            urlFromUnity.url = url;
            string json = JsonMapper.ToJson(urlFromUnity);
            SDKManager.PhoneMehtodForOpenUrlFromUnity(json);
        }
    }
}
