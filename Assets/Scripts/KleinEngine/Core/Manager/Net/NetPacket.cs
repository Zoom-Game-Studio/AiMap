using System;

namespace KleinEngine
{
    public class NetPacket
    {
        public uint msgId = 0;
        int index = 0;
        byte[] msgData = null;

        public NetPacket(uint id, int dataLen)
        {
            msgId = id;
            msgData = new byte[SocketClient.PROTOCOL_HEAD_LENGTH + dataLen];
            writeInt(dataLen);
            writeUInt(msgId);
        }

        public NetPacket(uint id, byte[] data)
        {
            if (null != data)
            {
                msgId = id;
                msgData = data;
                index = data.Length;
            }
        }

        public void clear()
        {
            msgId = 0;
            index = 0;
            if (null != msgData) Array.Clear(msgData, 0, msgData.Length);
        }

        public void writeInt(int v)
        {
            //在大端字节序和小端字节序不同的计算机设备上，Buffer.BlockCopy()拷贝的数据是不同的
            //Buffer.BlockCopy(b, 0, data, index, b.Length);            
            //BitConverter.IsLittleEndian
            byte[] b = BitConverter.GetBytes(v);
            Array.Copy(b, 0, msgData, index, b.Length);
            index += sizeof(int);
        }

        public void writeUInt(uint v)
        {
            byte[] b = BitConverter.GetBytes(v);
            Array.Copy(b, 0, msgData, index, b.Length);
            index += sizeof(uint);
            //writer.Write(IPAddress.HostToNetworkOrder(v));
        }

        public void writeBytes(byte[] bytes)
        {
            if (null == bytes) return;
            Array.Copy(bytes, 0, msgData, index, bytes.Length);
            index += bytes.Length;
        }

        public byte[] getData()
        {
            return msgData;
        }
    }
}
