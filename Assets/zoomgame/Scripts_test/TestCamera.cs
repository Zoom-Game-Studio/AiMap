using System;
using System.IO;
using System.Net;
using Architecture;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace C_ScriptsTest
{
    public class TestCamera : MonoBehaviour
    {
        public string assetId;
        [Button]
        void Test()
        {
            TileBuilder.Instantiate(Application.streamingAssetsPath,assetId);
        }
    }
}