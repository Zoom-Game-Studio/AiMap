using System;
using Architecture;
using Architecture.Command;
using Architecture.TypeEvent;
using BestHTTP;
using QFramework;
using UnityEngine;

namespace WeiXiang
{
    /// <summary>
    /// 相机截图时的姿态,收到定位信息后进行坐标系对齐
    /// </summary>
    public class CapturePoseNode : AbstractMonoController
    {
        public long ticks { get; set; }

        private void Start()
        {
            this.RegisterEvent<LocationResponseEvent>(OnRespones).UnRegisterWhenGameObjectDestroyed(gameObject);
            Invoke(nameof(Destroy),60);
        }

        void Destroy()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 定位结束的回调
        /// </summary>
        /// <param name="evt">定位事件信息</param>
        private void OnRespones(LocationResponseEvent evt)
        {
            try
            {
                if (evt.isFinish && evt.isSuccess  && evt.ticks == ticks)
                {
                    this.SendCommand(new AlignCoordinateCommand()
                    {
                        data = evt.data,
                        transform = transform,
                    });
                }
            }
            catch (Exception e)
            {
                Console.Error(e);
            }
            Destroy(gameObject);   
        }
    }
}