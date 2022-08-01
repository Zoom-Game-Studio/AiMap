using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LitJson;
using Newtonsoft.Json;
using QFramework;
using UnityEngine;
using Formatting = Newtonsoft.Json.Formatting;

namespace Data
{
    public static class XmlConvert
    {
        public static StringBuilder GetJson(string content)
        {
            var xml = new XmlDocument();
            xml.LoadXml(content);
            var json = JsonConvert.SerializeXmlNode(xml, Formatting.None, true);
            var sb = new StringBuilder(json);
            var index = json.IndexOf("}");
            sb.Remove(0, index + 1);
            sb.Replace("@", string.Empty);
            sb.Replace("#comment", "comment");
            return sb;
        }
        
        public static Vector3 ConvertToVector3(this string data)
        {
            var self = new Vector3();
            try
            {
                var numbers = data.Split(",");
                self.x = float.Parse(numbers[0]);
                self.y= float.Parse(numbers[1]);
                self.z = float.Parse(numbers[2]);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return self;
        }
        
        public static Quaternion ConvertToQuaternion(this string data)
        {
            var self = new Quaternion();
            try
            {
                var numbers = data.Split(",");
                self.x = float.Parse(numbers[0]);
                self.y= float.Parse(numbers[1]);
                self.z = float.Parse(numbers[2]);
                self.w = float.Parse(numbers[3]);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return self;
        }
    }

    public interface IXmlItem
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class LayerXml
    {
        public List<LayerXmlItem> item = new List<LayerXmlItem>();

        public class LayerXmlItem : IXmlItem
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public static LayerXml Convert(string content)
        {
            var json = XmlConvert.GetJson(content).ToString();
            Debug.Log(json);
            var jsonData = JsonMapper.ToObject(json);
            var data = jsonData["item"];
            var xml = new LayerXml();
            if (data.IsArray)
            {
                var count = data.Count;
                for (int i = 0; i < count; i++)
                {
                    var itemData = data[i];
                    var item = new LayerXmlItem();
                    item.id = (string)itemData["id"];
                    item.name = (string)itemData["name"];
                    xml.item.Add(item);
                }
            }
            else if (data.IsObject){
                var item = new LayerXmlItem();
                item.id = (string)data["id"];
                item.name = (string)data["name"];
                xml.item.Add(item);
            }
            return xml;
        }
    }

    public class IconXml
    {
        public List<IconXmlItem> item = new List<IconXmlItem>();

        public class IconXmlItem : IXmlItem
        {
            public string id { get; set; }
            public string name { get; set; }
            public string icon { get; set; }
        }

        public static IconXml Convert(string content)
        {
            var json = XmlConvert.GetJson(content).ToString();
            Debug.Log(json);
            var jsonData = JsonMapper.ToObject(json);
            var data = jsonData["item"];
            var xml = new IconXml();
            if (data.IsArray)
            {
                var count = data.Count;
                for (int i = 0; i < count; i++)
                {
                    var itemData = data[i];
                    var item = new IconXmlItem();
                    item.id = (string)itemData["id"];
                    item.name = (string)itemData["name"];
                    item.icon = (string)itemData["icon"];
                    xml.item.Add(item);
                }
            }
            else if (data.IsObject){
                var item = new IconXmlItem();
                item.id = (string)data["id"];
                item.name = (string)data["name"];
                item.icon = (string)data["icon"];
                xml.item.Add(item);
            }
            return xml;
        }
    }

    /// <summary>
    /// 模块 xml
    /// </summary>
    public class ModuleXml
    {
        public string[] comment;
        public List<ModuleXmlItem> item = new List<ModuleXmlItem>();

        public class ModuleXmlItem
        {
            public string id { get; set; }
            public string layer { get; set; }
            public string limitBack { get; set; }
        }
        
        public static ModuleXml Convert(string content)
        {
            // return JsonConvert.DeserializeObject<ModuleXml>(XmlConvert.GetJson(content).ToString());
            var json = XmlConvert.GetJson(content).ToString();
            Debug.Log(json);
            var jsonData = JsonMapper.ToObject(json);
            var data = jsonData["item"];
            var xml = new ModuleXml();
            if (data.IsArray)
            {
                var count = data.Count;
                for (int i = 0; i < count; i++)
                {
                    var itemData = data[i];
                    var item = new ModuleXmlItem();
                    item.id = (string)itemData["id"];
                    item.layer = (string)itemData["layer"];
                    item.limitBack = (string)itemData["limitBack"];
                    xml.item.Add(item);
                }
            }
            else if (data.IsObject){
                var item = new ModuleXmlItem();
                item.id = (string)data["id"];
                item.layer = (string)data["layer"];
                item.limitBack = (string)data["limitBack"];
                xml.item.Add(item);
            }
            return xml;
        }
    }

    /// <summary>
    /// 模型 xml
    /// </summary>
    public class ModelXml
    {
        public string[] comment;
        public List<ModelXmlItem> item = new List<ModelXmlItem>();

        public class ModelXmlItem : IXmlItem
        {
            public string id { get; set; }
            public string name { get; set; }
            public string scale { get; set; }
            public string abName { get; set; }
            public string eulerangle { get; set; }
            public string pos { get; set; }
            public string rot { get; set; }
            public string remarks { get; set; }
        }

        public class ModelXmlItemData
        {
            /// <summary>
            /// model 的id，默认为名字
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// model的名字
            /// </summary>
            public string name { get; set; }
            public Vector3 scale { get; set; }
            
            /// <summary>
            /// 模型所在ab包的在主包的路径相对
            /// </summary>
            public string abName { get; set; }
            public Vector3 eulerangle { get; set; }
            public Vector3 pos { get; set; }
            public Quaternion rot { get; set; }
            public string remarks { get; set; }
            
        }
                
        public static ModelXml Convert(string content)
        {
            var json = XmlConvert.GetJson(content).ToString();
            Debug.Log(json);
            var jsonData = JsonMapper.ToObject(json);
            var data = jsonData["item"];
            ModelXml xml = new ModelXml();
            if (data.IsArray)
            {
                var count = data.Count;
                for (int i = 0; i < count; i++)
                {
                    var itemData = data[i];
                    var item = new ModelXmlItem();
                    item.id = (string)itemData["id"];
                    item.name = (string)itemData["name"];
                    item.scale = (string)itemData["scale"];
                    item.abName = (string)itemData["abName"];
                    item.eulerangle = (string)itemData["eulerangle"];
                    item.pos = (string)itemData["pos"];
                    item.rot = (string)itemData["rot"];
                    item.remarks = (string)itemData["remarks"];
                    xml.item.Add(item);
                }
            }
            else if (data.IsObject)
            {
                var item = new ModelXmlItem();
                item.id = (string)data["id"];
                item.name = (string)data["name"];
                item.scale = (string)data["scale"];
                item.abName = (string)data["abName"];
                item.eulerangle = (string)data["eulerangle"];
                item.pos = (string)data["pos"];
                item.rot = (string)data["rot"];
                item.remarks = (string)data["remarks"];
                xml.item.Add(item);
            }
            return xml;
        }

        public List<ModelXmlItemData> ConvertItemsToData()
        {
            var list = new ModelXmlItemData[item.Count];
            for (var i = 0; i < item.Count; i++)
            {
                var e = item[i];
                var data = new ModelXmlItemData()
                {
                    id = e.id,
                    name = e.name,
                    scale = e.scale.ConvertToVector3(),
                    abName = e.abName,
                    eulerangle = e.eulerangle.ConvertToVector3(),
                    pos = e.pos.ConvertToVector3(),
                    rot = e.rot.ConvertToQuaternion(),
                    remarks = e.remarks
                };
                list[i] = data;
            }

            return list.ToList();
        }
    }
}