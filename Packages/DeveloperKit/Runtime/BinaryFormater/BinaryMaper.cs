using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// 二进制读取
/// </summary>
public class BinaryMaper
{
    /// <summary>
    /// 对象转为二进制文件
    /// </summary>
    /// <param name="content"></param>
    /// <param name="outPath"></param>
    /// <typeparam name="T"></typeparam>
    public static void ToBinary<T>(T content, string outPath)
    {
        if (!System.IO.Directory.Exists(outPath))
        {
            Debug.LogError("输出路径不存在：" + outPath);
            return;
        }

        using (var fs = new FileStream(outPath, FileMode.Create))
        {
            var binaryFormat = new BinaryFormatter();
            binaryFormat.Serialize(fs, content);
        }
    }

    /// <summary>
    /// 二进制文件转为对象
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToObject<T>(string path)
    {
        using (var fs = new FileStream(path,FileMode.Open))
        {
            var binaryFormat = new BinaryFormatter();
            var obj =(T)binaryFormat.Deserialize(fs);
            return obj;
        }
    }
}