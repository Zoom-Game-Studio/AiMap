using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wayz9LSandBoxClick : MonoBehaviour
{
    public Camera camera01;
    //public GameObject SandBoxInfo;
    public GameObject SandBox;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = camera01.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Transform SandBoxInfo = objectHit.Find("01");
            SandBoxInfo.localScale = new Vector3(0.4387666f, 0.2247704f, 0.5922861f);
            Debug.Log("Hiii");

            // ������Ͷ�����еĶ���ִ��һЩ������
        }
    }
}
