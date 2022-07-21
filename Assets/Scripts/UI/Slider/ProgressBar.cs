using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mopsicus.TwinSlider;
using System;

//进度条控制：默认数值0-1
public class ProgressBar : MonoBehaviour
{
    private Animator[] animator_;//普通模型动画
    Animator model_;//普通模型
    AudioSource[] audioSources;//普通模型声音
    AudioSource AudioSource_;//8i模型声音
    string modelType_;//模型类型
    bool isStart;
    bool isDown;
    [SerializeField]
    private TwinSlider TwinSlider;
    public bool isPress;//是否拖动
    float strValue;
    public bool isOne;//是否第一个
    public int CyclSetate;//循环状态
    public float curSpeed;//速度值
    public Image OneHandle;
    public Image TwoHandle;
    public GameObject Handle2;
    float TotalTime;//总时间
    float CurrentTime;//当前时间
    bool isStop;
    public Image LoopImage;//循环按钮组件
    public Sprite NoLoop;
    public Sprite Loop;
    public Sprite ABLoop;
    public Image PlayAnimatorBtn;
    public Sprite Play_;
    public Sprite Pause_;

    void Start()
    {
        TwinSlider.OnSliderChange += OnValuesChanges;//滑块更改时的回调
        //AREventUtil.AddListener(GlobalOjbects.INIT_SLIDER_VALUE, InitSliderValue);
        model_ = new Animator();
        TwinSlider.Max = 1;
        TwinSlider.Min = 0;
        TwinSlider.SliderOne.value = 0;
        TwinSlider.SliderTwo.value = 1;
        //slider_.minValue = 0;//最小值
        //slider_.maxValue = 1;//最大值
        curSpeed = 1;
    }

    private void OnDestroy()
    {
        //AREventUtil.RemoveListener(GlobalOjbects.INIT_SLIDER_VALUE, InitSliderValue);
    }

    /// <summary>
    /// 重置Slider相应事件
    /// </summary>
    /// <param name="eventArgs">Event arguments.</param>
    private void InitSliderValue(AREventArgs eventArgs)
    {
        InitSlider();
    }

    /// <summary>
    /// 重置slider的值
    /// </summary>
    public void InitSlider()
    {
        TwinSlider.SliderOne.value = 0;
        TwinSlider.SliderTwo.value = 1;
        strValue = 0;
        Vector3 aa = ResetValue();
        Handle2.transform.localPosition = aa;
    }
    public string ReturnModelType()
    {
        return modelType_;
    }
    float first = 1f;
    float second = 0f;
    public void AnimatorController()//开始进度条
    {
        print("进度条。。。:" + modelType_);
        ///
        ///*******关闭普通模型*******只显示8i模型
        ///
        if (modelType_ != "1" && modelType_ != "4")
        {
            modelType_ = "15";
        }
        if (modelType_ == "5")//普通模型
        {
            print("modelType...==5");
            print("普通模型。。。");
            {
                if (animator_.Length >= 1)
                {
                    second = animator_[0].GetCurrentAnimatorStateInfo(0).length;
                    model_ = animator_[0];
                    for (int i = 0; i < animator_.Length; i++)
                    {
                        // print("animator_");
                        //动画总长度:animator_[i].GetCurrentAnimatorStateInfo(0).length);
                        first = animator_[i].GetCurrentAnimatorStateInfo(0).length;
                        // print("first"+ first);
                        // //print("second" + second);
                        if (first >= second)
                        {
                            second = first;
                            model_ = animator_[i];
                            print("model_... " + model_.name);
                        }
                        else
                        {
                            print("model_...不存在 ");
                        }
                    }
                    //slider_.maxValue = second;//最大值
                }
                isStart = true;
                isDown = true;
            }
            DisplayObject();
        }
        if (modelType_ == "1" || modelType_ == "4")//8i模型
        {
           
                print("空。。。");
                if (IsInvoking("AnimatorController"))//是否被延时，即将执行
                {
                    CancelInvoke("AnimatorController");//取消延时
                }
                Invoke("AnimatorController", 1f);
        }
    }
    float Border_;
    void DisplayObject()//显示所有关闭子物体
    {
        //OneHandle.color = new Color(1, 0.5f, 0.5f, 0);//红色透明
        TwoHandle.color = new Color(1, 1, 1, 0);//透明
        transform.Find("Background").gameObject.SetActive(true);
        transform.Find("Filler").gameObject.SetActive(true);
        transform.Find("SliderOne").gameObject.SetActive(true);
        transform.Find("SliderTwo").gameObject.SetActive(true);
        PlayAnimatorBtn.gameObject.SetActive(true);
        isPlaying = true;
        //改变滑块之间的边界限制
        if (modelType_ == "1" || modelType_ == "4")
        {
            Border_ = (TotalTime / 50) / TotalTime;
            TwinSlider.Border = Border_;
        }
        if (modelType_ == "5")
        {
            Border_ = (second / 50) / second;
            TwinSlider.Border = Border_;
        }
        print("Border_: " + Border_);
    }
    bool isSet = true;
    void Update()
    {
        if (isStart)
        {
            if (isDown)
            {
                //normalizedTime：整数部分是状态循环的次数，小数部分是动画循环中进度的百分比
                //除1取余取掉小数点前面部分，得到当前进度的百分比值
                //slider_.value = (model_.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1;
                if (!isPress)
                {
                    if (CyclSetate == 1)//单次循环
                    {
                        //  print("CyclSetate == 1");
                        LoopImage.sprite = NoLoop;
                        // print("CyclSetate == 1....");
                        Handle2.SetActive(false);
                        //OneHandle.color = new Color(1, 0.5f, 0.5f, 0);//红色透明
                        TwoHandle.color = new Color(1, 1, 1, 0);//透明
                        TwinSlider.SliderTwo.value = 1;
                        TwinSlider.SliderTwo.interactable = false;//禁止拖动
                        if (modelType_ == "1" || modelType_ == "4")
                        {
                           
                        }
                        if (modelType_ == "5")
                        {
                            // print("modelType_ == 5");
                            if (model_.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.99f)//动画播完一次
                            {
                                //print("动画播完一次");
                                model_.enabled = false;//
                                model_.enabled = true;
                                //model_.Play(0);
                                model_.Play(model_.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, 0);
                                model_.speed = 0;//暂停动画
                                foreach (var audio in audioSources)
                                {
                                    audio.time = 0;
                                    audio.Stop();
                                }
                                TwinSlider.SliderOne.value = 1;
                                //暂停状态
                                isPlaying = false;
                                PlayAnimatorBtn.sprite = Pause_;
                            }
                        }
                    }
                    //CyclSetate_23();
                    if (CyclSetate == 2)//循环
                    {
                        //  print("CyclSetate == 2");
                        LoopImage.sprite = Loop;
                        //  print("CyclSetate == 2....");
                        if (isChange2)
                        {
                            if (modelType_ == "1" || modelType_ == "4")
                            {
                                if (curSpeed == 1)
                                {
                                    AudioSource_.volume = 1;
                                }
                             
                            }
                            if (modelType_ == "5")
                            {
                                foreach (var audio in audioSources)
                                {
                                    if (curSpeed == 1)
                                    {
                                        audio.volume = 1;
                                    }
                                }
                                model_.speed = 1;
                            }
                            isChange2 = false;
                        }
                    }
                    if (CyclSetate == 3)//AB循环
                    {
                        //   print("CyclSetate ==3");
                        LoopImage.sprite = ABLoop;
                        TwinSlider.SliderTwo.interactable = true;
                        Handle2.SetActive(true);
                        TwoHandle.color = new Color(1, 1, 1, 1);
                        //print("strValue： " + strValue);
                        //print("SliderOne.value： " + TwinSlider.SliderOne.value);
                        //print("SliderTwo.value;： " + TwinSlider.SliderTwo.value);
                        if (strValue.Equals(0))
                        {
                            strValue += 0.01f;
                        }
                        if (TwinSlider.SliderOne.value <= strValue)
                        {
                            print("一次循环。。。");
                            TwinSlider.SliderOne.value = TwinSlider.SliderTwo.value;
                            //print("SliderOne.value: " + TwinSlider.SliderOne.value);
                            if (modelType_ == "1" || modelType_ == "4")
                            {
                                //if (isStop)
                                //{
                                float seek = TotalTime * (1 - TwinSlider.SliderOne.value);
                                if (AudioSource_.clip != null)
                                {
                                    AudioSource_.time = AudioSource_.clip.length * (1 - TwinSlider.SliderOne.value);
                                    AudioSource_.Play();
                                }

                                isStop = false;
                                //}
                            }
                            if (modelType_ == "5")
                            {
                                //print("再一次。。11");
                                foreach (var audio in audioSources)
                                {
                                    audio.time = audio.clip.length * (1 - TwinSlider.SliderOne.value);
                                    audio.Play();
                                }
                                model_.enabled = false;
                                model_.enabled = true;
                                model_.Play(model_.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, 1 - TwinSlider.SliderOne.value);
                                model_.speed = 1;
                            }
                        }
                        //if (isChange2)
                        //{
                        //    isChange2 = false;
                        //}
                    }
                    //进度条控制
                    if (modelType_ == "1" || modelType_ == "4")
                    {
                        if (curSpeed == 1)
                        {
                            AudioSource_.volume = 1;//打开音量
                        }
                        //print("CurrentTime: " + CurrentTime);
                        if (CurrentTime > 0)
                        {
                            TwinSlider.SliderOne.value = 1 - (CurrentTime / TotalTime);//第一个滑块的进度
                            //isStop = true;
                        }
                        else
                        {
                            TwinSlider.SliderOne.value = TwinSlider.SliderTwo.value;
                            //hvrActor_.assetInterface.Stop();
                            //float seek = TotalTime * (1 - TwinSlider.SliderOne.value);
                            //hvrActor_.assetInterface.Seek(seek);//从指定位置播放
                        }
                    }
                    if (modelType_ == "5")
                    {
                        if (model_ != null)
                        {
                            TwinSlider.SliderOne.value = 1 - (model_.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1;//第一个滑块的进度
                        }
                        else
                        {

                            // print("model_ is null");
                        }
                        foreach (var audio in audioSources)
                        {
                            if (curSpeed == 1)
                            {
                                audio.volume = 1;
                            }
                        }
                    }
                    if (isChange)
                    {
                        isChange = false;
                    }
                }
            }
            else
            {
                if (CyclSetate == 3)//AB循环
                {
                    if (TwinSlider.SliderTwo.value - TwinSlider.SliderOne.value > Border_)
                    {
                        if (modelType_ == "1" || modelType_ == "4")
                        {
                            AudioSource_.volume = 0;//静音
                            if (isOne)
                            {
                                if (AudioSource_.clip != null)
                                    AudioSource_.time = AudioSource_.clip.length * TwinSlider.SliderOne.value;
                            }
                            else
                            {
                                if (AudioSource_.clip != null)
                                    AudioSource_.time = AudioSource_.clip.length * TwinSlider.SliderTwo.value;
                            }
                        }
                        if (modelType_ == "5")
                        {
                            if (isOne)
                            {
                                foreach (var audio in audioSources)
                                {
                                    if (audio.clip == null) continue;
                                    audio.volume = 0;
                                    audio.time = audio.clip.length * TwinSlider.SliderOne.value;
                                }
                                model_.Play(model_.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, TwinSlider.SliderOne.value);
                            }
                            else
                            {
                                foreach (var audio in audioSources)
                                {
                                    if (audio.clip == null) continue;
                                    audio.volume = 0;
                                    audio.time = audio.clip.length * (TwinSlider.SliderTwo.value);
                                }
                                model_.Play(model_.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, TwinSlider.SliderTwo.value);
                            }
                        }
                    }
                    else
                    {
                        print("等于 Border_");
                    }
                }
                else//其他循环
                {
                    if (modelType_ == "1" || modelType_ == "4")
                    {
                        AudioSource_.volume = 0;//静音
                        if (AudioSource_.clip != null)
                            AudioSource_.time = AudioSource_.clip.length * TwinSlider.SliderOne.value;
                    }
                    if (modelType_ == "5")
                    {
                        foreach (var audio in audioSources)
                        {
                            if (audio.clip == null) continue;
                            audio.volume = 0;
                            audio.time = audio.clip.length * TwinSlider.SliderOne.value;
                        }
                        model_.Play(model_.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, TwinSlider.SliderOne.value);
                    }
                }
            }
        }
    }
    void OnValuesChanges(float value_1, float value_2)//滑块更改时的回调
    {
        if (isPress)
        {
            if (IsInvoking("PlayAnim"))
            {
                CancelInvoke("PlayAnim");
            }
            isDown = false;
            if (modelType_ == "1" || modelType_ == "4")
            {
                AudioSource_.Stop();//
            }
            if (modelType_ == "5")
            {
                model_.speed = 0;//暂停动画
                foreach (var audio in audioSources)
                {
                    audio.Stop();
                }
            }
            Invoke("PlayAnim", 0.5f);
        }
    }
    public void strValue_()
    {
        if (isOne)
        {
            strValue = TwinSlider.SliderOne.value;//标记
            print("重置strValue： " + strValue);
        }
    }
    public void PlayAnim()//播放动画
    {
        print("标记: " + strValue);
        isDown = true;//
        if (modelType_ == "1" || modelType_ == "4")
        {
            if (CyclSetate == 3)
            {
                if (TwinSlider.SliderTwo.value - TwinSlider.SliderOne.value <= (Border_ + 0.02))//等于边界限制+0.02
                {
                    TwinSlider.SliderOne.value -= 0.05f;
                    strValue -= 0.05f;
                    Vector3 aa = ResetValue_();
                    Handle2.transform.localPosition = aa;
                }
                if (AudioSource_.clip != null)
                    AudioSource_.time = AudioSource_.clip.length * (1 - TwinSlider.SliderTwo.value);
            }
            else
            {
                //PlayAnimatorBtn.SetActive(false);
                if (AudioSource_.clip != null)

                    AudioSource_.time = AudioSource_.clip.length * (1 - TwinSlider.SliderOne.value);
            }
        }
        if (modelType_ == "5")
        {
            if (CyclSetate == 3)
            {
                if (TwinSlider.SliderTwo.value - TwinSlider.SliderOne.value <= (Border_ + 0.02))//等于边界限制
                {
                    TwinSlider.SliderOne.value -= 0.05f;
                    strValue -= 0.05f;
                    Vector3 aa = ResetValue_();
                    Handle2.transform.localPosition = aa;
                }
                foreach (var audio in audioSources)
                {
                    if (audio.clip == null) continue;
                    audio.time = audio.clip.length * (1 - TwinSlider.SliderTwo.value);
                }
                model_.Play(model_.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, 1 - TwinSlider.SliderTwo.value);//动画名 层级 进度比
            }
            else
            {
                //PlayAnimatorBtn.SetActive(false);
                model_.Play(model_.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, 1 - TwinSlider.SliderOne.value);//动画名 层级 进度比
                foreach (var audio in audioSources)
                {
                    if (audio.clip == null) continue;
                    audio.time = audio.clip.length * (1 - TwinSlider.SliderOne.value);
                }
            }
        }
        PlayAnima();
        isChange = true;
        isChange2 = true;
    }
    bool isPlaying;
    public void AnimatorBtn()//播放按钮
    {
        if (isPlaying)
        {
            PauseAnima();
        }
        else
        {
            PlayAnima();
        }
    }
    void PlayAnima()//播放
    {
        if (modelType_ == "1" || modelType_ == "4")
        {
            AudioSource_.Play();
        }
        if (modelType_ == "5")
        {
            foreach (var audio in audioSources)
            {
                audio.Play();
            }
            model_.speed = 1;
        }
        isPlaying = true;
        PlayAnimatorBtn.sprite = Play_;
    }
    void PauseAnima()//暂停
    {
        if (modelType_ == "1" || modelType_ == "4")
        {
            AudioSource_.Pause();
        }
        if (modelType_ == "5")
        {
            foreach (var audio in audioSources)
            {
                audio.Pause();
            }
            model_.speed = 0;
        }
        isPlaying = false;
        PlayAnimatorBtn.sprite = Pause_;
    }
    //每次切换循环状态后调用
    bool isChange;
    bool isChange2;
    public void Playstatus()//播放状态
    {
        if (modelType_ == "15")//关闭进度条后显示状态
        {
            if (CyclSetate == 1)
            {
                LoopImage.sprite = NoLoop;
            }
            else
            {
                LoopImage.sprite = Loop;
            }
        }
        else//打开
        {
            isPlaying = true;
            PlayAnimatorBtn.sprite = Play_;
            if (CyclSetate == 3)
            {
                Vector3 aa = ResetValue();
                Handle2.transform.localPosition = aa;
                strValue_();
            }
            isChange = true;
            isChange2 = true;
        }
    }
    public Vector3 ResetValue()//初始B进度条
    {
        TwinSlider.SliderOne.value = 0;
        Vector3 aa = OneHandle.gameObject.transform.localPosition;
        return aa;
    }
    Vector3 ResetValue_()//重置进度条
    {
        Vector3 aa = OneHandle.gameObject.transform.localPosition;
        return aa;
    }
    public void ResetSlider()//重置滑块位置
    {
        TwinSlider.SliderOne.value = strValue;
        print("strValue2: " + strValue);
    }
}
