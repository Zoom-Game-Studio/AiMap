using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace KleinEngine
{
    public class NetworkManager : EventDispatcher
    {
        private string netIP;
        private int netPort;
        private SocketClient client;
        private Queue<NetMessage> receiveQueue = new Queue<NetMessage>();
        //private Queue<NetPacket> sendQueue = new Queue<NetPacket>();
        private Queue<KeyValuePair<string, object>> eventQueue = new Queue<KeyValuePair<string, object>>();
        //private Dictionary<uint, Action<IMessage>> handleList = new Dictionary<uint, Action<IMessage>>();
        //private Dictionary<uint, MessageParser> msgParserDic = new Dictionary<uint, MessageParser>();

        public NetworkManager()
        {
            client = new SocketClient(this);
        }

        public bool isConnect
        {
            get { return client.isConnected; }
        }

        //网络完整包数据放入队列
        public void pushMessage(uint msgId, byte[] data)
        {
            //lock(receivelock)
            //{
            //    NetMessage netMsg = new NetMessage();
            //    netMsg.msgId = msgId;
            //    if (msgParserDic.ContainsKey(msgId))
            //    {
            //        MessageParser parser = msgParserDic[msgId];
            //        if (null != parser) netMsg.msg = parser.ParseFrom(data);
            //    }
            //    receiveQueue.Enqueue(netMsg);
            //}            
        }
        private readonly object eventlock = new object();
        //网络异步回调事件放入队列,避免在非主线程中执行
        public void pushEvent(string key, object param = null)
        {
            lock(eventlock)
            {
                eventQueue.Enqueue(new KeyValuePair<string, object>(key, param));
            }            
        }

		// 添加每个协议对应的处理方法
		//public bool addHandle(uint msgId, Action<IMessage> handle, MessageParser parser)
		//{
		//	if (!handleList.ContainsKey(msgId))
		//	{
		//		handleList[msgId] = handle;
  //              if (null != parser) msgParserDic[msgId] = parser;
  //              return true;
		//	}
		//	return false;
		//}

		public void removeHandle(uint msgId)
		{
			//if (handleList.ContainsKey(msgId))
			//{
			//	handleList.Remove(msgId);
			//}
   //         if (msgParserDic.ContainsKey(msgId))
   //         {
   //             msgParserDic.Remove(msgId);
   //         }
        }

        public void setIPAndPort(string ip,int port)
        {
            netIP = ip;
            netPort = port;
        }

		// 开启网络连接
		public void startConnect()
		{
			client.ConnectServer(netIP, netPort);
		}

        public void update()
        {
            handleEvent();
            //handleSend();
            handleReceive();
        }

        // 处理队列中的网络事件
        void handleEvent()
        {
            lock (eventlock)
            {
                while (eventQueue.Count > 0)
                {
                    KeyValuePair<string, object> netEvent = eventQueue.Dequeue();
                    dispatchEvent(netEvent.Key, netEvent.Value);
                }
            }            
        }

        // 发送消息
        public void handleSend()
        {
            //while (sendQueue.Count > 0)
            //{
            //    NetPacket packet = sendQueue.Dequeue();
            //    client.WriteMessage(packet.getData());
            //    //++sendCount;
            //    //if (sendCount >= 100) break;
            //}
        }

        private readonly object receivelock = new object();
        // 处理接收回来的消息
        public void handleReceive()
        {
            lock(receivelock)
            {
                while (receiveQueue.Count > 0)
                {
                    NetMessage netMsg = receiveQueue.Dequeue();
                    //dispatchEvent(NET_EVENT.HANDLE_START, msg.Key);
                    //handleMessage(netMsg.msgId, netMsg.msg);
                }
            }            
        }

        // 处理协议数据
        //private void handleMessage(uint key, IMessage msg)
        //{
        //    if (handleList.ContainsKey(key))
        //    {
        //        Action<IMessage> handle = handleList[key];
        //        if (handle != null) handle(msg);
        //    }
        //    else
        //    {
        //        dispatchEvent(NET_EVENT.HANDLE_NONE, key);
        //    }
        //}

        public void handleConnectFail(SocketError error)
        {
            dispatchEvent(NET_EVENT.CONNECT_FAIL, error);
        }

		//放入发送队列
        //public void sendMessage(uint msgId, IMessage message = null, bool isUdp = false)
        //{
        //    int packetLen = 0;
        //    byte[] msgData = null;
        //    if (null != message)
        //    {
        //        msgData = CodecManager.GetInstance().aesEncode(message.ToByteArray());
        //        //msgData = message.ToByteArray();
        //        packetLen = msgData.Length;
        //    }
        //    //优化为一个packet
        //    NetPacket packet = new NetPacket(msgId,packetLen);
        //    packet.writeBytes(msgData);
        //    client.WriteMessage(packet.getData());
        //    //sendQueue.Enqueue(packet);
        //}

        public void closeConnect()
        {
            client.close();
            lock (eventlock)
            {
                eventQueue.Clear();
            }
            dispatchEvent(NET_EVENT.CONNECT_CLOSE);
        }

        public override void onClear()
        {
            //closeConnect();
            ////sendQueue.Clear();
            //lock (receivelock)
            //{
            //    receiveQueue.Clear();
            //}
            //lock (eventlock)
            //{
            //    eventQueue.Clear();
            //}
            //handleList.Clear();
            //base.onClear();
        }

        public void onDestroy()
        {
            client.close();
        }

    }

    public class NetMessage
    {
        public uint msgId = 0;
        //public IMessage msg = null;
    }

    public struct NET_EVENT
    {
        public const string CONNECT_START = "connect_start";
        public const string CONNECT_SUCC = "connect_succ";
        public const string CONNECT_FAIL = "connect_fail";
        public const string CONNECT_CLOSE = "connect_close";

        // 协议开始发送
        public const string SEND_START = "send_start";


        // 协议没有对应处理方法：参数为协议号
        public const string HANDLE_NONE = "handle_none";
        public const string HANDLE_START = "handle_start";
        public const string HANDLE_END = "handle_end";

    }
}