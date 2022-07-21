/***
 * 名称:状态机
 * 功能:管理状态，状态进入，退出，切换;处理状态消息
 *      
 * 作者:
 * 备注:
 * 
 ***/

using System.Collections.Generic;

namespace KleinEngine
{
    public class StateMachine
    {
        private State currentState;
        private Dictionary<string, State> stateList = new Dictionary<string, State>();

        public StateMachine()
        {

        }

        public void addState(State state)
        {
            string name = state.getName();
            if (stateList.ContainsKey(name))
            {
//                Debug.Log("已经存在状态：" + name);
                return;
            }
            state.addEvent(State.STATE_SWITCH, handleSwitchState);
            stateList.Add(name, state);
        }

        void handleSwitchState(EventObject ev)
        {
            string name = ev.param.ToString();
            changeState(name);
        }

        public void removeState(string name)
        {
            if (!stateList.ContainsKey(name))
            {
//                Debug.Log("正在移除不存在的状态：" + name);
                return;
            }
            State st = stateList[name];
            st.removeEvent(State.STATE_SWITCH, handleSwitchState);
            if (st == currentState)
            {
                st.exit();
                currentState = null;
            }
            stateList.Remove(name);
        }

        public void changeState(string stateName)
        {
            if (null != currentState)
            {
                if (currentState.getName() == stateName) return;
                currentState.exit();
                currentState = null;
            }
            State state;
            if (stateList.TryGetValue(stateName, out state))
            {
                currentState = state;
                currentState.enter();
            }
            else
            {
//                Debug.Log("没有状态：" + stateName);
            }
        }

        public void update()
        {
            if (null != currentState)
                currentState.update();
        }

        public void handleMessage(StateMessage msg)
        {
            if (null != currentState)
                currentState.handleMessage(msg);
        }

    }

    public class State : EventDispatcher
    {
        protected string name;
        public const string STATE_SWITCH = "state_switch";
        //	public const string STATE_END = "state_end";

        public string getName()
        {
            return name;
        }

        public virtual void switchState(string stateName)
        {
            dispatchEvent(STATE_SWITCH, stateName);
        }

        public virtual void enter()
        {

        }

        public virtual void update()
        {

        }

        public virtual void handleMessage(StateMessage msg)
        {

        }

        public virtual void exit()
        {

        }

    }

    //可以做成工厂类，做对象池
    //public class StateMessage : ObjectPool<StateMessage>
    public class StateMessage
    {
        public string type;
        public object data;
    }

}
