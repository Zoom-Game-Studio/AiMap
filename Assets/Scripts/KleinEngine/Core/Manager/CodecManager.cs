using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Security.Cryptography;

namespace KleinEngine
{
    public class CodecManager : Singleton<CodecManager>
    {
        IBufferedCipher rsaEncrypt;
        IBufferedCipher rsaDecrypt;

        ICryptoTransform encryptTransform;
        ICryptoTransform decryptTransform;

        RSACryptoServiceProvider rsaCrypto;
        byte[] aesKey;

        public byte[] getAesKey()
        {
            return aesKey;
        }

        public void initAES()
        {
            AesManaged aes = new AesManaged();
            aes.Mode = CipherMode.ECB;
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MinSize;
            aes.Padding = PaddingMode.Zeros;
            aesKey = aes.Key;
            //Debug.Log(Encoding.UTF8.GetString(aes.Key));
            //Debug.Log(BitConverter.ToString(aes.Key).Replace("-", ""));
            encryptTransform = aes.CreateEncryptor();
            decryptTransform = aes.CreateDecryptor();
        }

        public void initRSA(string key)
        {
            rsaCrypto = new RSACryptoServiceProvider();
            rsaCrypto.FromXmlString(key);
            RSAParameters rp = rsaCrypto.ExportParameters(false);
            //转换密钥
            AsymmetricKeyParameter pbk = DotNetUtilities.GetRsaPublicKey(rp);
            rsaEncrypt = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥
            rsaEncrypt.Init(true, pbk);
            rsaDecrypt = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            rsaDecrypt.Init(false, pbk);
        }

        public byte[] rsaEncode(byte[] data)
        {
            if (null != rsaEncrypt) return rsaEncrypt.DoFinal(data);
            return null;
        }

        public byte[] rsaDecode(byte[] data)
        {
            if (null != rsaDecrypt) return rsaDecrypt.DoFinal(data);
            return null;
        }

        public byte[] aesEncode(byte[] data)
        {
            if (null != encryptTransform) return encryptTransform.TransformFinalBlock(data, 0, data.Length);
            return null;
        }

        public byte[] aesDecode(byte[] data,UInt16 len)
        {
            if (null != decryptTransform)
            {
                byte[] decryptData = decryptTransform.TransformFinalBlock(data, 0, data.Length);
                //int i = decryptData.Length - 1;
                //for (; i >= 0; i--)
                //{
                //    if (decryptData[i] != '\0')
                //        break;
                //}
                //++i;
                byte[] bytes = new byte[len];
                Array.Copy(decryptData, bytes, len);
                return bytes;
            }
            //using (MemoryStream msEncrypt = new MemoryStream())
            //{
            //    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptTransform, CryptoStreamMode.Write))
            //    {
            //        csEncrypt.Write(data,0,data.Length);
            //        csEncrypt.Close();
            //    }
            //    return msEncrypt.ToArray();
            //}
            return null;
        }

    }
}
