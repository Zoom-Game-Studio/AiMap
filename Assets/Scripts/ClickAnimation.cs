using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAnimation : MonoBehaviour, IPointerClickHandler
{
    public enum Identity
    {
        None,
        Earth,
        Astronaut,
        CoffeeCup,
        TopicButton
    }

    [Header("点击地球摇动的距离范围")] [Range(0, 100)]
    public float EarthPopDistance = 1;

    [Space(20)] [Header("点击宇航员弹出的距离范围")] [Range(0, 50)]
    public float AstronautPopDistance = 1;

    private int ClickTimes = 0;
    public GameObject Target;
    public GameObject UIButton;
    private bool TargetStatus;
    public Identity identity;
    private GameObject ChangeButton;
    GameObject myGO;
    Canvas myCanvas;
    [SerializeField] public Sprite[] ButtonSprites;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (identity)
        {
            case Identity.None:
                return;
                break;
            case Identity.Astronaut:
                AstronautMotion();
                break;
            case Identity.Earth:
                EarthMotion();
                break;
            case Identity.CoffeeCup:
                OnClickCoffeeCup();
                break;
            case Identity.TopicButton:
                OnPillarChangeTopic();
                break;
        }
    }

    //宇航员的点击动画
    public void AstronautMotion()
    {
        Vector3 vec = (transform.position - Camera.main.transform.position).normalized * AstronautPopDistance;
        if (transform.GetComponent<AudioSource>().isPlaying)
        {
            transform.GetComponent<AudioSource>().Stop();
        }

        transform.GetComponent<AudioSource>().Play();
        transform.DOMove(transform.position + vec, 2).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            transform.DOMove(transform.position - vec, 2);
        });
    }

    //地球的点击动画
    public void EarthMotion()
    {
        Vector3 vec = (transform.position - Camera.main.transform.position).normalized * EarthPopDistance;
        transform.DOMove(transform.position + vec, 1).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            transform.DOMove(transform.position - vec, 1);
        });
    }

    //咖啡杯点击事件
    public void OnClickCoffeeCup()
    {
        ClickTimes++;
        Target.SetActive(ClickTimes % 2 == 1);
        //  TargetStatus = !TargetStatus;
    }

    /// <summary>
    /// 立柱切换风格按钮事件
    /// </summary>
    public void OnPillarChangeTopic()
    {
        for (int i = 0; i < Target.transform.childCount; i++)
        {
            Target.transform.GetChild(i).gameObject.SetActive(i == 1);

            Target.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            Target.transform.GetChild(i).GetChild(0).position = Camera.main.transform.position +
                                                                Camera.main.transform.forward * 3 + new Vector3(0,
                                                                    -Camera.main.transform.forward.y, 0);
            Target.transform.GetChild(i).GetChild(0).LookAt(new Vector3(Camera.main.transform.position.x,
                Target.transform.GetChild(i).position.y, Camera.main.transform.position.z));
        }

        ClickTimes++;
        ChangeButton.SetActive(true);
        ChangeButton.GetComponent<Button>().image.sprite = ButtonSprites[1];
        ChangeButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            for (int i = 1; i < Target.transform.childCount; i++)
            {
                Target.transform.GetChild(i).gameObject.SetActive(ClickTimes % 5 == i);


                Target.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                Target.transform.GetChild(i).GetChild(0).position = Camera.main.transform.position +
                                                                    Camera.main.transform.forward * 3 +
                                                                    new Vector3(0, -Camera.main.transform.forward.y, 0);
                Target.transform.GetChild(i).GetChild(0).LookAt(new Vector3(Camera.main.transform.position.x,
                    Target.transform.GetChild(i).position.y, Camera.main.transform.position.z));
            }

            ClickTimes++;

            if (ClickTimes % 5 == 0)
            {
                ClickTimes++;
            }

            ChangeButton.GetComponent<Button>().image.sprite = ButtonSprites[ClickTimes % 5 - 1];
        });
    }


    // Start is called before the first frame update
    void Start()
    {
        if (Target == null)
        {
            Target = this.gameObject;
        }

        switch (identity)
        {
            case Identity.CoffeeCup:
                TargetStatus = Target == this.gameObject ? true : false;
                Target.SetActive(TargetStatus);
                break;
            case Identity.TopicButton:
                Target.SetActive(true);
                for (int i = 0; i < Target.transform.childCount; i++)
                {
                    Target.transform.GetChild(i).gameObject.SetActive(ClickTimes % 5 == i);
                }

                ClickTimes++;

                if (UIButton != null)
                {
                    myGO = Instantiate(UIButton) as GameObject;
                    ChangeButton = myGO.GetComponentInChildren<Button>().gameObject;
                    ChangeButton.SetActive(false);
                }

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}