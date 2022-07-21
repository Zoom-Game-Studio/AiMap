using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDistance : MonoBehaviour
{
    Text distanceTextDetail;
    Camera main;
    private void Start()
    {
        GameObject distanceTextDetailObj = UnityUtilties.FindChild(transform, "DistanceTextDetail");
        if (distanceTextDetailObj != null)
        {
            distanceTextDetail = distanceTextDetailObj.GetComponent<Text>();
            if (distanceTextDetail != null)
            {
                InvokeRepeating("DistanceChange", 0, 2f);
            }
        }
        main = Camera.main;
    }

    void DistanceChange()
    {
        float distance = Vector3.Distance(main.transform.position, transform.position);
        distanceTextDetail.text = string.Format("{0:0}m", distance);
    }

}
