using System;
using Architecture;
using Architecture.Command;
using QFramework;
using TMPro;
using UniRx;
using UnityEngine;
using WeiXiang;

public class CameraCorrect : AbstractMonoController
{
    
    public float step = 0.1f;

    [SerializeField] private TextMeshProUGUI _textMeshProX,_textMeshProY,_textMeshProZ;
    
    public enum Axis
    {
        x,y,z
    }

    private void Start()
    {
        var data = this.GetModel<ICameraOffset>();
        data.X.Subscribe(v => _textMeshProX.text = v.ToString("F3")).AddTo(this);
        data.Y.Subscribe(v => _textMeshProY.text = v.ToString("F3")).AddTo(this);
        data.Z.Subscribe(v => _textMeshProZ.text = v.ToString("F3")).AddTo(this);
    }

    public void AddX()
    {
        this.SendCommand(new ChangeCameraOffsetCommand(Axis.x,step));
    }

    public void SubX()
    {
        this.SendCommand(new ChangeCameraOffsetCommand(Axis.x,-step));
    }
    
    public void AddY()
    {
        this.SendCommand(new ChangeCameraOffsetCommand(Axis.y,step));
    }

    public void SubY()
    {
        this.SendCommand(new ChangeCameraOffsetCommand(Axis.y,-step));
    }
    
    public void AddZ()
    {
        this.SendCommand(new ChangeCameraOffsetCommand(Axis.z,step));
    }

    public void SubZ()
    {
        this.SendCommand(new ChangeCameraOffsetCommand(Axis.z,-step));
    }
}
