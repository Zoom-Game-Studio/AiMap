using System;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace C_ScriptsTest
{
    public class TestCamera : MonoBehaviour
    {

        [SerializeField] private ARCameraManager arCameraManager;
        private void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(5f)).Subscribe(OnInterval).AddTo(this);
        }

        void OnInterval(long _)
        {
            // if (!isARInit)
            // {
            //     //In my case 0=640*480, 1= 1280*720, 2=1920*1080
            //     arCameraManager.subsystem.currentConfiguration = arCameraManager.GetConfigurations(Allocator.Temp)[1];
            //     isARInit = true;
            // }
            bool isSucc = arCameraManager.TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics);
            var focalLength = cameraIntrinsics.focalLength.x + "," + cameraIntrinsics.focalLength.y;
            var principalPoint = cameraIntrinsics.principalPoint.x + "," + cameraIntrinsics.principalPoint.y;

            Debug.LogWarning($"focalLength: {focalLength},principalPoint: {principalPoint}");
        }
        
    }
}