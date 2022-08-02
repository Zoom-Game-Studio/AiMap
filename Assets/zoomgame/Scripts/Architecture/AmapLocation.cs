using System;
using QFramework;
using UnityEngine;

namespace WeiXiang
{
    /// <summary>
    /// 高德定位接口
    /// </summary>
    public interface IAmap : IUtility
    {
        /// <summary>
        /// 经度 104
        /// </summary>
        float Longitude { get; }
        /// <summary>
        /// 纬度 30
        /// </summary>
        float Latitude { get; }
    }
    /// <summary>
    /// 高德定位
    /// </summary>
    public class AmapLocation : IAmap
    {
        public static AmapLocation Instance { private set; get; }
        private bool _isRunning = false;
        private AndroidJavaClass unityPlayer;
        private AndroidJavaObject currentActivity;
        private AndroidJavaObject locationClient;
        private AndroidJavaObject locationOption;
        private AMapEvent amap;
        /// <summary>
        /// 经度
        /// </summary>
        private double longitude = 104.067607;

        /// <summary>
        /// 纬度
        /// </summary>
        private double latitude = 30.551928;

        private string errorMessage;

        public bool IsRunning => _isRunning;
        public string ErrorMessage => errorMessage;

        /// <summary>
        /// 经度
        /// </summary>
        public float Longitude => (float) longitude;
        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude => (float) latitude;

        public AmapLocation()
        {
            Instance = this;
        }
        
        public void StartLocation()
        {
            try
            {
                this.unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                this.currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                //同意隐私授权
                var androidJavaClass = new AndroidJavaClass("com.amap.api.location.AMapLocationClient");
                androidJavaClass.CallStatic("updatePrivacyShow", currentActivity, true, true);
                androidJavaClass.CallStatic("updatePrivacyAgree", currentActivity, true);
                //更新设定
                this.locationClient =
                    new AndroidJavaObject("com.amap.api.location.AMapLocationClient", currentActivity);
                this.locationOption = new AndroidJavaObject("com.amap.api.location.AMapLocationClientOption");
                locationClient.Call("setLocationOption", locationOption);
                //设定定位监听
                this.amap = new AMapEvent();
                amap.locationChanged += OnLocationChanged;
                locationClient.Call("setLocationListener", amap);
                //开启定位
                locationClient.Call("startLocation");
                _isRunning = true;
            }
            catch (Exception e)
            {
                WeiXiang.Console.Error("[Amap] location error" + e.Message);
                EndLocation();
            }
        }

        void EndLocation()
        {
            if (amap != null)
            {
                amap.locationChanged -= OnLocationChanged;
            }
            if (locationClient != null)
            {
                locationClient.Call("stopLocation");
                locationClient.Call("onDestroy");
            }

            _isRunning = false;
        }

        private void OnLocationChanged(AndroidJavaObject amapLocation)
        {
            if (amapLocation != null)
            {
                if (amapLocation.Call<int>("getErrorCode") == 0)
                {
                    this.longitude = amapLocation.Call<double>("getLongitude");
                    this.latitude = amapLocation.Call<double>("getLatitude");
                    errorMessage = String.Empty;
                }
                else
                {
                    this.errorMessage = amapLocation.Call<string>("getErrorInfo");
                }

            }
        }
    }
}