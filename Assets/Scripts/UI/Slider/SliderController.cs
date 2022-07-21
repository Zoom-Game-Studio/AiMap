using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//给Slider添加拖拽事件
public class SliderController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ProgressBar ProgressBar;
    public GameObject Handle_;
    public GameObject Handle_2;
    bool isDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //给Slider添加开始拖拽事件
    public void OnDrag(PointerEventData eventData)
    {
        ProgressBar.isPress = true;
        if (gameObject.name== "SliderOne")//移动第一个Slider显示标记
        {
            ProgressBar.isOne = true;
            if (ProgressBar.CyclSetate == 3)
            {
                Handle_2.SetActive(true);
                Handle_2.transform.localPosition = Handle_.transform.localPosition;
                ProgressBar.strValue_();
            }
            else
            {
                if (!isDown)
                {
                    Vector3 aa = ProgressBar.ResetValue();
                    Handle_2.transform.localPosition = aa;
                    ProgressBar.strValue_();
                    isDown = true;
                }
            }
        }
        if (gameObject.name == "SliderTwo")//移动第二个Slider显示标记
        {
            ProgressBar.isOne = false;
            if (ProgressBar.CyclSetate == 3)
            {
                ProgressBar.ResetSlider();
            }
        }
    }
    //给Slider添加结束拖拽事件
    public void OnEndDrag(PointerEventData eventData)
    {
        //if (ProgressBar.CyclSetate == 3)
        //{
        //    Handle_.transform.localPosition = Handle_2.transform.localPosition;
        //}
        ProgressBar.isPress = false;
        isDown = false;
    }
}
