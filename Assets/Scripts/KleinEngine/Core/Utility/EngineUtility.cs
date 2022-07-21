using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace KleinEngine
{
    public class EngineUtility
    {
        private static float time;
        //private static int time;
        public static void OnTimeStart()
        {
            //time = Environment.TickCount;
            time = Time.realtimeSinceStartup;
        }

        public static void OnTimeEnd(string str = "")
        {
            float test = (Time.realtimeSinceStartup - time) * 1000;
            //int test = Environment.TickCount - time;
            Debug.LogWarning("测试经过时间(毫秒)：" + test + " [" + str + "]");
        }

        private static MD5 md5 = MD5.Create();
        public static string MD5Encode(string src)
        {
            //byte[] srcBytes = Encoding.Default.GetBytes(src);//Encoding.Default在uwp系统不兼容
            byte[] srcBytes = Encoding.UTF8.GetBytes(src);
            byte[] md5Bytes = md5.ComputeHash(srcBytes);
            return BitConverter.ToString(md5Bytes).Replace("-", "").ToLower();
        }

        public static string GetMD5(string msg)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();
            return FormatMD5(md5Data);
        }

        public static string GetFileMd5(string filename)
        {
            string fileMd5 = string.Empty;
            if (File.Exists(filename))
            {
                using (var fileStream = File.OpenRead(filename))
                {
                    var md5 = MD5.Create();
                    var fileMD5Bytes = md5.ComputeHash(fileStream);//计算指定Stream 对象的哈希值                                     
                    fileMd5 = FormatMD5(fileMD5Bytes);
                }
            }
            return fileMd5;
        }

        public static string GetDataMd5(byte[] bytes)
        {
            string fileMd5 = string.Empty;
            var md5 = MD5.Create();
            var fileMD5Bytes = md5.ComputeHash(bytes);
            fileMd5 = FormatMD5(fileMD5Bytes);
            return fileMd5;
        }

        public static string FormatMD5(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLower();//将byte[]装换成字符串
            /*
            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
            */
        }

        /// 获取当前时间戳（精确到毫秒）
        public static ulong GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToUInt64(ts.TotalMilliseconds);
        }

        static DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        public static DateTime NormalizeTimpstamp(long timpStamp)
        {
            long unixTime = timpStamp * 10000L;
            TimeSpan toNow = new TimeSpan(unixTime);
            DateTime dt = dtStart.Add(toNow);
            return dt;
        }

        /// <summary>
        /// 时钟式倒计时
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public string GetSecondString(int second)
        {
            return string.Format("{0:D2}", second / 3600) + string.Format("{0:D2}", second % 3600 / 60) + ":" + string.Format("{0:D2}", second % 60);
        }

        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
            Debug.Log(startTime);
            time = startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }


        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(DateTime target, long timestamp)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            return start.AddSeconds(timestamp);
        }

        //获取缩略图
        public static Texture2D GetThumbnail(Texture2D source, int targetWidth, int targetHeight, bool makeNoLongerReadable = true)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int i = 0; i < result.height; ++i)
            {
                for (int j = 0; j < result.width; ++j)
                {
                    Color newColor = source.GetPixelBilinear((float)j * incX, (float)i * incY);
                    result.SetPixel(j, i, newColor);
                }
            }
            result.Apply(false, makeNoLongerReadable);
            return result;
        }
    }
}
