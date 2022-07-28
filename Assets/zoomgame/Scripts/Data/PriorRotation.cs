using Sirenix.OdinInspector;
using UnityEngine;

namespace HttpData
{
    /// <summary>
    /// 设备旋转信息
    /// </summary>
    [System.Serializable]
    public struct PriorRotation
    {
        public float x, y, z, w;

        public override string ToString()
        {
            return "{" + $"x={x},y={y},z={z},w={w}" + "}";
        }

        public static PriorRotation GetRotation()
        {
            var c = Camera.main;
            if (c)
            {
                var ro = c.transform.rotation;
                return new PriorRotation()
                {
                    x = ro.x, y = ro.y, z = ro.z, w = ro.w,
                };
            }
            else
            {
                return new PriorRotation()
                {
                    x = 0, y = 0, z = 0, w = 0,
                };
            }
        }
    }
    
    
}