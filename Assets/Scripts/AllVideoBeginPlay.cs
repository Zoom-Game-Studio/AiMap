using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AllVideoBeginPlay : MonoBehaviour
{
    VideoPlayer[] videoPlayers1;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayers1 = GameObject.FindObjectsOfType<VideoPlayer>();
        for (int i = 0; i < videoPlayers1.Length; i++)
        {
            videoPlayers1[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Cube_table")
                {
                    if (!videoPlayers1[0].isPlaying)
                    {
                        for (int i = 0; i < videoPlayers1.Length; i++)
                        {
                            videoPlayers1[i].gameObject.SetActive(true);
                            videoPlayers1[i].Play();
                        }
                    }
                }
            }
        }
    }
}
