using System;
using WeiXiang;
using Console = WeiXiang.Console;

namespace HttpData
{
    /// <summary>
    /// gps
    /// </summary>
    [System.Serializable]
    public struct PriorTranslation
    {
        public float alt, lat, lon;

        public static PriorTranslation Default => new PriorTranslation()
        {
            alt = 0,
            lat = 0,
            lon = 0
        };

        public static PriorTranslation ShangHai => new PriorTranslation()
        {
            alt = 10,
            lat = 31.17980398552158f,
            lon = 121.60449397773354f
        };

        public static PriorTranslation ChengDu => new PriorTranslation()
        {
            alt = 66,
            lat = 30.551928f,
            lon = 104.067607f
        };

        public override string ToString()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return json;
        }

        public static PriorTranslation GetGps()
        {
            try
            {
                PriorTranslation gps = new PriorTranslation();
                gps.lat = AmapLocation.Instance.Latitude;
                gps.lon = AmapLocation.Instance.Longitude;
                
                if (!AmapLocation.Instance.IsRunning)
                {
                    Console.Error("[Amap] 运行异常:"+ AmapLocation.Instance.ErrorMessage);
                }
                return gps;
            }
            catch (Exception e)
            {
                Console.Error("Catch Exception on get gps:" + e.Message);
                return PriorTranslation.Default;
            }
        }
    }
}