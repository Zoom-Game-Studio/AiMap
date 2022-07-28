using System.IO;
using System.Xml;
using Architecture;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;

public class LoadAb : MonoBehaviour
{
    public string url = "9L_国潮风";
    private TileAssetBundles tile;
    private TileLoader loader;

    [Header("建筑信息")] 
    [SerializeField] private string buildInfoUrl = @"chengdu\model\buildinginfo";
    [SerializeField] private string buildInfoPrefab ="BuildingInfo";

    [Button]
    void Build()
    {
        if (Application.isPlaying)
        {
            this.loader = new TileLoader();
            this.tile = loader.LoadTile(url);
        }
    }

    [Button]
    void InsBuildInfoPrefab()
    {
        var buildUrl = Path.Combine(Application.streamingAssetsPath, buildInfoUrl);
        var ab = AssetBundle.LoadFromFile(buildUrl);
        var info = ab.LoadAsset<GameObject>(buildInfoPrefab);
        Instantiate(info);
    }
}