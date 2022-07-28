using System;
using UniRx;
using UnityEngine;

public class GpsTest : MonoBehaviour
{
    private void Start()
    {
        Input.location.Start();
        Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(OnInterval);
    }

    void OnInterval(long _)
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var data = Input.location.lastData;
            Debug.Log($"{data.altitude},{data.latitude},{data.longitude}");
        }
        else
        {
            Debug.Log($"gps not runing ,{Input.location.status}");
        }
    }
}