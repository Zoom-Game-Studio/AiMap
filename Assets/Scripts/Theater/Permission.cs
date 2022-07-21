using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Permission : MonoBehaviour
{

    private string m_PermissionCamera = "android.permission.CAMERA";
    private string m_PermissionWrite = "android.permission.WRITE_EXTERNAL_STORAGE";
    private string m_PermissionRecordAudio = "android.permission.RECORD_AUDIO";
    private bool m_PermissionRequested = false;

    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        if (!AndroidRuntimePermission.HasPermission(m_PermissionCamera) ||
            !AndroidRuntimePermission.HasPermission(m_PermissionWrite) ||
            !AndroidRuntimePermission.HasPermission(m_PermissionRecordAudio))
        {
            AndroidRuntimePermission.RequestPermissions(new string[] { m_PermissionCamera, m_PermissionWrite, m_PermissionRecordAudio });
            m_PermissionRequested = true;
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
