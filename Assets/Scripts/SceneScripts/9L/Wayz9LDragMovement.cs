using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum FingerIE
{
    zero,
    OneFinger,
    TwoFinger,
}
public class Wayz9LDragMovement : MonoBehaviour
{
    public Vector3 initialRot;
    public Vector3 initialSca;
    public static Wayz9LDragMovement instance;
    IEnumerator ie;
    FingerIE finger_num = FingerIE.zero;
    bool a = false;
    Vector3 startPos;
    public GameObject tron6;
    public GameObject tron5;
    private Vector3 maxVec3;
    private Vector3 minVec3;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startPos = transform.localPosition;
        Debug.Log(startPos);
        maxVec3 = new Vector3(startPos.x, startPos.y, startPos.z + 1);
        minVec3 = new Vector3(startPos.x, startPos.y, startPos.z - 1);
        Debug.Log(maxVec3);
        Debug.Log(minVec3);


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
                                Debug.Log("111");
                                //StartCoroutine(OnMouse_Down());
                                a = true;
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
                    else//
                    {
                        if (finger_num != FingerIE.OneFinger)
                        {
                           
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
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector3 offset = transform.localPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0));
        while (Input.GetMouseButton(0))
        {
            //Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, 0, 0);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.localPosition = curPosition;
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    ///移动
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
                float deltaPosX = deltaPos.y * 0.01f;

                if (deltaPosX >= 1)
                {
                    deltaPosX = 1;
                    tron6.SetActive(true);
                    tron5.SetActive(false);
                    transform.localPosition = new Vector3(startPos.x, startPos.y, startPos.z + 0.5f);
                    break;
                }

                if (deltaPosX < -1)
                {
                    deltaPosX = -1;
                    tron6.SetActive(false);
                    tron5.SetActive(true);
                    transform.localPosition = new Vector3(startPos.x, startPos.y, startPos.z - 0.5f);
                    break;
                }
                tron6.SetActive(true);
                tron5.SetActive(true);
                if (transform.localPosition.z <= maxVec3.z && transform.localPosition.z > minVec3.z)
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + deltaPosX);
                //transform.Translate(new Vector3(0, 0, deltaPosX), Space.Self);
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