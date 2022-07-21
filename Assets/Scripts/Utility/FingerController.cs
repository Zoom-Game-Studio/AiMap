using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum FingerIE2
{
    zero,
    OneFinger,
    TwoFinger,
}
public class FingerController : MonoBehaviour
{
    public Vector3 initialRot;
    public Vector3 initialSca;
    public static FingerController instance;
    IEnumerator ie;
    FingerIE finger_num = FingerIE.zero;
    bool a = false;


    void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 mScreenPos = Input.mousePosition;//定义射线
                    Ray mRay = Camera.main.ScreenPointToRay(mScreenPos);
                    RaycastHit mHit;
                    if (Physics.Raycast(mRay, out mHit))//判断射线是否击中地面
                    {
                        Debug.DrawLine(Input.mousePosition, mHit.point, Color.red);//在Scene中生成这条射线
                        if (mHit.transform.gameObject.name == this.name)//检测碰撞到的物体
                        {
                            if (a == false)//控制不重复调用
                            {
                                StartCoroutine(OnMouse_Down());
                                a = true;
                            }
                        }
                    }
                    else//
                    {
                        if (finger_num != FingerIE.OneFinger)
                        {
                            if (ie != null)
                            {
                                StopCoroutine(ie);
                            }
                            ie = IMonitorMouseOneFinger();
                            StartCoroutine(ie);
                            finger_num = FingerIE.OneFinger;
                        }
                    }
                }
            }
        }
        else
        {
            a = false;
            if (Input.touchCount == 0)
            {
                if (finger_num != FingerIE.zero)
                {
                    StopCoroutine(ie);
                    ie = null;
                    finger_num = FingerIE.zero;
                }
            }
        }
    }


    IEnumerator OnMouse_Down()//物体跟随屏幕触摸移动
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// 一根手指控制转动
    /// </summary>
    /// <returns></returns>
    IEnumerator IMonitorMouseOneFinger()
    {
        Touch oneFingerTouch;
        while (true)
        {
            oneFingerTouch = Input.GetTouch(0);
            if (oneFingerTouch.phase == TouchPhase.Moved)
            {
                Vector2 deltaPos = oneFingerTouch.deltaPosition;
                //transform.Rotate(-Vector3.up * deltaPos.x * 0.2f, Space.Self);

                transform.Translate(new Vector3(deltaPos.x, 0, 0));
            }
            yield return 0;
        }
    }

    /// <summary>
    /// 复位
    /// </summary>
    public void ResetRot()
    {
        this.transform.localEulerAngles = initialRot;
        this.transform.localScale = initialSca;
    }
}
