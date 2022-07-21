using System;
using System.IO;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace KleinEngine
{
    public class SocketClient
    {
        private Socket client;        

        private MemoryStream memStream;
        private BinaryReader reader;

        private const int MAX_READ = 8192;
        private byte[] byteBuffer;
        public const int PROTOCOL_HEAD_LENGTH = 8;

        bool handShake = false;
        bool connectStop = true;
        NetworkManager networkMgr;

        public SocketClient(NetworkManager manager)
        {
            networkMgr = manager;
            byteBuffer = new byte[MAX_READ];
            memStream = new MemoryStream();
            reader = new BinaryReader(memStream);
        }

        // 连接服务器
        public void ConnectServer(string host, int port)
        {
            handShake = false;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.NoDelay = true;//关闭Nagle算法 
            try
            {
                SocketAsyncEventArgs evnetArgs = new SocketAsyncEventArgs();
                evnetArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOHandle);
                try
                {
                    evnetArgs.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
                }
                catch
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(host);
                    evnetArgs.RemoteEndPoint = new IPEndPoint(hostEntry.AddressList[0], port);
                }
                bool willRaiseEvent = client.ConnectAsync(evnetArgs);
                if (!willRaiseEvent)
                {
                    OnConnect(evnetArgs);
                }
            }
            catch (SocketException ex)
            {
                Debug.Log(ex.SocketErrorCode);
                networkMgr.handleConnectFail(ex.SocketErrorCode);
            }
        }

        // 写数据
        public void WriteMessage(byte[] message)
        {
            if (client != null && client.Connected)
            {
                SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
                sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOHandle);
                sendArgs.SetBuffer(message, 0, message.Length);
                bool willRaiseEvent = client.SendAsync(sendArgs);
                if (!willRaiseEvent)
                {
                    OnWrite(sendArgs);
                }
            }
            else
            {
                networkMgr.handleConnectFail(SocketError.NotConnected);
            }
        }

        void OnConnect(SocketAsyncEventArgs e)
        {
            try
            {
                handShake = false;
                //networkMgr.pushEvent(NET_EVENT.CONNECT_SUCC);
                connectStop = false;
                e.SetBuffer(byteBuffer, 0, byteBuffer.Length);
                bool willRaiseEvent = client.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    OnRead(e);
                }
            }
            catch (SocketException ex)
            {
                networkMgr.pushEvent(NET_EVENT.CONNECT_FAIL, ex.SocketErrorCode);
            }
        }

        // 连接服务器回调   
        void OnIOHandle(object sender, SocketAsyncEventArgs e)
        {
            if (null == client) return;
            if (e.SocketError != SocketError.Success)
            {
                networkMgr.pushEvent(NET_EVENT.CONNECT_FAIL, e.SocketError);
                return;
            }
            //判断消息的接收状态
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    {
                        OnConnect(e);
                    }
                    break;
                case SocketAsyncOperation.Receive:
                    {
                        //Debug.Log("SocketAsyncOperation.Receive:");
                        OnRead(e);
                    }
                    break;
                case SocketAsyncOperation.Send:
                    {
                        OnWrite(e);
                    }
                    break;
                default:                    
                    Debug.Log(e.LastOperation);
                    break;
            }
        }
        // 读取消息
        void OnRead(SocketAsyncEventArgs e)
        {            
            try
            {
                int bytesRead = e.BytesTransferred;
                //Debug.Log("Receive:" + bytesRead);
                if (bytesRead > 0)
                {
                    OnReceive(bytesRead);
                    e.AcceptSocket = null;
                    bool willRaiseEvent = client.ReceiveAsync(e);
                    if (!willRaiseEvent)
                    {
                        OnRead(e);
                    }
                }
                else
                {
                    networkMgr.pushEvent(NET_EVENT.CONNECT_FAIL, SocketError.Shutdown);
                    connectStop = true;
                }
            }
            catch (SocketException ex)
            {
                networkMgr.pushEvent(NET_EVENT.CONNECT_FAIL, ex.SocketErrorCode);
                connectStop = true;
            }
        }

        // 链接写入数据流回调
        void OnWrite(SocketAsyncEventArgs e)
        {
            //Debug.Log("send length:" + e.Buffer.Length);
        }

        // 接收到消息,解析协议
        void OnReceive(int length)
        {
            memStream.Seek(0, SeekOrigin.End);
            memStream.Write(byteBuffer, 0, length);
            memStream.Seek(0, SeekOrigin.Begin);
            bool isFullMsg = false;
            while (remainingByteLen() >= PROTOCOL_HEAD_LENGTH)
            {
                //int messageLen = reader.ReadInt32();
                int messageLen = reader.ReadUInt16();
                UInt16 dataLen = reader.ReadUInt16();
                //Debug.Log(messageLen);
                uint msgId = reader.ReadUInt32();
                if (remainingByteLen() >= messageLen)
                {
                    isFullMsg = true;
                    byte[] data = reader.ReadBytes(messageLen);
                    //Debug.Log(BitConverter.ToString(data, 0));
                    try
                    {
                        if(!handShake)
                        {
                            byte[] handShakeData = CodecManager.GetInstance().rsaDecode(data);
                            string str = System.Text.Encoding.UTF8.GetString(handShakeData);
                            //Debug.Log(str);
                            switch (str)
                            {
                                case "ok":
                                    handShake = true;
                                    networkMgr.pushEvent(NET_EVENT.CONNECT_SUCC);
                                    break;
                                //case "no":
                                //    Debug.LogError("握手失败");
                                //    close();
                                //    break;
                                default:
                                    byte[] aesKey = CodecManager.GetInstance().getAesKey();
                                    byte[] toData = new byte[handShakeData.Length + aesKey.Length];
                                    Array.Copy(handShakeData, 0, toData, 0, handShakeData.Length);
                                    Array.Copy(aesKey, 0, toData, handShakeData.Length, aesKey.Length);
                                    byte[] handshake_data = CodecManager.GetInstance().rsaEncode(toData);
                                    NetPacket packet = new NetPacket(msgId, handshake_data.Length);
                                    packet.writeBytes(handshake_data);
                                    WriteMessage(packet.getData());
                                    break;
                            }
                        }
                        else
                        {
                            byte[] decodeData = CodecManager.GetInstance().aesDecode(data,dataLen);
                            //Debug.Log("msgId:" + msgId + " srcLen:" + dataLen + " toLen:" + decodeData.Length);
                            networkMgr.pushMessage(msgId, decodeData);
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.LogWarning("msgId:" + msgId + "::" + e.ToString());
                    }
                }
                else
                {
                    memStream.Position = memStream.Position - PROTOCOL_HEAD_LENGTH;
                    break;
                }
            }
            if (isFullMsg)
            {
                int len = remainingByteLen();
                if (len > 0)
                {
                    //reader和memStream是关联的，所以必须先读取，再重置memStream
                    byte[] leftover = reader.ReadBytes(len);
                    memStream.SetLength(0);
                    memStream.Write(leftover, 0, leftover.Length);
                }
                else memStream.SetLength(0);
            }
        }

        // 剩余的字节长度
        private int remainingByteLen()
        {
            return (int)(memStream.Length - memStream.Position);
        }

        public bool isConnected
        {
            get
            {
                return !connectStop;
            }
        }

        // 关闭连接
        public void close()
        {
            connectStop = true;
            memStream.SetLength(0);
            if (null == client) return;

            try
            {
                if (client.Connected)
                {
                    client.Shutdown(SocketShutdown.Both);
                }
            }
            catch
            {
				//NetworkManager.GetInstance().handleConnectFail(ex.SocketErrorCode);
            }
#if UNITY_WSA && !UNITY_EDITOR
            client.Dispose();
#else
            client.Close();
#endif
            client = null;
        }
    }
}

//#if !UNITY_WSA && !UNITY_EDITOR
//    using Windows.Networking;
//    using Windows.Networking.Sockets;
//    using Windows.Storage.Streams;

//    client = new StreamSocket();
//    client.Control.NoDelay = true;
//    var hostName = new Windows.Networking.HostName(host);
//    await client.ConnectAsync(hostName, port.ToString());
//    networkMgr.pushEvent(NET_EVENT.CONNECT_SUCC);
//    connectStop = false;
//    DataReader _reader = new DataReader(client.InputStream);
//    //_reader.UnicodeEncoding = UnicodeEncoding.Utf8; //注意
//    _reader.InputStreamOptions = InputStreamOptions.Partial;
//    uint count = await _reader.LoadAsync(8192);
//    _reader.ReadBytes(byteBuffer);
//    OnReceive((int) count);
//#endif

//判断是否ip地址
//public bool IsValidIP(string ip)
//{
//    if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
//    {
//        string[] ips = ip.Split('.');
//        if (ips.Length == 4 || ips.Length == 6)
//        {
//            if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
//                return true;
//            else
//                return false;
//        }
//        else
//            return false;

//    }
//    else
//        return false;
//}