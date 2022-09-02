using System;
using System.IO;
using Data;
using Newtonsoft.Json;
using UnityEngine;

namespace HttpData
{
    public static class DataMapper
    {
        /// <summary>
        /// 把定位请求转为json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJson(BodyData data)
        {
            var content = new LocationRequest();
            content.UUID = data.UUID;
            content.distortion = data.distortion.ToString();
            content.intrinsic = data.intrinsic.ToString();
            content.orientation = data.orientation;
            // content.prior_maptile_name = data.prior_maptile_name;
            content.prior_rotation = data.prior_rotation.ToString();
            content.prior_translation = data.prior_translation.ToString();
            content.timestamp = data.timestamp.ToString();
            // content.horizontal_deviation = data.horizontal_deviation.ToString();
            // content.vertical_deviation = data.vertical_deviation.ToString();
            return Newtonsoft.Json.JsonConvert.SerializeObject(content);
        }

        /// <summary>
        /// 把鉴权通过回复的定位信息转为实例
        /// </summary>
        /// <param name="response">回复信息的内容</param>
        /// <returns></returns>
        public static Location ToLocation(string response)
        {
            var r = JsonConvert.DeserializeObject<LocationResponse>(response);
            return ToLocation(r);
        }


        /// <summary>
        /// 把鉴权通过回复的定位信息转为实例
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static Location ToLocation(LocationResponse response)
        {
            var location = new Location();
            location.deviation = response.deviation;
            location.maptile_name = response.maptile_name;
            location.ori_qvec = response.ori_qvec;
            location.ori_tvec = response.ori_tvec;
            location.description = response.description;
            location.translation = ToVector3(response.translation);
            location.rotation = ToQuaternion(response.rotation);
            return location;
        }

        public static Quaternion ToQuaternion(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return Quaternion.identity;
            }
            var numbers = content.Split(' ');
            var ro = new Quaternion();
            if (float.TryParse(numbers[0], out var x))
            {
                ro.x = x;
            }
            else
            {
                WeiXiang.Console.Error("convert string to x fail:"+ numbers[0]);
            }
            if (float.TryParse(numbers[1], out var y))
            {
                ro.y = y;
            }
            else
            {
                WeiXiang.Console.Error("convert string to y fail:"+ numbers[1]);
            }
            if (float.TryParse(numbers[2], out var z))
            {
                ro.z = z;
            }
            else
            {
                WeiXiang.Console.Error("convert string to z fail:"+ numbers[2]);
            }
            if (float.TryParse(numbers[3], out var w))
            {
                ro.w = w;
            }
            else
            {
                WeiXiang.Console.Error("convert string to z fail:"+ numbers[3]);
            }

            return ro;
        }

        public static void ConvertToUnity(Location location)
        {
        }

        public static Vector3 ToVector3(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return Vector3.zero;
            }
            var numbers = content.Split(' ');
            var vector = new Vector3();
            if (float.TryParse(numbers[0], out var x))
            {
                vector.x = x;
            }
            else
            {
                WeiXiang.Console.Error("convert string to x fail:"+ numbers[0]);
            }
            if (float.TryParse(numbers[1], out var y))
            {
                vector.y = y;
            }
            else
            {
                WeiXiang.Console.Error("convert string to y fail:"+ numbers[1]);
            }
            if (float.TryParse(numbers[2], out var z))
            {
                vector.z = z;
            }
            else
            {
                WeiXiang.Console.Error("convert string to z fail:"+ numbers[2]);
            }
            return vector;
        }

        public static string LoadImageFromPath(string path)
        {
            try
            {
                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                var bufferBytes = new byte[fileStream.Length];
                fileStream.Read(bufferBytes, 0, (int) fileStream.Length);
                var base64String = Convert.ToBase64String(bufferBytes);
                return base64String;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}