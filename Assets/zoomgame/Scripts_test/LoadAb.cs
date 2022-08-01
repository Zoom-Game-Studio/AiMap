using System;
using Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

public class LoadAb : MonoBehaviour
{
    public string tileName = "9L_国潮风";

    [SerializeField] private bool loadOnStart = false;
    private void Start()
    {
        if (loadOnStart)
        {
            Invoke(nameof(Build),3);
        }
    }

    [Button]
    void Build()
    {
        if (Application.isPlaying)
        {
            TileBuilder.Instantiate(tileName);
        }
    }

}