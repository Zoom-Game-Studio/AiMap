using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class AngularVelocityReader : MonoBehaviour
{
    // Debug.Log("[Tracking]");

    Vector3 PrevPos;
    Vector3 NewPos;

    Vector3 PrevRot;
    Vector3 NewRot;

    public Vector3 angularVelocity;
    public Vector3 ObjRotation;
    // Use this for initialization
    void Start()
    {
        PrevPos = transform.position;
        PrevRot = transform.eulerAngles;
    }

    // TODO(frye movel to common module)
    private Vector3 QuaternionRotatePoint(Quaternion quaternion, Vector3 point)
    {
        Quaternion quat_norm = Quaternion.Normalize(quaternion);
        Quaternion q1 = quat_norm;
        q1 = Quaternion.Inverse(q1);
        Quaternion qNode = new Quaternion(point.x, point.y, point.z, 0);
        qNode = quat_norm * qNode * q1;
        return new Vector3(qNode.x, qNode.y, qNode.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool transfer_right_handed = false;
        if (transfer_right_handed)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            // milliseconds
            long timestamp = Convert.ToInt64(ts.TotalSeconds * 1000);

            Debug.Log("[Tracking]" + ';'
                + timestamp.ToString() + ';'
                + Camera.main.transform.position.ToString("0.000") + ';'
                + Camera.main.transform.rotation.ToString("0.000") + ';'
                + Camera.main.velocity.ToString("0.000") + ';'
                + Camera.main.transform.eulerAngles.ToString("0.000"));

            return;
        }
        else
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            // milliseconds
            long timestamp = Convert.ToInt64(ts.TotalSeconds * 1000);


            Vector3 camera_pos = Camera.main.transform.position;
            Quaternion camera_rot = Camera.main.transform.rotation;
            Vector3 camera_linear = Camera.main.velocity;
            Vector3 new_camera_linear = QuaternionRotatePoint(Quaternion.Euler(0, 0, 90), camera_linear);

            Quaternion z90 = Quaternion.Euler(0, 0, -90);
            camera_rot = camera_rot * z90;

            /// 将视觉定位相机坐标（右手系）下的模型点转到Unity相机坐标系(左手系)下
            camera_pos.y = -camera_pos.y;
            camera_rot.y = -camera_rot.y;
            camera_rot.w = -camera_rot.w;
            new_camera_linear.y = -new_camera_linear.y;

            Debug.Log("[Tracking]" + ';'
                + timestamp.ToString() + ';'
                + camera_pos.ToString("0.000") + ';'
                + camera_rot.ToString("0.000") + ';'
                + new_camera_linear.ToString("0.000") + ';'
                + Camera.main.transform.eulerAngles.ToString("0.000"));
            //+ Camera.main.transform.eulerAngles.ToString("0.000"));
        }

        /*
                //transform.rotation = Quaternion.identity;
                if (false)
                {
                    NewPos = transform.position;  // each frame track the new position
                    angularVelocity = (NewPos - PrevPos) / Time.fixedDeltaTime;
                    PrevPos = NewPos;  // update position for next frame calculation

                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

                    // milliseconds
                    long timestamp = Convert.ToInt64(ts.TotalSeconds * 1000);

                    /// unity手机本体坐标系到unity相机坐标系需绕Z轴转90°
                    Quaternion z90 = Quaternion.Euler(0, 0, -90);
                    Quaternion mobileQuatInUnity = Camera.main.transform.rotation;
                    Quaternion right_handed_rotation = mobileQuatInUnity * z90;

                    Vector3 right_handed_position = QuaternionRotatePoint(z90, Camera.main.transform.position);
                    right_handed_position.y = -right_handed_position.y;

                    /// 左手系转右手系
                    right_handed_rotation.y = -right_handed_rotation.y;
                    right_handed_rotation.w = -right_handed_rotation.w;

                    NewRot = transform.eulerAngles;  // each frame track the new rotation
                    ObjRotation = (NewRot - PrevRot) / Time.fixedDeltaTime;
                    PrevRot = NewRot;  // update position for next frame calculation

                    Debug.Log("[Tracking]," + timestamp.ToString()
                        + ',' +
                        right_handed_position.ToString("0.000") + ',' + right_handed_rotation.ToString("0.000")
                        + ',' +
                        Camera.main.velocity.ToString("0.000")
                        + ',' +
                        ObjRotation.ToString("0.000")
                        );
                }*/
    }

}
