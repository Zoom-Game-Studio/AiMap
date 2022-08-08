using UnityEngine;

namespace C_ScriptsTest
{
    public class RailTest : MonoBehaviour
    {
        private System.Collections.Generic.List<Waku.Module.AssetInfoItem> serverList;
        public static  RailTest instance { get; private set; }
        private void Start()
        {
            instance = this;
        }

        public static void ShowBoundary(System.Collections.Generic.List<Waku.Module.AssetInfoItem> list)
        {
            instance.serverList = list;
            instance.ShowRail();
        }

        void ShowRail()
        {
            foreach (var info in serverList)
            {
                var coordinate = info.coordinate;
                var go = new GameObject(info.place + "_" + info.name+"_"+info.id);
                var pos = new Vector3()
                {
                    x = coordinate.longitude,
                    z = coordinate.latitude,
                };
                go.transform.position = pos;
                var boundarys = info.boundary.coordinates[0];
                foreach (var b in boundarys)
                {
                    var p = new GameObject(info.name);
                    p.transform.SetParent(go.transform);
                    var pPos = new Vector3()
                    {
                        x = b[0],
                        z = b[1],
                    };
                    p.transform.position = pPos;
                }
            }
        }
    }
}