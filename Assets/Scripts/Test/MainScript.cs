using System;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject GamePannel;//要控制移动的物体
    public GameObject TargetPos;//要跟随的物体
    public Vector3 pos;
    public int SamplingNumber = 10;//采样帧数

    private bool FindObjState = true;
    private int NumCount = 0;
    private int MovCount = 0;

    private Quaternion IndexRoa;
    private Vector3 IndexPos;
    private Vector3 MovePos;
    private Quaternion MoveRoa;


    void Update()
    {
        DelayMove();
    }

    public void SetPannelActive(bool Istrue)
    {
        FindObjState = Istrue;
        GamePannel.SetActive(Istrue);
    }

    void DelayMove()
    {
        if (FindObjState)
        {
            //Debug.Log("true");
            NumCount += 1;

            //Pos记录
            IndexPos.x = TargetPos.transform.localPosition.x - GamePannel.transform.localPosition.x + IndexPos.x + pos.x;

            IndexPos.y = TargetPos.transform.localPosition.y - GamePannel.transform.localPosition.y + IndexPos.y + pos.y;

            IndexPos.z = TargetPos.transform.localPosition.z - GamePannel.transform.localPosition.z + IndexPos.z + pos.z;

            //Roa记录
            IndexRoa.x = TargetPos.transform.rotation.x - GamePannel.transform.rotation.x + IndexRoa.x;

            IndexRoa.y = TargetPos.transform.rotation.y - GamePannel.transform.rotation.y + IndexRoa.y;

            IndexRoa.z = TargetPos.transform.rotation.z - GamePannel.transform.rotation.z + IndexRoa.z;

            IndexRoa.w = TargetPos.transform.rotation.w - GamePannel.transform.rotation.w + IndexRoa.w;

            if (NumCount >= SamplingNumber)
            {
                NumCount = 0;
                //Debug.Log("x :" + X_dev + "  y :" + Y_dev + "  z :" + Z_dev);
                MovePos = new Vector3(IndexPos.x / (float)Math.Pow(SamplingNumber, 2), IndexPos.y / (float)Math.Pow(SamplingNumber, 2), IndexPos.z / (float)Math.Pow(SamplingNumber, 2));

                MoveRoa = new Quaternion(IndexRoa.x / (float)Math.Pow(SamplingNumber, 2),
                    IndexRoa.y / (float)Math.Pow(SamplingNumber, 2), IndexRoa.z / (float)Math.Pow(SamplingNumber, 2), IndexRoa.w / (float)Math.Pow(SamplingNumber, 2));

                IndexPos = new Vector3(0, 0, 0);
                IndexRoa = new Quaternion(0, 0, 0, 0);
                MovCount = SamplingNumber;
            }

            if (Math.Abs(MovePos.x) > 0.001 || Math.Abs(MovePos.y) > 0.001 || Math.Abs(MovePos.z) > 0.001 && MovCount > 0)
            {
                MovCount -= 1;
                Vector3 new_pos = new Vector3(GamePannel.transform.localPosition.x + MovePos.x,
                    GamePannel.transform.localPosition.y + MovePos.y, GamePannel.transform.localPosition.z + MovePos.z);
                GamePannel.transform.localPosition = new_pos;
            }

            if (Math.Abs(MoveRoa.x) > 0.001 || Math.Abs(MoveRoa.y) > 0.001 || Math.Abs(MoveRoa.z) > 0.001 && MovCount > 0)
            {
                MovCount -= 1;
                Quaternion new_roa = new Quaternion(GamePannel.transform.rotation.x + MoveRoa.x,
                    GamePannel.transform.rotation.y + MoveRoa.y, GamePannel.transform.rotation.z + MoveRoa.z, GamePannel.transform.rotation.w + MoveRoa.w);
                GamePannel.transform.rotation = new_roa;
            }
        }
    }
}

