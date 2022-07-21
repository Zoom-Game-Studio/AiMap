using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using LitJson;
using System;

namespace AppLogic
{
    public class BulletScreenCommand : BaseCommand
    {
        List<string> bulletStr = new List<string>();
        GameObject bulletChatPrefab = null;
        GameObject bulletPool = null;
        string json1 = "{\"ThreeDTextList\":[{\"placeId\":\"7hJzyYaLaBt\",\"text\":\"维智科技欢迎您！\",\"lon\":121.344953,\"lat\":31.259122},{\"placeId\":\"1233\",\"lon\":121.345457,\"lat\":31.258388,\"text\":\"维智科技欢迎您2！\"},{\"placeId\":\"7hJfARLzjSQ\",\"lon\":121.344594,\"lat\":31.258819,\"text\":\"维智科技欢迎您3！\"}],\"currentLocation\":{\"lon\":121.344932,\"lat\":31.258743}}";
        string json2 = "{\"placeId\":\"7hJfARLzjSQ\",\"lon\":121.344594,\"lat\":31.258819,\"text\":\"维智科技欢迎您3！\"}";
        ThreeDText.BulletScreenInfo bulletScreenInfo;
        int bulletChatNum = 0;

        public override void onExecute(object param)
        {
            //cube = new GameObject("Cube");
            //tick = TickManager.GetInstance().createTick(1, null, TestLerp);
            //tick.start();
            Debug.Log("BulletScreenCommand...");
            bulletPool = new GameObject("BulletPool");
            bulletChatPrefab = Resources.Load<GameObject>("BulletChat");
            if (bulletChatPrefab == null) return;
            bulletStr.Add("维智科技欢迎您Wayz~~");
            addModuleEvent(ModuleEventType.GET_THREE_D_TEXT, HandleGetThreeDText);
            addModuleEvent(ModuleEventType.CREATE_A_THREE_D_TEXT, HandleCreateAThreeDText);

            //test
            //sendModuleEvent(ModuleEventType.GET_THREE_D_TEXT, json1);
            //sendModuleEvent(ModuleEventType.CREATE_A_THREE_D_TEXT, json2);
        }

        private void HandleCreateAThreeDText(EventObject ev)
        {
            Debug.Log("HandleCreateAThreeDText...");
            string json = ev.param.ToString();
            if (json == null) return;
            AddABulletChat(json);
           
            SDKManager.PhoneMethodForProcessDone("1");
        }

        private void HandleGetThreeDText(EventObject ev)
        {
            string json = ev.param.ToString();
            if (json == null) return;
            bulletScreenInfo = JsonMapper.ToObject<ThreeDText.BulletScreenInfo>(json);
            if (bulletScreenInfo == null) return;
            CreateBulletChat(bulletScreenInfo);
        }

        /// <summary>
        /// 根据收到的数据创建已有的弹幕
        /// </summary>
        void CreateBulletChat(ThreeDText.BulletScreenInfo bulletScreenInfo)
        {
            Debug.Log("CreateBulletChat...");
            if (bulletScreenInfo.ThreeDTextList == null) return;

            for (int i = 0; i < bulletScreenInfo.ThreeDTextList.Count; i++)
            {
                bulletChatNum++;
                GameObject bulletChat = GameObject.Instantiate<GameObject>(bulletChatPrefab);
                bulletChat.transform.SetParent(bulletPool.transform);
                VTextInterface vText = bulletChat.GetComponentInChildren<VTextInterface>();
                vText.RenderText = bulletScreenInfo.ThreeDTextList[i].text;
                bulletChat.transform.position = CreatPointOnSphere(bulletChatNum, 10, 5);
                bulletChat.transform.LookAt(Camera.main.transform);
            }
        }

        /// <summary>
        /// 添加一个弹幕
        /// </summary>
        void AddABulletChat(string json)
        {
            bulletChatNum++;
            Debug.Log("AddBulletChat...");
            ThreeDText.ThreeDTextListItem threeDText = JsonMapper.ToObject<ThreeDText.ThreeDTextListItem>(json);
            if (threeDText == null) return;
            GameObject bulletChat = GameObject.Instantiate<GameObject>(bulletChatPrefab);
            bulletChat.transform.SetParent(bulletPool.transform);
            VTextInterface vText = bulletChat.GetComponentInChildren<VTextInterface>();
            vText.RenderText = threeDText.text;

            BulletChatFirstCreated(bulletChat);
        }


        void BulletChatFirstCreated(GameObject bulletChatObj)
        {
            bulletChatObj.transform.LookAt(Camera.main.transform.position);
            bulletChatObj.AddComponent<BulletMovement>();
            //isTickStart = true;

            //tick.start();

        }

        public Vector3 CreatPointOnSphere(int i, int _wBornPointSum, float _dwDectRadius)
        {
            float inc = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
            float off = 2.0f / _wBornPointSum;   //注意保持数值精度  m_wBornPointSum：生成的点数
            float y;
            float r;
            float phi;

            y = (float)i * off + (off / 2.0f) - 1.0f;
            r = Mathf.Sqrt(1.0f - y * y);
            phi = i * inc;
            Vector3 pos = new Vector3(Mathf.Cos(phi) * r * _dwDectRadius, y * _dwDectRadius, Mathf.Sin(phi) * r * _dwDectRadius); //m_dwDectRadius  距离球心的距离
            return pos;
        }

        public void CreatPointOnSphere(int _wBornPointSum, float _dwDectRadius)
        {
            int tempIndex = 0;
            //生成
            float inc = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
            float off = 2.0f / _wBornPointSum;   //注意保持数值精度  m_wBornPointSum：生成的点数
            float y;
            float r;
            float phi;
            for (int i = 0; i < _wBornPointSum; i++)
            {
                y = (float)i * off + (off / 2.0f) - 1.0f;
                r = Mathf.Sqrt(1.0f - y * y);
                phi = i * inc;
                Vector3 pos = new Vector3(Mathf.Cos(phi) * r * _dwDectRadius, y * _dwDectRadius, Mathf.Sin(phi) * r * _dwDectRadius); //m_dwDectRadius  距离球心的距离
                tempIndex++;
                BornPoint(pos, tempIndex);
            }
        }

        void BornPoint(Vector3 pos, int tempIndex)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            obj.transform.localPosition = pos;
            obj.name = string.Concat("Cube", tempIndex);
        }
    }
}
