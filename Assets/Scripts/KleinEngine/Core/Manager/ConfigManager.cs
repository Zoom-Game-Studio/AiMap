using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KleinEngine
{
    public class ConfigManager
    {
        private static readonly XMLParser XML_PARSER = new XMLParser();
        //key(string):具体配置类名
        private static Dictionary<string, Dictionary<string, BaseConfigInfo>> allConfigs = new Dictionary<string, Dictionary<string, BaseConfigInfo>>();

        public static IDictionary GetConfigs<T>()
        {
            Type t = typeof(T);
            string name = t.Name;
            Debug.Log("configName:" + name);
            if (allConfigs.ContainsKey(name))
            {
                allConfigs.Remove(name);
                //return allConfigs[name];
            }
            string fileName = name.Substring(0, name.Length - 10);
            string cfgStr = LoadConfig(fileName);
            if (cfgStr == string.Empty) return null;
            allConfigs[name] = OnParse(t, cfgStr);
            Debug.Log("allConfigs:" + allConfigs.Count);
            return allConfigs[name];
        }

        public static IDictionary GetConfigs<T>(string cfgStr) where T : BaseConfigInfo
        {
            Type t = typeof(T);
            if (cfgStr == string.Empty) return null;
            return OnParse(t, cfgStr);
        }

        public static T GetConfigInfo<T>(int id) where T : BaseConfigInfo
        {
            return GetConfigInfo<T>(id.ToString());
        }

        public static T GetConfigInfo<T>(string id) where T : BaseConfigInfo
        {
            Type t = typeof(T);
            string name = t.Name;
            //if (!allConfigs.ContainsKey(name))
            {
                //"ConfigInfo"字符串长度为10，配置文件名=配置类名前缀（即类名长度减去10）
                Debug.Log("name@:"+name);
                string fileName = name.Substring(0, name.Length - 10);
                string cfgStr = LoadConfig(fileName);
                if (cfgStr == string.Empty) return default(T);
                allConfigs[name] = OnParse(t, cfgStr);
            }
            //else
            //{
            //    if(name.Equals("data"))
            //    {
            //        string fileName = name.Substring(0, name.Length - 10);
            //        string cfgStr = LoadConfig(fileName);
            //        if (cfgStr == string.Empty) return default(T);
            //        allConfigs[name] = OnParse(t, cfgStr);
            //    }
            //}
            if (allConfigs[name].ContainsKey(id)) return allConfigs[name][id] as T;
            //          Debug.Log("没有找到对应配置:" + name + " id:" + id);
            return default(T);
        }

        private static string LoadConfig(string fileName)
        {
            Debug.Log("loadconfig:"+fileName);
            TextAsset asset = ResourceManager.GetInstance().loadAsset<TextAsset>("data", fileName.ToLower());

            //Debug.Log("asset:"+asset.ToString());
            if (null != asset) return asset.text;
            //asset = ResourceManager.GetInstance().loadAsset<TextAsset>("dynamic_data", fileName.ToLower());
            //if (null != asset) return asset.text;
            return string.Empty;
        }

        private static Dictionary<string, BaseConfigInfo> OnParse(Type t, string str)
        {
            XMLNode node = XML_PARSER.Parse(str);
            XMLNodeList list = node.GetNodeList("config>0>item");//注意 这里固定配置
            int len = list.Count;
            Dictionary<string, BaseConfigInfo> configs = new Dictionary<string, BaseConfigInfo>(len);
            PropertyInfo[] fields = t.GetProperties();
            //          MethodInfo[] fields = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            BaseConfigInfo info;
            for (int i = 0; i < len; i++)
            {
                XMLNode subNode = (XMLNode)list[i];
                info = ParseConfigInfo(t, fields, subNode);
                configs[info.id] = info;   //可以在导出XML文件的工具里检验id是否重复
            }
            node.Clear();
            return configs;
        }

        private static BaseConfigInfo ParseConfigInfo(Type type, PropertyInfo[] propertys, XMLNode node)
        {
            BaseConfigInfo cfgInfo = Activator.CreateInstance(type) as BaseConfigInfo;
            int count = propertys.Length;
            for (int i = 0; i < count; ++i)
            {
                PropertyInfo p = propertys[i];
                string key = '@' + p.Name;
                if (node.ContainsKey(key))
                {
                    string value = node.GetValue(key);
                    if (p.PropertyType.IsArray)
                    {
                        //Debug.Log(p.PropertyType.GetElementType());
                        switch (p.PropertyType.Name)
                        {
                            case "String[]":
                                string[] strList = BaseConfigInfo.GetValueArray(value);
                                p.SetValue(cfgInfo, strList, null);
                                break;
                            case "Tuple2[]":
                                string[] tp2List = BaseConfigInfo.GetValueArray(value);
                                int tp2Len = tp2List.Length;
                                Tuple2[] tp2Array = new Tuple2[tp2Len];
                                for (int tp2Num = 0; tp2Num < tp2Len; tp2Num++)
                                {
                                    Tuple2 tp2Temp = new Tuple2();
                                    tp2Temp.onParseData(tp2List[tp2Num]);
                                    tp2Array[tp2Num] = tp2Temp;
                                }
                                p.SetValue(cfgInfo, tp2Array, null);
                                break;
                        }
                    }
                    else
                    {
                        switch (p.PropertyType.Name)
                        {
                            case "Tuple2":
                                Tuple2 tp2 = new Tuple2();
                                tp2.onParseData(value);
                                p.SetValue(cfgInfo, tp2, null);
                                break;
                            case "Boolean":
                                bool valueBool = value.Equals("1") ? true : false;
                                p.SetValue(cfgInfo, Convert.ChangeType(valueBool, p.PropertyType), null);
                                break;
                            default:
                                p.SetValue(cfgInfo, Convert.ChangeType(value, p.PropertyType), null);
                                break;
                        }
                    }
                }
                else
                {
                    if (p.PropertyType.Name.Contains("Dictionary"))
                    {
                        int indexofA = p.PropertyType.FullName.IndexOf(",[");
                        int indexofB = p.PropertyType.FullName.IndexOf("ConfigInfo");
                        string fullClsName = p.PropertyType.FullName.Substring(indexofA + 2, indexofB - indexofA + 8);
                        string[] clsNameList = fullClsName.Split('.');
                        string clsName = clsNameList[clsNameList.Length - 1];
                        string nodeName = clsName.Substring(0, clsName.Length - 10).ToLower();
                        XMLNodeList nodeList = node.GetNodeList(nodeName);
                        if (null == nodeList) continue;
                        int len = nodeList.Count;
                        IDictionary dic = Activator.CreateInstance(p.PropertyType) as IDictionary;
                        //Type clstype = Type.GetType(fullClsName);
                        Type clstype = GlobalObject.GetType(fullClsName);
                        PropertyInfo[] nextPropertys = clstype.GetProperties();
                        for (int m = 0; m < len; ++m)
                        {
                            //XMLNode nextNode = nodeList.Pop();//XMLNode nodeNext = (XMLNode)nodeList[m]; 比较一下效率
                            XMLNode nextNode = (XMLNode)nodeList[m];
                            BaseConfigInfo cfg = ParseConfigInfo(clstype, nextPropertys, nextNode);
                            dic[cfg.id] = cfg;
                        }
                        nodeList.Clear();
                        p.SetValue(cfgInfo, dic, null);
                    }
                    else if (p.PropertyType.Name.Contains("ConfigInfo"))
                    {
                        string subName = p.PropertyType.Name.Substring(0, p.PropertyType.Name.Length - 10).ToLower();
                        XMLNode subNode = node.GetNodeList(subName).Pop();
                        Type cfgType = Type.GetType(p.PropertyType.Name);
                        if (null != cfgType)
                        {
                            PropertyInfo[] cfgPropertys = cfgType.GetProperties();
                            BaseConfigInfo cfg = ParseConfigInfo(cfgType, cfgPropertys, subNode);
                            p.SetValue(cfgInfo, cfg, null);
                        }
                    }
                }
            }
            return cfgInfo;
        }

        public static string GetLanguage(string key)
        {
            LanguageConfigInfo cfgInfo = GetConfigInfo<LanguageConfigInfo>(key);
            if (null != cfgInfo) return cfgInfo.value;
            return "";
        }

        //public static T GetConfigInfoExtern<T>(int id) where T : BaseConfigInfo, new()
        //{
        //    Type t = typeof(T);
        //    string name = t.Name;
        //    if (!allConfigs.ContainsKey(name))
        //    {
        //        //"ConfigInfo"字符串长度为10，配置文件名=配置类名前缀（即类名长度减去10）
        //        string fileName = name.Substring(0, name.Length - 10);
        //        string cfgStr = LoadConfig(fileName);
        //        if (cfgStr == string.Empty) return default(T);
        //        allConfigs[name] = OnParseExtern<T>(cfgStr);
        //    }
        //    if (allConfigs[name].ContainsKey(id)) return allConfigs[name][id] as T;
        //    //          Debug.Log("没有找到对应配置:" + name + " id:" + id);
        //    return default(T);
        //}

        ////可以结合工具生成反序列化代码（执行速度比反射快将近25%）
        //private static Dictionary<int, BaseConfigInfo> OnParseExtern<T>(string str) where T : BaseConfigInfo, new()
        //{
        //    XMLNode node = GlobalObject.XML_PARSER.Parse(str);
        //    XMLNodeList list = node.GetNodeList("config>0>item");//注意 这里固定配置
        //    int len = list.Count;
        //    Dictionary<int, BaseConfigInfo> configs = new Dictionary<int, BaseConfigInfo>(len);
        //    BaseConfigInfo info;
        //    for (int i = 0; i < len; i++)
        //    {
        //        XMLNode subNode = (XMLNode)list[i];
        //        info = new T();
        //        info.onParse(subNode);
        //        configs[info.id] = info;   //可以在导出XML文件的工具里检验id是否重复
        //    }
        //    node.Clear();
        //    return configs;
        //}
    }
}
