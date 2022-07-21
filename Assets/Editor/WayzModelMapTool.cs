using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class WayzModelMapTool : EditorWindow
{
    static GameObject pointCloudObj;
    [MenuItem("Assets/创建点云实例")]
    public static void CreatePointCloud()
    {
        GameObject[] selectionOjbs = Selection.gameObjects;
        foreach (var selectionOjb in selectionOjbs)
        {
            string objPath = AssetDatabase.GetAssetPath(selectionOjb.GetInstanceID());
            GameObject prefabObj = AssetDatabase.LoadAssetAtPath(objPath, typeof(GameObject)) as GameObject;
            pointCloudObj = PrefabUtility.InstantiatePrefab(prefabObj) as GameObject;
            pointCloudObj.transform.eulerAngles = new Vector3(-90, 0, 0);
            pointCloudObj.transform.localScale = new Vector3(1, -1, 1);
            pointCloudObj.name = "PointCloudObj";
        }
    }
    [MenuItem("Assets/创建模型实例")]
    public static void CreateModel()
    {
        GameObject[] selectionOjbs = Selection.gameObjects;
        foreach (var selectionOjb in selectionOjbs)
        {
            GameObject modelParent = new GameObject();
            pointCloudObj = GameObject.Find("PointCloudObj");
            modelParent.transform.SetParent(pointCloudObj.transform, false);
            modelParent.transform.localScale = new Vector3(1, -1, 1);
            modelParent.name = "修改名称为唯一ID";

            string objPath = AssetDatabase.GetAssetPath(selectionOjb.GetInstanceID());
            GameObject prefabObj = AssetDatabase.LoadAssetAtPath(objPath, typeof(GameObject)) as GameObject;
            pointCloudObj = PrefabUtility.InstantiatePrefab(prefabObj) as GameObject;

            pointCloudObj.transform.SetParent(modelParent.transform, false);
        }
    }
    [MenuItem("GameObject/生成预制体", priority = 0)]
    public static void CreatePrefab()
    {
        string folderPath = "Assets/Common/TempPrefabs";
        GameObject[] selectionOjbs = Selection.gameObjects;
        Dictionary<string, Vector3> selectionObjsDic = new Dictionary<string, Vector3>();
        foreach (var selectionOjb in selectionOjbs)
        {
            selectionObjsDic.Add(selectionOjb.name, selectionOjb.transform.localScale);
        }

        foreach (var selectionOjb in selectionOjbs)
        {
            Vector3 tempScale = selectionOjb.transform.localScale;
            if (tempScale.y < 0)
                selectionOjb.transform.localScale = new Vector3(tempScale.x, -tempScale.y, tempScale.z);
            else
                selectionOjb.transform.localScale = new Vector3(tempScale.x, tempScale.y, tempScale.z);
        }



        foreach (var selectionOjb in selectionOjbs)
        {
            string filePath = folderPath + "/" + selectionOjb.name + ".prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(selectionOjb, filePath);
            AssetDatabase.Refresh();
        }
        foreach (var selectionOjb in selectionOjbs)
        {
            foreach (var name in selectionObjsDic.Keys)
            {
                if (selectionOjb.name.Equals(name))
                    selectionOjb.transform.localScale = selectionObjsDic[name];
            }
        }
        selectionObjsDic.Clear();
    }

}