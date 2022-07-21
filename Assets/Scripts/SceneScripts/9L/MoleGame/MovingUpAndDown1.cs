using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingUpAndDown1 : MonoBehaviour
{
    Vector3 oriVector;
    Vector3 finalVector;
    public float amplitude = 2;
    public float rate = 1f;

    Vector3 revertPos;

    private void Awake()
    {
        oriVector = transform.localPosition;
        revertPos = transform.localPosition;
    }

    private void Update()
    {
        finalVector = oriVector;
        finalVector.y = Mathf.Sin(Time.fixedTime * Mathf.PI * rate) * amplitude + oriVector.y;
        transform.localPosition = finalVector;
    }
    private void OnDisable()
    {
        transform.localPosition = revertPos;
    }

}