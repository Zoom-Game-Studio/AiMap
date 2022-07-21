using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    //如果好用，请收藏地址，帮忙分享。
    public class SkyDataItem
    {
    /// <summary>
    /// 
    /// </summary>
    public string cover { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int modeType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string modelNo { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string path { get; set; }
    /// <summary>
    /// 剧场One
    /// </summary>
    public string title { get; set; }
}

    public class SkyRoot
    {
    /// <summary>
    /// 
    /// </summary>
    public List<SkyDataItem> data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string filesPath { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int isArCore { get; set; }
    /// <summary>
    /// 剧场模式数据
    /// </summary>
    public string name { get; set; }
}

