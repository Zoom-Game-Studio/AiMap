using System;
using AppLogic;
using Architecture;
using QFramework;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WeiXiang;
using zoomgame.Scripts.Architecture.Query;

public class GpsStateUi : AbstractMonoController
{
    public TextMeshProUGUI content;
    public Image gpsIcon;
    public Color normal = Color.green;
    public Color error = Color.red;
    public Color unInit = Color.yellow;

    private void Start()
    {
        SetGpsState(GpsState.uninit);
        Observable.Interval(TimeSpan.FromSeconds(0.2f)).Subscribe(v =>
        {
            var amap = this.GetArchitecture().GetUtility<IAmap>();
            SetGpsState(amap.IsRunning? GpsState.normal: GpsState.error);
            UpdateContent(amap.IsRunning?$"{amap.Latitude},{amap.Longitude}": amap.ErrorCode.ToString());
            
        });
    }

    enum GpsState
    {
        normal,error,uninit
    }

    void UpdateContent(string info)
    {
        content.text = info;
    }

    void SetGpsState(GpsState state)
    {
        switch (state)
        {
            case GpsState.normal:
                gpsIcon.color = normal;
                break;
            case GpsState.error:
                gpsIcon.color = error;
                break;
            case GpsState.uninit:
                gpsIcon.color = unInit;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}