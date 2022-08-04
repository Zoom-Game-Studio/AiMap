using QFramework;
using UnityEngine;
using WeiXiang;

namespace zoomgame.Scripts.Architecture.Query
{
    public class QueryGpsCommand : AbstractQuery<Vector2>
    {
        protected override Vector2 OnDo()
        {
            var amap = this.GetArchitecture().GetUtility<IAmap>();
            var gps = new Vector2();
            gps.x = amap.Longitude;
            gps.y = amap.Latitude;
            return gps;
        }
    }
}