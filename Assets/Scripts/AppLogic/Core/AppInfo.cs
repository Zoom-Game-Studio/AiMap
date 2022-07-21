using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppLogic
{
    public class AppInfo
    {

    }

    public class ParallelverseInfos
    {
        public List<ParallelverseInfo> parallesverseList = new List<ParallelverseInfo>();
    }

    public class ParallelverseInfo
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string remark = string.Empty;
    }

    public class DynamicAsset
    {
        public string id = string.Empty;
        public string crc = string.Empty;
        public int length = 0;
        public bool isUpdate = false;
    }

    /// <summary>
    /// 平行宇宙功能callback所传数据
    /// </summary>
    public class ParallelverseModelInfo
    {
        public string id;
        public List<GameObject> modelList = new List<GameObject>();

    }


    /// <summary>
    /// 模型可见性 数据传输定义
    /// </summary>
    public class ModelVisibleInfo
    {
        public float distance;
        public List<GameObject> modelList = new List<GameObject>();
    }

    //维智9L求签
    public class Wayz9LLuckySign
    {
        public string id = null;
        public string name = null;
        public string detail = null;
    }

    //向安卓传递的参数
    public class UrlFromUnity
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type = null;
        /// <summary>
        /// 初期定义为url，拓展为字符串
        /// </summary>
        public string url = null;
    }

    /// <summary>
    /// 区域内资源文件
    /// </summary>
    public class LocalAreaAssetsFile
    {
        public string id = null;
        public string contentPath = null;
        public string screenOrientation = "0";
    }

    /// <summary>
    /// 下载的文件
    /// </summary>
    public class DownloadFiles
    {
        public List<DownloadFilePath> downloadFilePath = new List<DownloadFilePath>();
    }
    /// <summary>
    /// 下载的文件路径
    /// </summary>
    public class DownloadFilePath
    {
        /// <summary>
        /// 需要下载的文件网络地址
        /// </summary>
        public string downloadPath = null;
        /// <summary>
        /// 下载成功后的文件本地地址
        /// </summary>
        public string localFilePath = null;
    }

    public class OriginTransform
    {
        public Vector3 originPos = Vector3.zero;
        public Vector3 orginEuler = Vector3.zero;
        public Vector3 originScale = Vector3.one;
    }

    public class ActionReportData
    {
        public string actionTime;
        public string actionValue = "472";
        public string value;
    }

    public class Files
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string path { get; set; }
    }

    public class CameraImageInfo
    {
        public string path = null;
        public string focalLength = null;
        //public string resolution = null;
        public string principalPoint = null;
    }

    public class LoadModelInfo
    {
        /// <summary>
        /// 是否支持ARcore
        /// </summary>
        public string isArCore { get; set; }
        /// <summary>
        /// 是否支持上传
        /// </summary>
        public string isUpload { get; set; }
        /// <summary>
        /// 模型类型
        /// </summary>
        public string moduleType { get; set; }
        /// <summary>
        /// 截图水印
        /// </summary>
        public string banner { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 文件列表所在文件夹路径
        /// </summary>
        public string folderPath { get; set; }
    }

    public class MapPositionInfo
    {
        public int causeValue { get; set; }
        public string description { get; set; }
        public string deviation { get; set; }
        public List<double> ori_qvec { get; set; }
        public List<double> ori_tvec { get; set; }
        public string rotation { get; set; }
        public string timestamp { get; set; }
        public string translation { get; set; }
    }


    /// <summary>
    /// 模型的6Dof信息
    /// </summary>
    public class Model6DoFInfo
    {
        public Vector3 pos = Vector3.zero;
        public Quaternion rot = Quaternion.identity;

        public override string ToString()
        {
            return "Model6DoFInfo: " + "(" + pos.x + "," + pos.y + "," + pos.z + ")" + "/" + "(" + rot.x + "," + rot.y + "," + rot.z + "," + rot.w + ")";
        }
    }

    public class ChangeModelInfo
    {
        public string modelId = null;
        public GameObject model = null;
        public Vector3 pos = Vector3.zero;
        public Vector3 eulerAngle = Vector3.zero;
        public Vector3 scale = Vector3.zero;
        public string abName = null;
        public string remarks = null;

        public override string ToString()
        {
            return "ChangeModelInfo: " + "(" + pos.x + "," + pos.y + "," + pos.z + ")" + "/" + "(" + eulerAngle.x + "," + eulerAngle.y + "," + eulerAngle.z+")";
        }
    }

    public class ChangeModelInfoSavingFormat
    {
        public string modelId = null;
        public string pos = null;
        public string eulerAngle = null;
        public string scale = null;
        public string remarks = null;
        public string abName = null;
    }

    public class LoadingModelInfo
    {
        public string id = null;
        public string name = null;
        public string abName = null;
        public Vector3 pos = Vector3.zero;
        public Quaternion rot = Quaternion.identity;
        public Vector3 eulerAngle = Vector3.zero;
        public Vector3 scale = Vector3.one;
    }

    public class UIHideInfo
    {
        public bool hide = true;
        public int type = 0;

    }
    public class LoadAssetInfo
    {
        public List<string> assetNameList = new List<string>();
        public Action<bool> CallBack = null;
    }


    #region POI old

    public class POIInfoInUnity
    {
        public Vector3 buildingLocation = Vector3.zero;

        public string id { get; set; }
        /// <summary>
        /// 炬芯大楼
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 维智科技
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Gps gps { get; set; }

    }


    public class Gps
    {
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int alt { get; set; }
    }

    public class BuidingLocation
    {
        /// <summary>
        /// 
        /// </summary>
        public double x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double z { get; set; }
    }

    public class PoisItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 炬芯大楼
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 维智科技
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Gps gps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BuidingLocation buidingLocation { get; set; }
    }

    public class POIInfos
    {
        /// <summary>
        /// 
        /// </summary>
        public int causeValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PoisItem> pois { get; set; }
    }

    #endregion

    #region POI New
    public class POIListItem
    {
        public string placeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int distance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int classId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string className { get; set; }
    }

    public class CurrentLocation
    {
        /// <summary>
        /// 
        /// </summary>
        public double lon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }
    }

    public class WayzPOI
    {
        /// <summary>
        /// 
        /// </summary>
        public List<POIListItem> POIList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CurrentLocation currentLocation { get; set; }
    }


    #endregion

    #region 3d字体
    namespace ThreeDText
    {
        public class ThreeDTextListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string placeId { get; set; }
            /// <summary>
            /// 维智科技欢迎您！
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double lon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double lat { get; set; }
        }

        public class CurrentLocation
        {
            /// <summary>
            /// 
            /// </summary>
            public double lon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double lat { get; set; }
        }

        public class BulletScreenInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ThreeDTextListItem> ThreeDTextList { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CurrentLocation currentLocation { get; set; }
        }
        #endregion

    }
}
