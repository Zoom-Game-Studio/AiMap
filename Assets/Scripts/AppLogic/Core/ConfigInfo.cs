using KleinEngine;
using System.Collections.Generic;
namespace AppLogic
{
    public class AppConfigInfo : BaseConfigInfo
    {
        public Dictionary<string, NetConfigInfo> netDic { get; private set; }
    }

    public class NetConfigInfo : BaseConfigInfo
    {
        public string ip { get; private set; }
        public int port { get; private set; }
    }

    public class LayerConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
    }

    public class ModuleConfigInfo : BaseConfigInfo
    {
        public string abName { get; private set; }
        public string layer { get; private set; }
        public bool limitBack { get; private set; }
    }
    public class ModelConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string abName { get; private set; }
        public string eulerangle { get; private set; }
        public string pos { get; private set; }
        public string rot { get; private set; }
        public string remarks { get; private set; }
        public string[] parallelverse { get; private set; }
    }

    public class ParallelverseConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string remark { get; private set; }
    }

    public class IconConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string icon { get; private set; }
    }

    public class VersionConfigInfo : BaseConfigInfo
    {
        public string value { get; private set; }
        public string md5 { get; private set; }
    }

    public class AssetConfigInfo : BaseConfigInfo
    {
        public string crc { get; private set; }
        public int length { get; private set; }
    }

    public class BrandConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string icon { get; private set; }
    }

    public class CarConfigInfo : BaseConfigInfo
    {
        public int brand { get; private set; }
        public string name { get; private set; }
        public string icon { get; private set; }
        public string model { get; private set; }
        public bool open { get; private set; }
        public string[] wheelSize { get; private set; }
        public string style { get; private set; }
        public bool mrmode { get; private set; }
        public int vip { get; private set; }
    }

    public class PartConfigInfo : BaseConfigInfo
    {
        public int brand { get; private set; }
        public int type { get; private set; }
        public string name { get; private set; }
        public string icon { get; private set; }
        public string model { get; private set; }
        public int price { get; private set; }
        public bool open { get; private set; }
        public int animation { get; private set; }
        public string[] size { get; private set; }
        public int height { get; private set; }
        public int vip { get; private set; }
        public string[] carId { get; private set; }
        public string style { get; private set; }
    }
    public class CarPartConfigInfo : BaseConfigInfo
    {
        public int type { get; private set; }
        public string name { get; private set; }
        public string icon { get; private set; }
        public string module { get; private set; }
        public bool needPrice { get; private set; }
        public string camera { get; private set; }
        public string[] point { get; private set; }
        public string objname { get; private set; }
        public bool open { get; private set; }
        public bool color { get; private set; }
    }

    public class MaterialConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string color_attr { get; private set; }
        public string icon { get; private set; }
    }

    public class ColorConfigInfo : BaseConfigInfo
    {
        public string icon { get; private set; }
        public float r { get; private set; }
        public float g { get; private set; }
        public float b { get; private set; }
        public float a { get; private set; }
        public string color { get; internal set; }
        public string reflect { get; private set; }
    }

    public class MatConfigInfo : BaseConfigInfo
    {
        public string obj_name { get; private set; }
        public Dictionary<string, ColorConfigInfo> colorDic { get; set; }
    }

    public class StyleConfigInfo : BaseConfigInfo
    {
        public Dictionary<string, MatConfigInfo> matDic { get; set; }
    }

    public class AniValueConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public int value { get; private set; }
    }
    public class AnimationConfigInfo : BaseConfigInfo
    {
        public string para_name { get; private set; }
        public Dictionary<string, AniValueConfigInfo> paraValueDic { get; set; }
    }
    public class PartBrandConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string icon { get; private set; }
        public string introduction { get; private set; }
        public string phone { get; private set; }
    }

    public class SuspensionConfigInfo : BaseConfigInfo
    {
        public string icon { get; private set; }
        public float maxValue { get; private set; }
        public float minValue { get; private set; }
        public string name { get; private set; }

    }

    public class TemplateConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string palyable { get; private set; }
        public bool open { get; private set; }
    }

    public class SkyBoxConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string scene { get; private set; }
        public string icon { get; private set; }
        public string cube { get; private set; }
        public bool open { get; private set; }
    }

    public class LabelConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public Dictionary<string, AdditionConfigInfo> labelDic { get; private set; }
    }

    public class AdditionConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string parents { get; private set; }
        public bool open { get; private set; }
    }

    public class StoreConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string icon { get; private set; }
        public string phone { get; private set; }
        public int fans { get; private set; }
        public int worlds { get; private set; }
        public string introduction { get; private set; }
    }

    public class SceneChartConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string icon { get; private set; }
        public string objName { get; private set; }
        public string[] cameraNmae { get; private set; }
        public bool open { get; private set; }
    }

    public class ShopConfigInfo : BaseConfigInfo
    {
        public string name { get; private set; }
        public string logo { get; private set; }
    }

    public class SettingConfigInfo : BaseConfigInfo
    {
        public string value { get; private set; }
    }

    public class InteriorConfigInfo : BaseConfigInfo
    {
        public string baseImg { get; private set; }
        public string[] cameraPoint { get; private set; }
        public Dictionary<string, InteriorPartConfigInfo> partDic { get; private set; }
    }

    public class InteriorPartConfigInfo : BaseConfigInfo
    {
        public Dictionary<string, InteriorStyleConfigInfo> styleDic { get; private set; }

    }

    public class InteriorStyleConfigInfo : BaseConfigInfo
    {
        public string icon { get; private set; }
        public string img { get; private set; }
        public string mainImg { get; private set; }
    }

}
