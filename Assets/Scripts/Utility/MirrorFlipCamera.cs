using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Camera))]
public class MirrorFlipCamera : MonoBehaviour
{
    new Camera camera;
	public bool  flipHorizontal;
	public bool flipVertical;
	void Awake()
    {
        camera = GetComponent<Camera>();
    }


    void OnPreCull()
    {
        //print("OnPreCull...");

        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(flipHorizontal ? -1 : 1, flipVertical ? -1 : 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender()
    {
        //print("OnPreRender...");
        GL.invertCulling = flipHorizontal;
    }

    void OnPostRender()
    {
        //print("OnPostRender...");
        GL.invertCulling = false;
    }
}

