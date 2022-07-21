using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace KleinEngine
{
    public class GlobalObject
    {
        //资源存放目录
        public static string ASSET_PATH = Application.persistentDataPath;

        public static string OLD_ASSET_PATH = null;

        public static string SCREEN_ORIENTATION = "0";

        //public static readonly StringBuilder STR_BUILDER = new StringBuilder();
        private static Assembly appAssembly = null;

        //代码所在域不同于类型所在域 不能单纯使用Type.GetType(typeName);
        public static Type GetType(string typeName)
        {
            if (null == appAssembly)
            {
                Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();//AppDomain在uwp平台不兼容
                int len = assemblyArray.Length;
                for (int i = 0; i < len; ++i)
                {
                    Assembly am = assemblyArray[i];
                    if (am.GetName().Name == "Assembly-CSharp")
                    {
                        appAssembly = am;
                        break;
                    }
                }
            }
            Type t = null;
            if (null != appAssembly)
            {
                t = appAssembly.GetType(typeName);
                if (null == t) t = Type.GetType(typeName);
            }
            return t;
            //return Type.GetType(typeName);
        }

        //public static string GetAssemblyVersion()
        //{
        //    return typeof(MyUtils).GetTypeInfo().Assembly.GetName().Version.ToString();
        //}
        //在程序集输出类型为类库或者 Windows 运行时组件时均可用，其限制条件是需知道程序集中的一个类，再通过 typeof(MyUtils).GetTypeInfo().Assembly 取得目标程序集，进而获得程序集的版本号。
        //var applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly 获得程序集所有的类
        //foreach (var temp in applacationAssembly.DefinedTypes)
        //{
        //    if (temp.CustomAttributes.Any(t => t.AttributeType == typeof(ViewModelAssembly)))
        //    {

        //    }
        //}
        //var applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
        //foreach (var temp in applacationAssembly.DefinedTypes
        //    .Where(temp => temp.CustomAttributes.Any(t => t.AttributeType == typeof(ViewModelAssembly))))
        //{

        //}
    }
}
