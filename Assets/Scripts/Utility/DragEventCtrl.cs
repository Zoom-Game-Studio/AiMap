
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragEventCtrl : MonoBehaviour
{
    public Transform target { get; set; }
    private float speed = 1f;
    bool uiTouchedFlag = false;
    private Vector3 centerPos;
    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        Rotate();
    }

    private void Rotate() //摄像机围绕目标旋转操作
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                uiTouchedFlag = false;
                Debug.Log("当前触摸在UI上");
            }
            else
            {
                uiTouchedFlag = true;
                Debug.Log("当前没有触摸在UI上");
            }
        }

        if (!uiTouchedFlag) return;
        //没有触摸，就是触摸点为0
        if (Input.touchCount <= 0)
        {
            return;
        }
        Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = touch.deltaPosition * 0.15f;
        var mouse_x = deltaPos.x;//获取鼠标X轴移动
                                 //var mouse_y = -deltaPos.y;//获取鼠标Y轴移动
        float  X= transform.localPosition.x + mouse_x * speed;
        transform.localPosition += new Vector3(X, 0, 0);
    }
}