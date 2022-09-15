using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpriteImageFadeOutIn : MonoBehaviour
{
    SpriteRenderer[] images_ImageFadeOutIn;

    AudioSource audioSource1;
    Animator[] animators;

    GameObject joker;
    GameObject redDoor;
    GameObject shop;
    GameObject cars;
    GameObject teddyBear;

    public float speed_ImageFadeOutIn = 1;
    bool isOpen = false;
    float timer = 0f;

    void Start()
    {
        images_ImageFadeOutIn = GameObject.FindObjectsOfType<SpriteRenderer>();

        animators = GameObject.Find("superdemo_001").GetComponentsInChildren<Animator>();
        audioSource1 = GameObject.Find("superdemo_001").GetComponent<AudioSource>();

        joker = GameObject.Find("Joker");
        redDoor = GameObject.Find("redDoor");
        shop = GameObject.Find("Shops");
        cars = GameObject.Find("Æû³µ");
        teddyBear = GameObject.Find("teddyBear");

        joker.transform.localScale = new Vector3(0, 0, 0);
        redDoor.transform.localScale = new Vector3(0, 0, 0);
        shop.SetActive(false);
        cars.SetActive(false);
        teddyBear.SetActive(false);

        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = false;
        }

        for (int i = 0; i < images_ImageFadeOutIn.Length; i++)
        {
            images_ImageFadeOutIn[i].color = new Color(1, 1, 1, 0);
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "LedDanceFloor01")
                {
                    if (!audioSource1.isPlaying)
                    {
                        audioSource1.Play();
                        for (int i = 0; i < animators.Length; i++)
                        {
                            animators[i].enabled = true;
                        }
                    }
                    else
                    {
                        audioSource1.Pause();
                        for (int i = 0; i < animators.Length; i++)
                        {
                            animators[i].enabled = false;
                        }
                    }
                }
                if (hit.collider.gameObject.name == "Cube_table")
                {
                    if (!isOpen)
                    {
                        isOpen = true;
                    }
                    else
                    {

                    }
                }
            }
        }

        if (isOpen)
        {
            if (timer < 1)
            {
                timer += (speed_ImageFadeOutIn * Time.fixedDeltaTime);
                for (int i = 0; i < images_ImageFadeOutIn.Length; i++)
                {
                    images_ImageFadeOutIn[i].color = new Color(1, 1, 1, timer);
                }
                joker.transform.localScale = new Vector3(timer, timer, timer);
                redDoor.transform.localScale = new Vector3(timer, timer, timer);
                shop.SetActive(true);
                cars.SetActive(true);
                teddyBear.SetActive(true);
            }
        }
        else
        {
            //if (timer > 0)
            //{
            //    timer -= (speed_ImageFadeOutIn * Time.fixedDeltaTime);
            //    for (int i = 0; i < images_ImageFadeOutIn.Length; i++)
            //    {
            //        images_ImageFadeOutIn[i].color = new Color(1, 1, 1, timer);
            //    }
            //}
        }
    }
}
