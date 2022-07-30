using System.IO;
using System.Xml;
using Architecture;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;

public class LoadAb : MonoBehaviour
{
    public string tileName = "9L_国潮风";


    [Button]
    void Build()
    {
        if (Application.isPlaying)
        {
            TileBuilder.Instantiate(tileName);
        }
    }

}