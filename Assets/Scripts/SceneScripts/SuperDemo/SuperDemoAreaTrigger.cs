using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AppLogic
{
    public class SuperDemoAreaTrigger : MonoBehaviour
    {
        private Camera mainCamera = null;
        private Transform contentChild;
        private Transform bubble;
        public float distance;
        private bool isInit = false;

        private void Awake()
        {
            mainCamera = Camera.main;
        }
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name.Contains("Bubble"))
                {
                    bubble = child;
                }
                else
                {
                    contentChild = child;
                    child.gameObject.SetActive(false);
                }
            }
        }

        private void OnEnable()
        {
            InvokeRepeating("AreaTrigger", 0, 1);
        }
        private void OnDisable()
        {
            CancelInvoke();
        }


        void StartDistance()
        {
            AreaTrigger();
        }


        void AreaTrigger()
        {
            if (mainCamera == null) return;

            if (Vector3.Distance(mainCamera.transform.position, this.transform.position) < distance)
            {
                SetChildActive(true);
            }
            else
            {
                SetChildActive(false);
            }
        }

        void SetChildActive(bool active)
        {
            if (contentChild == null) return;
            contentChild.gameObject.SetActive(active);
            if (bubble == null) return;
            bubble.gameObject.SetActive(!active);
        }

        

    }

}