using System.IO;
using UnityEngine;

namespace WeiXiang
{
    public static class PathConvert
    {
        /// <summary>
        /// 获取ab包的加载路径
        /// </summary>
        /// <param name="mainLocalPath">主包的相对路径</param>
        /// <returns></returns>
        public static string GetFullPath(string mainLocalPath)
        {
            return Path.Combine(Application.streamingAssetsPath, mainLocalPath);
        }

        /// <summary>
        /// 加载主包下的路径
        /// </summary>
        /// <param name="mainLocalPath">主包的相对路径</param>
        /// <param name="assetBundleName">分包的名字，读取主包获得</param>
        /// <returns></returns>
        public static string GetPath(string mainLocalPath, string assetBundleName)
        {
            return Path.Combine(Application.streamingAssetsPath, mainLocalPath + "/" + assetBundleName);
        }
    }
}