using System;
using System.Collections.Generic;
using UnityDeveloperKit.Runtime.Extension;

namespace Waku.Module
{
    [System.Serializable]
    public class AssetInfoItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        /// <summary>
        /// 图片url
        /// </summary>
        public string avatar { get; set; }

        public string type { get; set; }
        public string deviceType { get; set; }
        public string style { get; set; }

        /// <summary>
        /// 地块名字
        /// </summary>
        public string place { get; set; }

        /// <summary>
        /// gps坐标
        /// </summary>
        public Coordinate coordinate { get; set; }
        public Bundary bundary { get; set; }
        
        public string boundaryId { get; set; }
        public string city { get; set; }
        public string township { get; set; }
        public string address { get; set; }
        public Package package { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }

        public AssetInfoItem Clone()
        {
            AssetInfoItem item = new AssetInfoItem();
            item.id = id;
            item.name = name;
            item.description = description;
            item.avatar = avatar;
            item.type =type;
            item.deviceType = deviceType;
            item.style = style;
            item.place = place;
            item.coordinate = coordinate.Clone();
            item.bundary = bundary.Clone();
            item.boundaryId = boundaryId;
            item.city = city;
            item.township = township;
            item.address = address;
            item.package = package.Clone();
            item.createTime = createTime;
            item.updateTime = updateTime;
            return item;
        }

        public class Coordinate
        {
            float longitude { get; set; }
            float latitude { get; set; }

            public Coordinate Clone()
            {
                return new Coordinate()
                {
                    longitude = longitude,
                    latitude = latitude,
                };
            }
        }

        public class Bundary
        {
            public string type { set; get; }
            public List<List<float>> coordinates { get; set; }

            public Bundary Clone()
            {
                var b = new Bundary();
                b.type = type;
                b.coordinates = coordinates;
                return b;
            }
        }
        
        public class Package
        {
            public class File
            {
                public string id { get; set; }
                public string name { get; set; }
                public string type { get; set; }
                public string link { get; set; }
                public float size { get; set; }
                public string createTime { get; set; }
                public string updateTime { get; set; }

                public File Clone()
                {
                    var f = new File();
                    f.id = id;
                    f.name = name;
                    f.type = type;
                    f.link = link;
                    f.size = size;
                    f.createTime = createTime;
                    f.updateTime = updateTime;
                    return f;
                }
            }
            
            public List<File> files { get; set; }

            public Package Clone()
            {
                var p = new Package();
                p.files = new List<File>();
                foreach (var file in files)
                {
                    p.files.Add(file.Clone());
                }
            }
        }
    }
}