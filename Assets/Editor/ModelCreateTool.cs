using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;

public class ModelCreateTool : Editor
{
    public static void ClearHierarchy()
    {
        var allGos = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        var sllgos = FindObjectsOfType<GameObject>();
        //var previousSelection = Selection.objects;
        //Selection.objects = allGos;
        //var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        //Selection.objects = previousSelection;
        for (int i = 0; i < sllgos.Length; i++)
        {
            Debug.Log(sllgos[i].name);
        }
    }
    //[MenuItem("AppTools/记录Model信息", false, 0)]
    public static void SaveModelInfo()
    {
        string path = "";
        string folderPath = "/Common/Prefabs/Model/9L/";  //路径

        string pathTemp = Application.dataPath + folderPath;
#if UNITY_EDITOR_WIN
        //if (!SystemInfo.operatingSystem.Contains("Mac"))
        //pathTemp = (Application.dataPath + folderPath).Replace("/", "\\");
#endif
        ClearHierarchy();
        //GetFiles(pathTemp);
    }

    public static void GetFiles(string folderPath)
    {
        Debug.Log(folderPath);
        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(folderPath))
        {
            DirectoryInfo direction = new DirectoryInfo(folderPath);
            FileInfo[] files = direction.GetFiles("*");
            Debug.Log(files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                //忽略关联文件
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                ChangePrefabPara(Path.Combine(folderPath, files[i].Name));
                //Debug.Log("文件名:" + files[i].Name);
                //Debug.Log("文件绝对路径:" + files[i].FullName);
                //Debug.Log("文件所在目录:" + files[i].DirectoryName);
            }
        }
        else
        {
            Debug.Log("file is not exist");
        }
    }
    public static void ChangePrefabPara(string path)
    {
        Debug.Log(path.IndexOf("Assets").ToString());
        string prefabPath = path.Substring(path.IndexOf("Assets"));
        //GameObject prefabObj = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

        //GameObject go = PrefabUtility.InstantiatePrefab(prefabObj) as GameObject;
        GameObject go = PrefabUtility.LoadPrefabContents(prefabPath);

        if (go == null) Debug.Log("go is null");
        go.transform.localScale = Vector3.one;
        PrefabUtility.SaveAsPrefabAsset(go, path);
        PrefabUtility.UnloadPrefabContents(go);
    }
}
