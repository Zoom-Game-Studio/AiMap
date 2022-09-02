using System.Collections.Generic;
using Data;
using HttpData;
using QFramework.Config;
using UnityEngine;
using msQuaternion = System.Numerics.Quaternion;
using msVector3 = System.Numerics.Vector3;
using Pose = Data.Pose;

namespace WeiXiang
{
    /// <summary>
    /// 本地化转换
    /// </summary>
    public static class LocalizationConvert
    {
        # region 定位转换
        /// <summary>
        /// convert
        /// </summary>
        public static void ModelToCamera(Pose camera6DoFInfo,Pose modelPose,Pose startPosition)
        {
            //1 模型坐标点转到视觉定位的相机坐标系下
            msQuaternion cameraQuatInMapRight = new msQuaternion(camera6DoFInfo.rot.x, camera6DoFInfo.rot.y, camera6DoFInfo.rot.z, camera6DoFInfo.rot.w);
            msVector3 cameraTransInMapRight = new msVector3(camera6DoFInfo.pos.x, camera6DoFInfo.pos.y, camera6DoFInfo.pos.z);
            msQuaternion cameraQuatInMapRightInverse = msQuaternion.Inverse(cameraQuatInMapRight);

            msVector3 msModelPosition = new msVector3(modelPose.pos.x, modelPose.pos.y, modelPose.pos.z);
            msVector3 cameraTransInMapRightInverse = (MsQuaternionRotatePoint(cameraQuatInMapRightInverse, (msModelPosition - cameraTransInMapRight)));
            Vector3 modelInCamPosition = new Vector3(cameraTransInMapRightInverse.X, cameraTransInMapRightInverse.Y, cameraTransInMapRightInverse.Z);

            Quaternion modelInCamRotation = Quaternion.Inverse(camera6DoFInfo.rot) * modelPose.rot;
            
            //2 ARCore跟踪得到的unity手机本体位姿
            Vector3 mobilePositionInUnity = startPosition.pos;
            Quaternion mobileQuatInUnity = startPosition.rot;
            
            //3 相机坐标系转换为Unity相机坐标系,视觉定位相机坐标（右手系）下的模型点转为Unity相机的坐标系（左手系）
            modelInCamPosition.y = -modelInCamPosition.y;
            modelInCamRotation.y = -modelInCamRotation.y;
            modelInCamRotation.w = -modelInCamRotation.w;
            
            //4. unity手机本体坐标系到unity相机坐标系需要绕Z轴转90°
            Quaternion z90 = Quaternion.Euler(0, 0, -90);
            if (GlobalObject.SCREEN_ORIENTATION.Equals("1"))
            {
                switch (Input.deviceOrientation)
                {
                    case DeviceOrientation.LandscapeLeft:
                        z90 = Quaternion.Euler(0, 0, 0);
                        break;
                    case DeviceOrientation.LandscapeRight:
                        z90 = Quaternion.Euler(0, 0, 0);
                        break;
                }
            }
            Quaternion cameraRotInUnity90 = mobileQuatInUnity * z90;
            
            Vector3 modelPosUnityCam = QuaternionRotatePoint(cameraRotInUnity90, modelInCamPosition) + mobilePositionInUnity;
            Quaternion modeRotUnityCam = cameraRotInUnity90 * modelInCamRotation;
        }
        
        private static msVector3 MsQuaternionRotatePoint(msQuaternion quaternion, msVector3 point)
        {
            msQuaternion quat_norm = msQuaternion.Normalize(quaternion);
            msQuaternion q1 = quat_norm;
            q1 = msQuaternion.Conjugate(q1);

            msQuaternion qNode = new msQuaternion(point.X, point.Y, point.Z, 0);
            qNode = quat_norm * qNode * q1;

            return new msVector3(qNode.X, qNode.Y, qNode.Z);
        }
        
        private static Vector3 QuaternionRotatePoint(Quaternion quaternion, Vector3 point)
        {
            Quaternion quat_norm = Quaternion.Normalize(quaternion);
            Quaternion q1 = quat_norm;
            q1 = Quaternion.Inverse(q1);
            Quaternion qNode = new Quaternion(point.x, point.y, point.z, 0);
            qNode = quat_norm * qNode * q1;
            return new Vector3(qNode.x, qNode.y, qNode.z);
        }
        #endregion

        private static Transform _origin;

        private static List<Vector3> _postionArr = new List<Vector3>();
        private static List<Vector3> _eularArr = new List<Vector3>();

        static Vector3 GetArrAverage(List<Vector3> arr)
        {
            float x = 0;
            float y = 0;
            float z = 0;
            foreach (var v in arr)
            {
                x += v.x;
                y += v.y;
                z += v.z;
            }

            return new Vector3()
            {
                x = x / arr.Count,
                y = y / arr.Count,
                z = z / arr.Count,
            };
        }

        /// <summary>
        /// 左边原定
        /// localEulerAngles = new Vector3(90, 0, 0);
        /// position = Vector3.zero;
        /// </summary>
        public static Transform Origin
        {
            get
            {
                if (!_origin)
                {
                    var o = GameObject.FindGameObjectWithTag("Origin");
                    if (o)
                    {
                        _origin = o.transform;
                    }
                    else
                    {
                        _origin = new GameObject("坐标原点").transform;
                        _origin.gameObject.tag = "Origin";
                        _origin.transform.localEulerAngles = new Vector3(-90, 0, 0);
                        _origin.transform.localScale = new Vector3(1, -1, 1);
                        _origin.transform.position = Vector3.zero;
                    }
                }
                return _origin;
            }
        }


        /// <summary>
        /// 获取定位信息的在unity坐标系中的姿态
        /// </summary>
        /// <param name="locationInfo"></param>
        /// <returns></returns>
        public static Pose LocationToUnityPose(Location locationInfo)
        {
            var view = new GameObject(locationInfo.maptile_name+locationInfo.translation).transform;
            view.SetParent(Origin);
            view.localPosition = locationInfo.translation;
            view.localEulerAngles = locationInfo.rotation.eulerAngles;
            var pose = new Pose()
            {
                rot = view.rotation,
                pos = view.position,
            };
            Object.Destroy(view.gameObject);
            return pose;
        }

        /// <summary>
        /// 把模型得到的姿态和camera view匹配，对齐坐标系
        /// </summary>
        /// <param name="locationPose">定位返回姿态</param>
        /// <param name="cameraPose">相机在截图时的姿态</param>
        public static void CoordinatesAlignWithView(Pose locationPose,Transform cameraPose)
        {
            if (!cameraPose)
            {
                Console.Error("Can get main camera,fail to align with view !");
                return;
            }
            var anchor = new GameObject
            {
                transform =
                {
                    rotation = locationPose.rot,
                    position = locationPose.pos
                }
            };

            Origin.SetParent(anchor.transform);

            var transform = cameraPose;
            anchor.transform.position = transform.position;
            anchor.transform.rotation = transform.rotation;

            Origin.transform.SetParent(null);
            anchor.transform.localScale = Vector3.one;
            Object.Destroy(anchor.gameObject);
            
            // //计算多次定位的平均值
            // _postionArr.Add(Origin.position);
            // _eularArr.Add(Origin.eulerAngles);
            // Origin.position = GetArrAverage(_postionArr);
            // Origin.eulerAngles = GetArrAverage(_eularArr);
        }
    }
}