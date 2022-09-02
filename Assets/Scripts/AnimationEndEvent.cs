using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEndEvent : MonoBehaviour
{
    Animation Animation;
    // Start is called before the first frame update
    void Start()
    {
        Animation = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Animation.isActiveAndEnabled &&!Animation.isPlaying) {
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
