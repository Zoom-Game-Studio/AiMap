using UnityEngine;

namespace QFramework.Config
{
    [CreateAssetMenu(fileName = "ModelRootConfig", menuName = "Config/新建模型根节点配置表", order = 0)]
    public class ModelRootConfig : ScriptableObject
    {
        private static ModelRootConfig _config;

        public static ModelRootConfig Instance
        {
            get
            {
                if (!_config)
                {
                    _config = Resources.Load<ModelRootConfig>("Config/ModelRootConfig");
                }

                return _config;
            }
        }

        [System.Serializable]
        public class RootData
        {
            /// <summary>
            /// asset bundle 的下载url
            /// </summary>
            public string abUrl;
            public string tileName;
            public Vector3 localPostion;
            public Vector3 localEuler;
        }

        public RootData ShangHai;

        public RootData Chengdu_indoor;

    }
}