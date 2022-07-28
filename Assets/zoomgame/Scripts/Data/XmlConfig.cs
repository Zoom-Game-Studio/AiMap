using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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

        public static void LogJson(string content)
        {
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
            return JsonConvert.DeserializeObject<LayerXml>(XmlConvert.GetJson(content).ToString());
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
            return JsonConvert.DeserializeObject<IconXml>(XmlConvert.GetJson(content).ToString());
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
            return JsonConvert.DeserializeObject<ModuleXml>(XmlConvert.GetJson(content).ToString());
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
            return JsonConvert.DeserializeObject<ModelXml>(json);
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