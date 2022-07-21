using System.Collections.Generic;
using System;
using UnityEngine;

namespace KleinEngine
{
    public class TickManager : Singleton<TickManager>
    {
        private List<Tick> tickList = new List<Tick>();

        /// <summary>
        /// 便于做对象池
        /// delay:秒(不能为0)
        /// </summary>
        /// <param name="delay">秒(不能为0)</param>
        /// <param name="handle"></param>
        /// <param name="renderHandle">渲染帧执行</param>
        /// <returns></returns>
        public Tick createTick(float delay, Action handle,Action renderHandle = null)
        {
            Tick tick = new Tick(delay);
            tick.setTickHandle(handle);
            tick.setRenderHandle(renderHandle);
            addTick(tick);
            return tick;
        }

        public void addTick(Tick tick)
        {
            if (tickList.Contains(tick))
                return;
            tickList.Add(tick);
        }

        public void update(float timeElapsed)
        {
            int len = tickList.Count;
            Tick tick;
            for (int i = len - 1; i >= 0; i--)
            {
                tick = tickList[i];
                if (!tick.isEnd())
                {
                    tick.onTick(timeElapsed);
                }
                else
                {
                    tickList.RemoveAt(i);
                }
            }
        }

        public void disposeAllTick()
        {
            int len = tickList.Count;
            for (int i = 0; i < len; i++)
            {
                tickList[i].onDispose();
            }
        }

        public override string ToString()
        {
            int len = tickList.Count;
            int activeCount = 0;
            for (int i = 0; i < len; i++)
            {
                if (!tickList[i].isStop()) activeCount++;
            }
            return len.ToString() + "::" + activeCount.ToString();
        }
    }
}
