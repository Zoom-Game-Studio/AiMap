using Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

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