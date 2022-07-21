using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using LitJson;
using UnityEngine.UI;
using ARLocation;

namespace AppLogic
{
    public class AIMapPOICommand : BaseCommand
    {
        public string json = "{\"POIList\":[{\"placeId\":\"7hJzyYaLaBt\",\"name\":\"智尚酒店\",\"lon\":121.6316,\"lat\":31.189223,\"hight\":null,\"distance\":200,\"type\":\"Tower\",\"classId\":20000,\"className\":\"企业商务\"},{\"placeId\":\"7hJzyYaLaB2\",\"name\":\"便民百货\",\"lon\":121.634154,\"lat\":31.189535,\"hight\":null,\"distance\":100,\"type\":\"Tower\",\"classId\":20000,\"className\":\"企业商务\"},{\"placeId\":\"7hJfARLzjSQ\",\"name\":\"黄山菜饭骨头汤\",\"lon\":121.635237,\"lat\":31.18971,\"hight\":null,\"distance\":300,\"type\":\"Tower\",\"classId\":20000,\"className\":\"企业商务\"}],\"currentLocation\":{\"lon\":121.635114,\"lat\":31.188086}}";
        public static Location currentLocation = null;
        //public string json = "{\"POIList\":[{\"placeId\":\"7hJzyYaLaBt\",\"name\":\"申江花苑\",\"lon\":121.340892,\"lat\":31.259101,\"hight\":null,\"distance\":200,\"type\":\"Tower\",\"classId\":20000,\"className\":\"企业商务\"},{\"placeId\":\"嘉年华商场\",\"lon\":121.345291,\"lat\":31.260587,\"hight\":null,\"distance\":100,\"type\":\"Tower\",\"classId\":20000,\"className\":\"企业商务\"},{\"placeId\":\"7hJfARLzjSQ\",\"name\":\"江桥佳苑\",\"lon\":121.343811,\"lat\":31.256203,\"hight\":null,\"distance\":300,\"type\":\"Tower\",\"classId\":20000,\"className\":\"企业商务\"}],\"currentLocation\":{\"lon\":121.344857,\"lat\":31.258711}}";
        //public string json = "[{\"buidingGPS\":{\"lat\":31.177417,\"lon\":121.604488},\"buidingLocation\":{\"x\":3,\"y\":10,\"z\":30},\"detail\":\"盛趣大楼\",\"otherInfo\":{\"注册号\":\"310141000356839\",\"招聘信息\":\"研发、美术、策划\",\"企业经营状态\":\"开业\",\"企业联系电话\":\"021-50504740\",\"在职员工人数\":\"9999/人\",\"名称\":\"盛趣科技\",\"统一社会信用代码\":\"91310115MA1K3K4A54\",\"融资信息\":\"2019年01月07日，世纪华通以298亿并购盛跃网络\"},\"title\":\"海趣园\"}]";
        public GameObject worldRootPOI;
        List<POIInfoInUnity> buildingInfosInMapList = new List<POIInfoInUnity>();
        List<POIInfoInUnity> buildingInfosInUnity;
        List<GameObject> poiObjList = new List<GameObject>();
        GameObject poiPrefab;
        WayzPOI wayzPOI;
        GameObject compassObj;
        ARLocationOrientation arOrinentation;

        public override void onExecute(object param)
        {
            return;
            Debug.Log("AIMapPOICommand...");
            arOrinentation = GameObject.FindObjectOfType<ARLocationOrientation>();
            addModuleEvent(ModuleEventType.GET_POI_INFO_FROM_PHONE, HandleGetPOIInfoFromPhone);
            //worldRootPOI = new GameObject("WorldRootPOI");

            //ARSessionOrigin arSession = GameObject.FindObjectOfType<ARSessionOrigin>();
            //Vector3 cam = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
            //#if UNITY_EDITOR
            //sendModuleEvent(ModuleEventType.GET_POI_INFO_FROM_PHONE, json);
            //#endif
        }

        /// <summary>
        /// 从手机端获取POI信息
        /// </summary>
        /// <param name="ev"></param>
        private void HandleGetPOIInfoFromPhone(EventObject ev)
        {
            PlaceAtLocations atLocations = GameObject.FindObjectOfType<PlaceAtLocations>(true);
            if (atLocations == null) return;
            atLocations.gameObject.SetActive(true);

            string json = ev.param.ToString();
            if (json == null) return;
            wayzPOI = JsonMapper.ToObject<WayzPOI>(json);
            if (wayzPOI == null) return;
            sendModuleEvent(ModuleEventType.GET_POI_POS_IN_UNITY, wayzPOI);
            //Debug.Log(currentLocation.ToString());
        }

        bool isInitCompass = false;
        private void UpdateLoadingCompass()
        {
            Debug.Log(Input.compass.trueHeading + "/" + Camera.main.transform.eulerAngles.y + "/" + arOrinentation.transform.localEulerAngles.ToString());
            if (isInitCompass) return;
            if (Input.compass.trueHeading != 0)
            {
                isInitCompass = true;
                compassObj.transform.eulerAngles = new Vector3(0, -Input.compass.trueHeading, 0);
            }
        }

        /// <summary>
        /// 获取POI信息转化到unity场景
        /// </summary>
        /// <param name="ev"></param>
        private void HandleGetPOIPosInUnity(EventObject ev)
        {
            Debug.Log("HandleGetPOIPosInUnity...");
            WayzPOI pois = ev.param as WayzPOI;
            if (pois == null) return;

            if (poiObjList.Count.Equals(0))
            {
                string abPath = PATH_TYPE.MODEL + "/buildinginfo";
                string assetName = "BuildingInfo";
                ResourceManager.GetInstance().loadAssetAsync<GameObject>(abPath, assetName, LoadPOIObjCallBack);
            }
            else
            {
                for (int i = 0; i < buildingInfosInUnity.Count; i++)
                {
                    ResetPOI();
                    //SetBuildingInfo(poiObjList[i].transform, buildingInfosInUnity[i]);
                }
            }
        }

        //void ResetPOI()
        //{
        //    for (int i = 0; i < buildingInfosInUnity.Count; i++)
        //    {
        //        GameObject go = poiObjList[i];
        //        if (go == null) { Debug.Log(""); return; }
        //        go.name = buildingInfosInUnity[i].id;
        //        go.transform.position = buildingInfosInUnity[i].buildingLocation;
        //        int distance = buildingInfosInUnity[i].Distance / 10;
        //        distance = distance < 1 ? 1 : distance;
        //        Debug.Log("distance:" + distance);
        //        go.transform.localScale = new Vector3(distance/2, distance/2, distance/2);
        //        go.transform.LookAt(Camera.main.transform.position);
        //        //SetBuildingInfo(go.transform, buildingInfosInUnity[i]);
        //        SetPOIInfo(go.transform, buildingInfosInUnity[i]);
        //    }
        //}

        /// <summary>
        /// 重置POI
        /// </summary>
        void ResetPOI()
        {
            for (int i = 0; i < buildingInfosInUnity.Count; i++)
            {
                GameObject go = poiObjList[i];
                if (go == null) { Debug.Log(""); return; }
                go.name = buildingInfosInUnity[i].id;
                go.transform.position = buildingInfosInUnity[i].buildingLocation;
                int distance = buildingInfosInUnity[i].Distance / 10;
                distance = distance < 1 ? 1 : distance;
                Debug.Log("distance:" + distance);
                go.transform.localScale = new Vector3(distance / 2, distance / 2, distance / 2);
                go.transform.LookAt(Camera.main.transform.position);
                //SetBuildingInfo(go.transform, buildingInfosInUnity[i]);
                SetPOIInfo(go.transform, buildingInfosInUnity[i]);
            }
        }

        /// <summary>
        /// 读取POI物体回调
        /// </summary>
        /// <param name="obj"></param>
        private void LoadPOIObjCallBack(UnityEngine.Object obj)
        {
            Debug.Log("LoadPOIObjCallBack...");
            //if (loadCompleted) return;
            poiPrefab = (GameObject)obj;
            if (poiPrefab == null) return;
            foreach (var buildingInUnity in buildingInfosInUnity)
            {
                GameObject go = GameObject.Instantiate<GameObject>(poiPrefab);
                if (go == null) return;

                go.name = buildingInUnity.id;
                go.transform.position = buildingInUnity.buildingLocation;
                int distance = buildingInUnity.Distance;
                Debug.Log("distance:" + distance);
                float scale = distance / (distance / 10f);
                if (distance < 100)
                    go.transform.localScale = new Vector3(5, 5, 5);
                else
                    go.transform.localScale = new Vector3(scale, scale, scale);
                go.transform.parent = worldRootPOI.transform;
                go.transform.LookAt(Camera.main.transform.position);
                //SetBuildingInfo(go.transform, buildingInUnity);
                SetPOIInfo(go.transform, buildingInUnity);
                poiObjList.Add(go);
            }
        }

        //private void LoadPOIObjCallBack(UnityEngine.Object obj)
        //{
        //    Debug.Log("LoadPOIObjCallBack...");
        //    //if (loadCompleted) return;
        //    poiPrefab = (GameObject)obj;
        //    if (poiPrefab == null) return;
        //    foreach (var buildingInUnity in buildingInfosInUnity)
        //    {
        //        GameObject go = GameObject.Instantiate<GameObject>(poiPrefab);
        //        if (go == null) return;

        //        go.name = buildingInUnity.id;
        //        go.transform.position = buildingInUnity.buildingLocation;
        //        int distance = buildingInUnity.Distance / 10;
        //        distance = distance < 1 ? 1 : distance;
        //        Debug.Log("distance:" + distance);
        //        go.transform.localScale = new Vector3(distance, distance, distance);
        //        go.transform.parent = worldRootPOI.transform;
        //        go.transform.LookAt(Camera.main.transform.position);
        //        //SetBuildingInfo(go.transform, buildingInUnity);
        //        SetPOIInfo(go.transform, buildingInUnity);
        //        poiObjList.Add(go);
        //    }
        //}

        /// <summary>
        /// 设置POI信息
        /// </summary>
        /// <param name="go"></param>
        /// <param name="poiInfoInUnity"></param>
        void SetPOIInfo(Transform go, POIInfoInUnity poiInfoInUnity)
        {
            if (poiInfoInUnity == null) return;
            GameObject nameTextDetailObj = UnityUtilties.FindChild(go, "NameTextDetail");
            if (nameTextDetailObj != null)
            {
                Text nameTextDetail = nameTextDetailObj.GetComponent<Text>();
                if (nameTextDetail != null)
                {
                    nameTextDetail.text = poiInfoInUnity.name;
                }
            }
            GameObject distanceTextDetailObj = UnityUtilties.FindChild(go, "DistanceTextDetail");
            if (distanceTextDetailObj != null)
            {
                Text distanceTextDetail = distanceTextDetailObj.GetComponent<Text>();
                if (distanceTextDetail != null)
                {
                    distanceTextDetail.text = poiInfoInUnity.Distance + "m";
                }
            }
        }

        void SetBuildingInfo(Transform go, POIInfoInUnity building)
        {
            if (building == null) return;
            GameObject BuildingTextDetailObj = UnityUtilties.FindChild(go, "BuildingTextDetail");
            if (BuildingTextDetailObj != null)
            {
                Text BuildingTextDetail = BuildingTextDetailObj.GetComponent<Text>();
                if (BuildingTextDetail != null)
                {
                    //BuildingTextDetail.text = building.detail;
                }
            }

            GameObject nameTextDetailObj = UnityUtilties.FindChild(go, "NameTextDetail");
            if (nameTextDetailObj != null)
            {
                Text nameTextDetail = nameTextDetailObj.GetComponent<Text>();
                if (nameTextDetail != null)
                {
                    //nameTextDetail.text = building.otherInfo.名称;
                }
            }

            GameObject CreditCodeTextDetailObj = UnityUtilties.FindChild(go, "CreditCodeTextDetail");
            if (CreditCodeTextDetailObj != null)
            {
                Text CreditCodeTextDetail = CreditCodeTextDetailObj.GetComponent<Text>();
                if (CreditCodeTextDetail != null)
                {
                    //CreditCodeTextDetail.text = building.otherInfo.统一社会信用代码;
                }
            }

            GameObject RegistrationNumberTextDetailObj = UnityUtilties.FindChild(go, "RegistrationNumberTextDetail");
            if (RegistrationNumberTextDetailObj != null)
            {
                Text RegistrationNumberTextDetail = RegistrationNumberTextDetailObj.GetComponent<Text>();
                if (RegistrationNumberTextDetail != null)
                {
                    //RegistrationNumberTextDetail.text = building.otherInfo.注册号;
                }
            }

            GameObject OperatingStatusTextDetailObj = UnityUtilties.FindChild(go, "OperatingStatusTextDetail");
            if (OperatingStatusTextDetailObj != null)
            {
                Text OperatingStatusTextDetail = OperatingStatusTextDetailObj.GetComponent<Text>();
                if (OperatingStatusTextDetail != null)
                {
                    //OperatingStatusTextDetail.text = building.otherInfo.企业经营状态;
                }
            }

            GameObject PhoneNumberTextDetailObj = UnityUtilties.FindChild(go, "PhoneNumberTextDetail");
            if (PhoneNumberTextDetailObj != null)
            {
                Text PhoneNumberTextDetail = PhoneNumberTextDetailObj.GetComponent<Text>();
                if (PhoneNumberTextDetail != null)
                {
                    //PhoneNumberTextDetail.text = building.otherInfo.企业联系电话;
                }
            }
        }
    }
}
