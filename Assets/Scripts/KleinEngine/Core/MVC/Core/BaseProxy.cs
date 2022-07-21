//using Google.Protobuf;
using System;
using System.Collections.Generic;

namespace KleinEngine
{
    public class BaseProxy : BaseMVC,IProxy
    {
        List<uint> msgList = new List<uint>();

        public virtual void onRegister() { }
        void IProxy.onRemove()
        {
            int len = msgList.Count;
            for(int i = 0; i < len; ++i)
            {
                removeHandle(msgList[i]);
            }
            msgList.Clear();
        }

        //protected void addHandle(uint msgId, Action<IMessage> handle, MessageParser parser)
        //{
        //    //bool flag = Singleton<NetworkManager>.GetInstance().addHandle(msgId, handle, parser);
        //    //if (flag) msgList.Add(msgId);
        //}

        protected void removeHandle(uint msgId)
        {
            msgList.Remove(msgId);
            Singleton<NetworkManager>.GetInstance().removeHandle(msgId);
        }

        //protected void sendMessage(uint msgId, IMessage message = null, bool isUdp = false)
        //{
        //    //Singleton<NetworkManager>.GetInstance().sendMessage(msgId, message, isUdp);
        //}

    }
}
