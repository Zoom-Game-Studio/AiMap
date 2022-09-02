using System.Collections.Generic;
using Sirenix.OdinInspector;
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
            public string id;
            public Vector3 localPos;
            public Vector3 localAngle;
            public Vector3 localScale = Vector3.one;
        }

        [InfoBox("场景特殊节点配置，匹配接口数据中的id后，为ab包实例设定特殊父节点")]
        public List<RootData> nodeList;
    }
}