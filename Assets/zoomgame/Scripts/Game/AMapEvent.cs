using UnityEngine;  
using System.Collections;  

public class AMapEvent : AndroidJavaProxy {  

    public AMapEvent ()  
        : base ("com.amap.api.location.AMapLocationListener")  
    {  
    }  

    void onLocationChanged (AndroidJavaObject amapLocation)  
    {  
        if (locationChanged != null) {  
            locationChanged (amapLocation);  
        }  
    }  

    public delegate void DelegateOnLocationChanged(AndroidJavaObject amap);  
    public event DelegateOnLocationChanged locationChanged;  
}