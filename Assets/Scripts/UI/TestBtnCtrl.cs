using System.Collections;
using System.Collections.Generic;
using NatCorder.Examples;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AppLogic;
using LitJson;
using System.Text;
using System.Threading;

public class TestBtnCtrl : MonoBehaviour
{
    public CameraPreview cameraPreview;
    public MirrorFlipCamera mirrorFlipCamera;
    public TextMeshPro textMesh;
    public Text myText;
    public Transform buildingInfoObj;

    //private string json = "{\"buildings\":[{\"buidingGPS\":{\"lat\":31.177417,\"lon\":121.604488},\"buidingLocation\":{\"x\":3,\"y\":10,\"z\":30},\"detail\":\"盛趣大楼\",\"otherInfo\":{\"注册号\":\"310141000356839\",\"招聘信息\":\"研发、美术、策划\",\"企业经营状态\":\"开业\",\"企业联系电话\":\"021-50504740\",\"在职员工人数\":\"9999/人\",\"名称\":\"盛趣科技\",\"统一社会信用代码\":\"91310115MA1K3K4A54\",\"融资信息\":\"2019年01月07日，世纪华通以298亿并购盛跃网络\"},\"title\":\"海趣园\"}]}";
    private string json = "[{\"buidingGPS\":{\"lat\":31.177417,\"lon\":121.604488},\"buidingLocation\":{\"x\":3,\"y\":10,\"z\":30},\"detail\":\"盛趣大楼\",\"otherInfo\":{\"注册号\":\"310141000356839\",\"招聘信息\":\"研发、美术、策划\",\"企业经营状态\":\"开业\",\"企业联系电话\":\"021-50504740\",\"在职员工人数\":\"9999/人\",\"名称\":\"盛趣科技\",\"统一社会信用代码\":\"91310115MA1K3K4A54\",\"融资信息\":\"2019年01月07日，世纪华通以298亿并购盛跃网络\"},\"title\":\"海趣园\"}]";

    void Test()
    {
        //BuildingInfos buildingInfos = JsonMapper.ToObject<BuildingInfos>(json);
        //BuildingInfo[] buildingInfos = JsonMapper.ToObject<BuildingInfo[]>(json);
        //Debug.Log(buildingInfos.buildings[0].title);
        //Debug.Log(buildingInfos.buildings[0].otherInfo.名称);
        //SetBuildingInfo(buildingInfoObj, buildingInfos[0]);
    }
    void SetPOIInfo(Transform go, POIInfoInUnity poiInfoInUnity)
    {
        GameObject buildingTextDetailObj = UnityUtilties.FindChild(go, "BuildingTextDetail");
        if (buildingTextDetailObj != null)
        {
            Debug.Log("buildingTextDegail is not null");
            Text BuildingTextDetail = buildingTextDetailObj.GetComponent<Text>();
            if (BuildingTextDetail != null)
            {
                Debug.Log("desc:" + poiInfoInUnity.desc);
                BuildingTextDetail.text = poiInfoInUnity.desc;
            }
        }
        GameObject nameTextDetailObj = UnityUtilties.FindChild(go, "NameTextDetail");
        if (nameTextDetailObj != null)
        {
            Text nameTextDetail = nameTextDetailObj.GetComponent<Text>();
            if (nameTextDetail != null)
            {
                //nameTextDetail.text = poiInfoInUnity.name;
            }
        }

        GameObject iconImageObj = UnityUtilties.FindChild(go, "IconImage");
        if (iconImageObj != null)
        {
        }
    }
    void LoadABAsset()
    {
        string path = Application.streamingAssetsPath + "/louis";
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        GameObject go = ab.LoadAsset<GameObject>("Louis");
        GameObject.Instantiate<GameObject>(go);
    }

    void SetBuildingInfo(Transform go, PoisItem poi)
    {
        if (poi == null) return;
        if (go == null) return;
        go.name = poi.id;
        GameObject BuildingTextDetailObj = UnityUtilties.FindChild(go, "BuildingTextDetail");
        if (BuildingTextDetailObj != null)
        {
            Text BuildingTextDetail = BuildingTextDetailObj.GetComponent<Text>();
            if (BuildingTextDetail != null)
                BuildingTextDetail.text = poi.name;
        }

        GameObject nameTextDetailObj = UnityUtilties.FindChild(go, "NameTextDetail");
        if (nameTextDetailObj != null)
        {
            Text nameTextDetail = nameTextDetailObj.GetComponent<Text>();
            if (nameTextDetail != null)
                nameTextDetail.text = poi.name;
        }

        GameObject CreditCodeTextDetailObj = UnityUtilties.FindChild(go, "CreditCodeTextDetail");
        if (CreditCodeTextDetailObj != null)
        {
            Text CreditCodeTextDetail = CreditCodeTextDetailObj.GetComponent<Text>();
            if (CreditCodeTextDetail != null)
            {
                //CreditCodeTextDetail.text = building.otherInfo.统一社会信用代码;
            }
        }

        GameObject RegistrationNumberTextDetailObj = UnityUtilties.FindChild(go, "RegistrationNumberTextDetail");
        if (RegistrationNumberTextDetailObj != null)
        {
            Text RegistrationNumberTextDetail = RegistrationNumberTextDetailObj.GetComponent<Text>();
            if (RegistrationNumberTextDetail != null)
            {
                //RegistrationNumberTextDetail.text = building.otherInfo.注册号;
            }
        }

        GameObject OperatingStatusTextDetailObj = UnityUtilties.FindChild(go, "OperatingStatusTextDetail");
        if (OperatingStatusTextDetailObj != null)
        {
            Text OperatingStatusTextDetail = OperatingStatusTextDetailObj.GetComponent<Text>();
            if (OperatingStatusTextDetail != null)
            {
                //OperatingStatusTextDetail.text = building.otherInfo.企业经营状态;
            }
        }

        GameObject PhoneNumberTextDetailObj = UnityUtilties.FindChild(go, "PhoneNumberTextDetail");
        if (PhoneNumberTextDetailObj != null)
        {
            Text PhoneNumberTextDetail = PhoneNumberTextDetailObj.GetComponent<Text>();
            if (PhoneNumberTextDetail != null)
            {
                //PhoneNumberTextDetail.text = building.otherInfo.企业联系电话;
            }
        }
    }

    private void OnEnable()
    {
        //InvokeRepeating("TestInvoke", 0, 1);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void TestInvoke()
    {
        Debug.Log("Invoke");
    }
    void Start()
    {
       Transform a= transform.Find("aaaa");
        if (a == null) Debug.Log("ddd");
        a.gameObject.SetActive(true);
        return;
        //SetPOIInfo(transform,null);
        string json = "{\"type\":\"1\",\"url\":\"我是一个题目，哈哈哈哈\"}";
        //JsonData jsonData = JsonMapper.ToObject(json);
        //Debug.Log(jsonData["type"].ToString());
        //return;
        byte[] bytes = Encoding.Default.GetBytes(json);
        json = Encoding.UTF8.GetString(bytes);
        UrlFromUnity urlFromUnity = JsonUtility.FromJson<UrlFromUnity>(json);

        //UrlFromUnity urlFromUnity = JsonMapper.ToObject<UrlFromUnity>(json);
        Debug.Log(urlFromUnity.url);
    }
    public IEnumerator DelayToStart()
    {
        SetMaterialRenderingQueue(3000);

        yield return new WaitForSeconds(1);
        SetMaterialRenderingQueue(2001);
    }
    public void SetMaterialRenderingQueue(int queue)
    {
        GetComponent<MeshRenderer>().material.renderQueue = queue;
    }
    public IEnumerator DelayShowModel(GameObject model)
    {
        //model.gameObject.layer = 8;//layer=8模型不显示
        model.layer = LayerMask.NameToLayer("Ignore"); //指定Layer
        print("model.layer: " + model.layer);
        model.SetActive(false);

        yield return new WaitForSecondsRealtime(2);
        model.SetActive(true);
        //model.gameObject.layer = 0;//显示
        //model.SetActive(true);
        //model.SetActive(false);//关闭模型的显示++++
    }
    bool isFront = false;
    void OnButtonClicked()
    {
        isFront = !isFront;
        print("isFront" + isFront);
        cameraPreview.StopDefaultCamera();
        cameraPreview.useFrontCamera = isFront;
        StartCoroutine(cameraPreview.StartCamera());
        mirrorFlipCamera.flipHorizontal = isFront;
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(5.0f);
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        return;
        //textMesh.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.position);
        textMesh.transform.LookAt(Camera.main.transform.position);
        Quaternion q = Quaternion.LookRotation(Camera.main.transform.position);

        Vector3 euler = textMesh.transform.eulerAngles;
        textMesh.transform.localEulerAngles = new Vector3(0, euler.y, 0);
    }
}