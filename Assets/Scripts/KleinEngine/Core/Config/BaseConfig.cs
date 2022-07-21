using System.Collections.Generic;

namespace KleinEngine
{
    public class BaseConfig<T> where T : new()
    {
        protected Dictionary<int, T> configs;

        public T getConfigInfo(int id)
        {
            if (null == configs)
                parseData();

            if (null != configs)
            {
                if (configs.ContainsKey(id))
                {
                    return configs[id];
                }
                else
                {
                    //				Util.Log("没有找到对应的配置 filename：" + this + "    id:" + id);
                }
            }
            else
            {
                //			Debug.Log("没有找到对应的配置文件 filename:" + this);
            }
            return default(T);
        }

        protected virtual void parseData()
        {

        }
    }

    public class BaseConfigInfo
    {
        public string id { get;protected set;}

        private static char[] SPLIT_SYMBOL_EXTEND = new char[] { '|' };
        private static char[] SPLIT_SYMBOL_LIST = new char[] { ',' };

        public BaseConfigInfo() { }

        public static string[] GetValueList(string str)
        {
            if (string.IsNullOrEmpty(str)) return new string[0];
            //if (null != str && str != "") list = str.Split(SPLIT_SYMBOL_EXTEND);
            return str.Split(SPLIT_SYMBOL_EXTEND);
        }
        public static string[] GetValueArray(string str)
        {
            if (string.IsNullOrEmpty(str)) return new string[0];
            //if (null != str && str != "") list = str.Split(SPLIT_SYMBOL_LIST);
            return str.Split(SPLIT_SYMBOL_LIST);
        }

        public virtual void onParse(XMLNode node)
        {
            id = node.GetValue("@id");
        }
    }
}