using System;

namespace KleinEngine
{
    public class Tick
    {
        Action tickHandle;
        Action renderHandle;

        private float m_Timer = 0;
        private bool m_StopFlag = true;
        private bool m_EndFlag = false;
        private float m_Delay;

        public Tick(float delay)
        {
            m_Delay = delay;
        }

        public float delay
        {
            set { m_Delay = value; }
            get { return m_Delay; }
        }

        public void setTickHandle(Action handle)
        {
            tickHandle = handle;
        }

        public void setRenderHandle(Action handle)
        {
            renderHandle = handle;
        }

        public void onTick(float timeElapsed)
        {
            if (isEnd()) return;
            if (isStop()) return;
            if (null != tickHandle)
            {
                if (m_Delay > 0)
                {
                    m_Timer += timeElapsed;
                    while (m_Timer >= m_Delay)
                    {
                        tickHandle();
                        m_Timer -= m_Delay;
                        if (isEnd()) break;
                        if (isStop()) break;
                    }
                }
            }
            //这里把计算和渲染分开，用于帧数降低时候，避免无谓的渲染消耗
            if (null != renderHandle) renderHandle();
        }

        public void start()
        {
            m_StopFlag = false;
        }

        public void reStart()
        {
            m_StopFlag = false;
            m_Timer = 0;
            //		m_AccumulativeTime = 0;
            //先执行一次
            //		update();
        }

        public void stop()
        {
            m_StopFlag = true;
        }

        public bool isStop()
        {
            return m_StopFlag;
        }

        public bool isEnd()
        {
            return m_EndFlag;
        }

        public void onDispose()
        {
            stop();
            tickHandle = null;
            renderHandle = null;
            m_EndFlag = true;
        }
    }
}
