using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AppLogic
{
    public class BulletMovement : MonoBehaviour
    {
        Camera camera;
        Vector3 finalPos = Vector3.zero;

        void Start()
        {
            camera = Camera.main;
            if (camera == null) return;
            finalPos = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z+5);
            transform.position = camera.transform.position;
            transform.LookAt(camera.transform.position);
        }

        void Update()
        {
            if (camera == null) return;
            transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime);
            if (Vector3.Distance(finalPos, transform.position)<1)
            {
                Destroy(this);
            }
        }
    }
}
