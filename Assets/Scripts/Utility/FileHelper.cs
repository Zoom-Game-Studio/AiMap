using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

public class FileHelper//创建目录
{
    /// <summary>
    /// 如果存在就删除 并且创建
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="name">Name.</param>

    public static void IsDeleteFile(string path, string name)
    {
        string FilePath = path + "//" + name;
        FileInfo ThisFile = new FileInfo(FilePath);
        if (ThisFile.Exists)
            ThisFile.Delete();
    }

	/// 判断文件是否存在
	public static bool IsFile(string path, string name)
	{
		string FilePath = path + "//" + name;
		FileInfo ThisFile = new FileInfo(FilePath);
		if (ThisFile.Exists) {
			return true;
		} 
		else 
		{
			return false;
		}
	}
    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="name">Name.</param>

    public static void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);
    }
    /// <summary>
    /// 删除指定目录及其所有子目录
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    public static void DeleteDirectory(string directoryPath)
    {
        if (IsExistDirectory(directoryPath))
        {
            Directory.Delete(directoryPath, true);
        }
    }
    /// <summary>
    /// Creates the directory.
    /// </summary>
    /// <param name="directoryPath">Directory path.</param>
    public static void CreateDirectory(string directoryPath)
    {
        //如果目录不存在则创建该目录
        if (!IsExistDirectory(directoryPath))
        {
            //Debug.Log("path doesnot exit");
            Directory.CreateDirectory(directoryPath);
        }
    }
    public static bool IsExistDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }
    /// <summary>
    /// Md5file the specified file.
    /// </summary>
    /// <param name="file">File.</param>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
    public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
    {
        //如果目录不存在，则抛出异常
        if (!IsExistDirectory(directoryPath))
        {
            throw new FileNotFoundException();
        }

        try
        {
            if (isSearchChild)
            {
                return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
            }
            else
            {
                return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
        }
        catch (IOException ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 获取指定目录中所有文件列表
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>        
    public static string[] GetFileNames(string directoryPath)
    {
        //如果目录不存在，则抛出异常
        if (!IsExistDirectory(directoryPath))
        {
            throw new FileNotFoundException();
        }
        //获取文件列表
        return Directory.GetFiles(directoryPath);
    }
    //获取文件列
    /// <summary>
    /// 创建文件
    /// </summary>
    public static void CreateModuleFile(string writepath, string name, byte[] info, int length)
    {
        Debug.Log(writepath + "//" + name);
        Stream sw = null;
        FileInfo t = new FileInfo(writepath + "//" + name);
        //stringToEdit +="t.Exists=="+ t.Exists  + "\n";
        if (!t.Exists)
        {
            sw = t.Create();
        }
        else
        {
            return;
        }
        sw.Write(info, 0, length);
        sw.Close();
        sw.Dispose();
    }
}
