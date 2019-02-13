using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

namespace DMData.Code
{
    public class DataEncoder
    {
        private static char[] c_array = new char[]
        {
            'C','p','2','u','V','S','Z','8','k','B'
            ,'h','E','X','t','W','F','x','i','\\','y'
            ,'=','b','9','L','s','c','g','R','n','z'
            ,'-','Q','m','w','|','4','I',')','O','3'
            ,'D','[','A','_','1','>','N','T','r',':'
            ,'f','7','o','.','5','d','v','q','/','6'
            ,'e','Y','G','a','~','`','!','@','#','$'
            ,'%','^','&','*','H','(','+','{','K','}'
            ,'0',']','J',';','M','\'','P',',','l','<'
            ,'U','?','j',' ','"'
        };

        //DES默认密钥向量
        private static byte[] DES_IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        //AES默认密钥向量
        private static readonly byte[] AES_IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private static int i_1 = c_array.Length;
        private static int i_2 = 6;
        private static int i_3 = 36;

        public static string EncryptData(string pStr)
        {
            if (pStr == null || pStr.Trim() == "")
            {
                return "";
            }
            char[] array = pStr.ToCharArray();
            string text = "";
            string text2 = "";
            for (int i = 0; i < array.Length; i++)
            {
                int num = array[i];
                if (num < 32 || num > 126)
                {
                    string text3 = Func_5(num);
                    text += c_array[i];
                    text += text3.Length.ToString();
                    text2 += text3;
                }
                else
                {
                    text2 += (char)num;
                }
            }
            return Func_1(text2, text);
        }
        public static string DecryptData(string pStr)
        {
            if (pStr == null)
            {
                return "";
            }
            int length = pStr.Length;
            if (length < 7)
            {
                return "";
            }
            char[] array = pStr.ToCharArray();
            int num = Func_3(array[5]);
            int num2 = 0;
            array[5] = Func_2(0);
            for (int i = 0; i < length; i++)
            {
                num2 += array[i];
            }
            if (num2 < 0)
            {
                num2 += i_1;
            }
            if (num != num2 % i_1)
            {
                return "";
            }
            int num3 = Func_3(array[0]);
            int num4 = Func_3(array[1]);
            int num5 = Func_3(array[2]);
            int num6 = Func_3(array[3]);
            int num7 = Func_3(array[4]);
            if (i_2 + num4 + num5 + num6 + num7 != length)
            {
                return "";
            }
            char[] array2 = new char[num5 + num6];
            for (int i = 0; i < num5 + num6; i++)
            {
                array2[i] = Func_2(Func_3(array[i + i_2 + num4]) - num3);
            }
            if (num6 == 0)
            {
                return new string(array2);
            }
            if (num6 > num5)
            {
                return "";
            }
            if (num6 % 2 == 1)
            {
                return "";
            }
            string text;
            string text2;
            if (num4 >= num7)
            {
                text = new string(array2, 0, num6);
                text2 = new string(array2, num6, num5);
            }
            else
            {
                text = new string(array2, num5, num6);
                text2 = new string(array2, 0, num5);
            }
            char[] array3 = text.ToCharArray();
            StringBuilder sbr = new StringBuilder(text2);
            for (int i = 0; i < array3.Length; i++)
            {
                int num8 = Func_3(array3[i]);
                i++;
                int num9 = Func_6(array3[i]);
                string strTemp = sbr.ToString(num8, num9);
                int num10 = Func_4(strTemp);
                char c = (char)num10;
                sbr.Remove(num8, num9);
                sbr.Insert(num8, c);
            }
            return sbr.ToString();
        }
        public static string EncryptByte(string pStr)
        {
            byte[] b = Encoding.ASCII.GetBytes(pStr);
            return Convert.ToBase64String(b);
        }
        public static string DecryptByte(string pStr)
        {
            byte[] b = Convert.FromBase64String(pStr);
            return Encoding.UTF8.GetString(b);
        }
        public static string MyMD5(string pStr)
        {
            string strReturn = string.Empty;
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.Default.GetBytes(pStr);
            byte[] s = md5.ComputeHash(b);
            for (int i = 0; i < s.Length; i++)
                strReturn += s[i].ToString("X2");
            return strReturn;
        }

        #region DES
        public static string EncryptDES(string input, string key)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptBytes = EncryptDES(inputBytes, keyBytes, DES_IV);
            //string result = Encoding.UTF8.GetString(encryptBytes); //无法解码,其加密结果中文出现乱码：d\"�e����(��uπ�W��-��,_�\nJn7 
            //原因：如果明文为中文，UTF8编码两个字节标识一个中文字符，但是加密后，两个字节密文，不一定还是中文字符。
            using (DES des = new DESCryptoServiceProvider())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cs))
                        {
                            writer.Write(inputBytes);
                        }
                    }
                }
            }

            string result = Convert.ToBase64String(encryptBytes);

            return result;
        }
        private static byte[] EncryptDES(byte[] inputBytes, byte[] key, byte[] IV)
        {
            DES des = new DESCryptoServiceProvider();
            //建立加密对象的密钥和偏移量
            des.Key = key;
            des.IV = IV;
            string result = string.Empty;

            //1、如果通过CryptoStreamMode.Write方式进行加密，然后CryptoStreamMode.Read方式进行解密，解密成功。
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }
                return ms.ToArray();
            }
            //2、如果通过CryptoStreamMode.Write方式进行加密，然后再用CryptoStreamMode.Write方式进行解密，可以得到正确结果
            //3、如果通过CryptoStreamMode.Read方式进行加密，然后再用CryptoStreamMode.Read方式进行解密，无法解密，Error：要解密的数据的长度无效。
            //4、如果通过CryptoStreamMode.Read方式进行加密，然后再用CryptoStreamMode.Write方式进行解密,无法解密，Error：要解密的数据的长度无效。
            //using (MemoryStream ms = new MemoryStream(inputBytes))
            //{
            //    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Read))
            //    {
            //        using (StreamReader reader = new StreamReader(cs))
            //        {
            //            result = reader.ReadToEnd();
            //            return Encoding.UTF8.GetBytes(result);
            //        }
            //    }
            //}
        }
        public static string DecryptDES(string input, string key)
        {
            //UTF8无法解密，Error: 要解密的数据的长度无效。
            //byte[] inputBytes = Encoding.UTF8.GetBytes(input);//UTF8乱码，见加密算法
            byte[] inputBytes = Convert.FromBase64String(input);

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] resultBytes = DecryptDES(inputBytes, keyBytes, DES_IV);

            string result = Encoding.UTF8.GetString(resultBytes);

            return result;
        }
        private static byte[] DecryptDES(byte[] inputBytes, byte[] key, byte[] iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //建立加密对象的密钥和偏移量，此值重要，不能修改
            des.Key = key;
            des.IV = iv;

            //通过write方式解密
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            //    {
            //        cs.Write(inputBytes, 0, inputBytes.Length);
            //    }
            //    return ms.ToArray();
            //}

            //通过read方式解密
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        string result = reader.ReadToEnd();
                        return Encoding.UTF8.GetBytes(result);
                    }
                }
            }

            //错误写法,注意哪个是输出流的位置，如果范围ms，与原文不一致。
            //using (MemoryStream ms = new MemoryStream(inputBytes))
            //{
            //    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
            //    {
            //        cs.Read(inputBytes, 0, inputBytes.Length);
            //    }
            //    return ms.ToArray();
            //}
        }
        /// 加密字符串
        public static string EncryptString(string input, string sKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
        }
        /// 解密字符串
        public static string DecryptString(string input, string sKey)
        {
            string[] sInput = input.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], System.Globalization.NumberStyles.HexNumber);
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
        }
        #endregion

        #region AES
        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="input">明文字符串</param>  
        /// <param name="key">密钥</param>  
        /// <returns>字符串</returns>  
        public static string EncryptByAES(string input, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = AES_IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        byte[] bytes = msEncrypt.ToArray();
                        //return Convert.ToBase64String(bytes);//此方法不可用
                        return BitConverter.ToString(bytes);
                    }
                }
            }
        }
        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="input">密文字节数组</param>  
        /// <param name="key">密钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static string DecryptByAES(string input, string key)
        {
            //byte[] inputBytes = Convert.FromBase64String(input); //Encoding.UTF8.GetBytes(input);
            string[] sInput = input.Split("-".ToCharArray());
            byte[] inputBytes = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                inputBytes[i] = byte.Parse(sInput[i], System.Globalization.NumberStyles.HexNumber);
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = AES_IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        /// <summary> 
        /// AES加密        
        /// </summary> 
        /// <param name="inputdata">输入的数据</param>         
        /// <param name="iv">向量128位</param>         
        /// <param name="strKey">加密密钥</param>         
        /// <returns></returns> 
        public static byte[] EncryptByAES(byte[] inputdata, byte[] key, byte[] iv)
        {
            ////分组加密算法 
            //Aes aes = new AesCryptoServiceProvider();          
            ////设置密钥及密钥向量 
            //aes.Key = key;
            //aes.IV = iv;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            //    {
            //        using (StreamWriter writer = new StreamWriter(cs))
            //        {
            //            writer.Write(inputdata);
            //        }
            //        return ms.ToArray(); 
            //    }               
            //}

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(inputdata);
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        return encrypted;
                    }
                }
            }
        }
        /// <summary>         
        /// AES解密         
        /// </summary> 
        /// <param name="inputdata">输入的数据</param>                
        /// <param name="key">key</param>         
        /// <param name="iv">向量128</param> 
        /// <returns></returns> 
        public static byte[] DecryptByAES(byte[] inputBytes, byte[] key, byte[] iv)
        {
            Aes aes = new AesCryptoServiceProvider();
            aes.Key = key;
            aes.IV = iv;
            byte[] decryptBytes;
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        string result = reader.ReadToEnd();
                        decryptBytes = Encoding.UTF8.GetBytes(result);
                    }
                }
            }

            return decryptBytes;
        }
        #endregion

        #region RSA
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>密文字符串</returns>
        public static string EncryptByRSA(string plaintext, string publicKey)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(plaintext);
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(publicKey);
                byte[] encryptedData = RSA.Encrypt(dataToEncrypt, false);
                return Convert.ToBase64String(encryptedData);
            }
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>明文字符串</returns>
        public static string DecryptByRSA(string ciphertext, string privateKey)
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(privateKey);
                byte[] encryptedData = Convert.FromBase64String(ciphertext);
                byte[] decryptedData = RSA.Decrypt(encryptedData, false);
                return byteConverter.GetString(decryptedData);
            }
        }
        /// <summary>
        /// 数字签名
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名</returns>
        public static string HashAndSignString(string plaintext, string privateKey)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(plaintext);

            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(privateKey);
                //使用SHA1进行摘要算法，生成签名
                byte[] encryptedData = RSAalg.SignData(dataToEncrypt, new SHA1CryptoServiceProvider());
                return Convert.ToBase64String(encryptedData);
            }
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <param name="SignedData">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static bool VerifySigned(string plaintext, string SignedData, string publicKey)
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(publicKey);
                UnicodeEncoding ByteConverter = new UnicodeEncoding();
                byte[] dataToVerifyBytes = ByteConverter.GetBytes(plaintext);
                byte[] signedDataBytes = Convert.FromBase64String(SignedData);
                return RSAalg.VerifyData(dataToVerifyBytes, new SHA1CryptoServiceProvider(), signedDataBytes);
            }
        }
        /// <summary>
        /// 获取Key
        /// 键为公钥，值为私钥
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, string> CreateRSAKey()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string privateKey = RSA.ToXmlString(true);
            string publicKey = RSA.ToXmlString(false);

            return new KeyValuePair<string, string>(publicKey, privateKey);
        }
        #endregion

        #region other
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string input)
        {
            string[] sInput = input.Split("-".ToCharArray());
            byte[] inputBytes = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                inputBytes[i] = byte.Parse(sInput[i], System.Globalization.NumberStyles.HexNumber);
            }
            return inputBytes;
        }
        #endregion

        #region temp
        //public static string AESEncrypt(string encryptStr, string encryptKey)
        //{
        //    if (string.IsNullOrWhiteSpace(encryptStr)) return string.Empty;
        //    encryptKey = StringHelper.SubString(encryptKey, 32);
        //    encryptKey = encryptKey.PadRight(32, ' ');
        //    //分组加密算法
        //    SymmetricAlgorithm des = Rijndael.Create();
        //    byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptStr);//得到需要加密的字节数组 
        //    //设置密钥及密钥向量
        //    des.Key = Encoding.UTF8.GetBytes(encryptKey);
        //    des.IV = AES_IV;
        //    byte[] cipherBytes = null;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
        //        {
        //            cs.Write(inputByteArray, 0, inputByteArray.Length);
        //            cs.FlushFinalBlock();
        //            cipherBytes = ms.ToArray();//得到加密后的字节数组
        //            cs.Close();
        //            ms.Close();
        //        }
        //    }
        //    return Convert.ToBase64String(cipherBytes);
        //}
        //public static string AESDecrypt(string decryptStr, string decryptKey)
        //{
        //    if (string.IsNullOrWhiteSpace(decryptStr))
        //        return string.Empty;

        //    decryptKey = StringHelper.SubString(decryptKey, 32);
        //    decryptKey = decryptKey.PadRight(32, ' ');

        //    byte[] cipherText = Convert.FromBase64String(decryptStr);

        //    SymmetricAlgorithm des = Rijndael.Create();
        //    des.Key = Encoding.UTF8.GetBytes(decryptKey);
        //    des.IV = AES_IV;
        //    byte[] decryptBytes = new byte[cipherText.Length];
        //    using (MemoryStream ms = new MemoryStream(cipherText))
        //    {
        //        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
        //        {
        //            cs.Read(decryptBytes, 0, decryptBytes.Length);
        //            cs.Close();
        //            ms.Close();
        //        }
        //    }
        //    return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");//将字符串后尾的'\0'去掉
        //}
        //public string EncryptDES(string encryptString, string encryptKey)
        //{
        //    try
        //    {
        //        byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
        //        byte[] rgbIV = DES_IV;
        //        byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        //        DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
        //        MemoryStream mStream = new MemoryStream();
        //        CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        //        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        //        cStream.FlushFinalBlock();
        //        return Convert.ToBase64String(mStream.ToArray());
        //    }
        //    catch
        //    {
        //        return encryptString;
        //    }
        //}

        //public string DecryptDES(string decryptString, string decryptKey)
        //{
        //    try
        //    {
        //        byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
        //        byte[] rgbIV = DES_IV;
        //        byte[] inputByteArray = Convert.FromBase64String(decryptString);
        //        DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
        //        MemoryStream mStream = new MemoryStream();
        //        CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        //        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        //        cStream.FlushFinalBlock();
        //        return Encoding.UTF8.GetString(mStream.ToArray());
        //    }
        //    catch
        //    {
        //        return decryptString;
        //    }
        //}
        #endregion

        private static string Func_1(string pStr1, string pStr2)
        {
            Random random = new Random();
            int length = pStr1.Length;
            int length2 = pStr2.Length;
            int num = Math.Abs(random.Next()) % i_1;
            int num2 = Math.Abs(random.Next()) % 5;
            int num3 = Math.Abs(random.Next()) % 5;
            int num4 = i_2 + num2 + length + length2 + num3;
            char[] array = new char[num4];
            if (num2 >= num3)
            {
                pStr1 = pStr2 + pStr1;
            }
            else
            {
                pStr1 += pStr2;
            }
            array[0] = Func_2(num);
            array[1] = Func_2(num2);
            array[2] = Func_2(length);
            array[3] = Func_2(length2);
            array[4] = Func_2(num3);
            array[5] = Func_2(0);
            for (int i = 0; i < num2; i++)
            {
                array[i_2 + i] = Func_2(Math.Abs(random.Next()) % i_1);
            }
            char[] array2 = pStr1.ToCharArray();
            for (int i = 0; i < length + length2; i++)
            {
                int num5 = Func_3(array2[i]);
                char c = Func_2(num5 + num);
                array[i_2 + num2 + i] = c;
            }
            for (int i = 0; i < num3; i++)
            {
                array[i_2 + num2 + length + length2 + i] = Func_2(Math.Abs(random.Next()) % i_1);
            }
            int num6 = 0;
            for (int i = 0; i < num4; i++)
            {
                num6 += array[i];
            }
            array[5] = Func_2(num6);
            return new string(array);
        }
        private static char Func_2(int pInt)
        {
            while (pInt < 0)
            {
                pInt += i_1;
            }
            while (pInt >= i_1)
            {
                pInt -= i_1;
            }
            return c_array[pInt];
        }
        private static int Func_3(char pChr)
        {
            for (int i = 0; i < i_1; i++)
            {
                if (c_array[i] == pChr)
                {
                    return i;
                }
            }
            return 0;
        }
        private static int Func_4(string pStr)
        {
            int num = 0;
            char[] array = pStr.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                num += Func_6(array[i]) * (int)Math.Pow(i_3, (array.Length - 1 - i));
            }
            return num;
        }
        private static string Func_5(int pInt)
        {
            string text = "";
            int i = pInt;
            while (i >= i_3)
            {
                int num = i % i_3;
                i /= i_3;
                if (num >= 0)
                {
                    text = Func_7(num) + text;
                }
            }
            return Func_7(i) + text;
        }
        private static int Func_6(char pChr)
        {
            if (pChr > '@' && pChr < '[')
            {
                return (pChr - '7');
            }
            if (pChr > '/' && pChr < ':')
            {
                return (pChr - '0');
            }
            return -1;
        }
        private static char Func_7(int pInt)
        {
            if (pInt > 9)
            {
                return (char)(pInt + 55);
            }
            return (char)(pInt + 48);
        }
    }
    /*
    public class DataEncoder
    {
        private static char[] O0O100Ol0110lOl0 = new char[]
        {
            'C',
            'p',
            '2',
            'u',
            'V',
            'S',
            'Z',
            '8',
            'k',
            'B',
            'h',
            'E',
            'X',
            't',
            'W',
            'F',
            'x',
            'i',
            '\\',
            'y',
            '=',
            'b',
            '9',
            'L',
            's',
            'c',
            'g',
            'R',
            'n',
            'z',
            '-',
            'Q',
            'm',
            'w',
            '|',
            '4',
            'I',
            ')',
            'O',
            '3',
            'D',
            '[',
            'A',
            '_',
            '1',
            '>',
            'N',
            'T',
            'r',
            ':',
            'f',
            '7',
            'o',
            '.',
            '5',
            'd',
            'v',
            'q',
            '/',
            '6',
            'e',
            'Y',
            'G',
            'a',
            '~',
            '`',
            '!',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '*',
            'H',
            '(',
            '+',
            '{',
            'K',
            '}',
            '0',
            ']',
            'J',
            ';',
            'M',
            '\'',
            'P',
            ',',
            'l',
            '<',
            'U',
            '?',
            'j',
            ' ',
            '"'
        };
        private static int l0O00l11O0O1l00 = DataEncoder.O0O100Ol0110lOl0.Length;
        private static int OlO00lOOlll1OlO0 = 6;
        private static int O1llOO110llOO10 = 36;
        public static string EncryptData(string OOl11O0l1l011lO)
        {
            if (OOl11O0l1l011lO == null || OOl11O0l1l011lO.Trim() == "")
            {
                return "";
            }
            char[] array = OOl11O0l1l011lO.ToCharArray();
            string text = "";
            string text2 = "";
            for (int i = 0; i < array.Length; i++)
            {
                int num = (int)array[i];
                if (num < 32 || num > 126)
                {
                    string text3 = DataEncoder.O00O11OOO0Ol1001(num);
                    text += DataEncoder.O0O100Ol0110lOl0[i];
                    text += text3.get_Length().ToString();
                    text2 += text3;
                }
                else
                {
                    text2 += (char)num;
                }
            }
            return DataEncoder.O00lO01O1l01Ol1O(text2, text);
        }
        private static string O00lO01O1l01Ol1O(string O10l10O1llOOOOO0, string OOlOO011O1111l1l)
        {
            Random random = new Random();
            int length = O10l10O1llOOOOO0.get_Length();
            int length2 = OOlOO011O1111l1l.get_Length();
            int num = Math.Abs(random.Next()) % DataEncoder.l0O00l11O0O1l00;
            int num2 = Math.Abs(random.Next()) % 5;
            int num3 = Math.Abs(random.Next()) % 5;
            int num4 = DataEncoder.OlO00lOOlll1OlO0 + num2 + length + length2 + num3;
            char[] array = new char[num4];
            if (num2 >= num3)
            {
                O10l10O1llOOOOO0 = OOlOO011O1111l1l + O10l10O1llOOOOO0;
            }
            else
            {
                O10l10O1llOOOOO0 += OOlOO011O1111l1l;
            }
            array[0] = DataEncoder.Ol000l11l1lOllOO(num);
            array[1] = DataEncoder.Ol000l11l1lOllOO(num2);
            array[2] = DataEncoder.Ol000l11l1lOllOO(length);
            array[3] = DataEncoder.Ol000l11l1lOllOO(length2);
            array[4] = DataEncoder.Ol000l11l1lOllOO(num3);
            array[5] = DataEncoder.Ol000l11l1lOllOO(0);
            for (int i = 0; i < num2; i++)
            {
                array[DataEncoder.OlO00lOOlll1OlO0 + i] = DataEncoder.Ol000l11l1lOllOO(Math.Abs(random.Next()) % DataEncoder.l0O00l11O0O1l00);
            }
            char[] array2 = O10l10O1llOOOOO0.ToCharArray();
            for (int i = 0; i < length + length2; i++)
            {
                int num5 = DataEncoder.OO10O100110010O1(array2[i]);
                char c = DataEncoder.Ol000l11l1lOllOO(num5 + num);
                array[DataEncoder.OlO00lOOlll1OlO0 + num2 + i] = c;
            }
            for (int i = 0; i < num3; i++)
            {
                array[DataEncoder.OlO00lOOlll1OlO0 + num2 + length + length2 + i] = DataEncoder.Ol000l11l1lOllOO(Math.Abs(random.Next()) % DataEncoder.l0O00l11O0O1l00);
            }
            int num6 = 0;
            for (int i = 0; i < num4; i++)
            {
                num6 += (int)array[i];
            }
            array[5] = DataEncoder.Ol000l11l1lOllOO(num6);
            return new string(array);
        }
        public static string DecryptData(string O00l00lOl1100O0O)
        {
            if (O00l00lOl1100O0O == null)
            {
                return "";
            }
            int length = O00l00lOl1100O0O.get_Length();
            if (length < 7)
            {
                return "";
            }
            char[] array = O00l00lOl1100O0O.ToCharArray();
            int num = DataEncoder.OO10O100110010O1(array[5]);
            int num2 = 0;
            array[5] = DataEncoder.Ol000l11l1lOllOO(0);
            for (int i = 0; i < length; i++)
            {
                num2 += (int)array[i];
            }
            if (num2 < 0)
            {
                num2 += DataEncoder.l0O00l11O0O1l00;
            }
            if (num != num2 % DataEncoder.l0O00l11O0O1l00)
            {
                return "";
            }
            int num3 = DataEncoder.OO10O100110010O1(array[0]);
            int num4 = DataEncoder.OO10O100110010O1(array[1]);
            int num5 = DataEncoder.OO10O100110010O1(array[2]);
            int num6 = DataEncoder.OO10O100110010O1(array[3]);
            int num7 = DataEncoder.OO10O100110010O1(array[4]);
            if (DataEncoder.OlO00lOOlll1OlO0 + num4 + num5 + num6 + num7 != length)
            {
                return "";
            }
            char[] array2 = new char[num5 + num6];
            for (int i = 0; i < num5 + num6; i++)
            {
                array2[i] = DataEncoder.Ol000l11l1lOllOO(DataEncoder.OO10O100110010O1(array[i + DataEncoder.OlO00lOOlll1OlO0 + num4]) - num3);
            }
            if (num6 == 0)
            {
                return new string(array2);
            }
            if (num6 > num5)
            {
                return "";
            }
            if (num6 % 2 == 1)
            {
                return "";
            }
            string text;
            string text2;
            if (num4 >= num7)
            {
                text = new string(array2, 0, num6);
                text2 = new string(array2, num6, num5);
            }
            else
            {
                text = new string(array2, num5, num6);
                text2 = new string(array2, 0, num5);
            }
            char[] array3 = text.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder(text2);
            for (int i = 0; i < array3.Length; i++)
            {
                int num8 = DataEncoder.OO10O100110010O1(array3[i]);
                i++;
                int num9 = DataEncoder.O00l0OO1l1110lO(array3[i]);
                string o011ll01001OO1ll = stringBuilder.ToString(num8, num9);
                int num10 = DataEncoder.OOl00ll10lO0l10O(o011ll01001OO1ll);
                char c = (char)num10;
                stringBuilder.Remove(num8, num9);
                stringBuilder.Insert(num8, c);
            }
            return stringBuilder.ToString();
        }
        private static char Ol000l11l1lOllOO(int I1ll001O1Ol1l011)
        {
            while (I1ll001O1Ol1l011 < 0)
            {
                I1ll001O1Ol1l011 += DataEncoder.l0O00l11O0O1l00;
            }
            while (I1ll001O1Ol1l011 >= DataEncoder.l0O00l11O0O1l00)
            {
                I1ll001O1Ol1l011 -= DataEncoder.l0O00l11O0O1l00;
            }
            return DataEncoder.O0O100Ol0110lOl0[I1ll001O1Ol1l011];
        }
        private static int OO10O100110010O1(char l1O0O101111l1l0)
        {
            for (int i = 0; i < DataEncoder.l0O00l11O0O1l00; i++)
            {
                if (DataEncoder.O0O100Ol0110lOl0[i] == l1O0O101111l1l0)
                {
                    return i;
                }
            }
            return 0;
        }
        private static int OOl00ll10lO0l10O(string O011ll01001OO1ll)
        {
            int num = 0;
            char[] array = O011ll01001OO1ll.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                num += DataEncoder.O00l0OO1l1110lO(array[i]) * (int)Math.Pow((double)DataEncoder.O1llOO110llOO10, (double)(array.Length - 1 - i));
            }
            return num;
        }
        private static string O00O11OOO0Ol1001(int I1ll1O00l0O0OO10)
        {
            string text = "";
            int i = I1ll1O00l0O0OO10;
            while (i >= DataEncoder.O1llOO110llOO10)
            {
                int num = i % DataEncoder.O1llOO110llOO10;
                i /= DataEncoder.O1llOO110llOO10;
                if (num >= 0)
                {
                    text = DataEncoder.lOOO01O0Ol110Ol(num) + text;
                }
            }
            return DataEncoder.lOOO01O0Ol110Ol(i) + text;
        }
        private static int O00l0OO1l1110lO(char l1O0O101111l1l0)
        {
            if (l1O0O101111l1l0 > '@' && l1O0O101111l1l0 < '[')
            {
                return (int)(l1O0O101111l1l0 - '7');
            }
            if (l1O0O101111l1l0 > '/' && l1O0O101111l1l0 < ':')
            {
                return (int)(l1O0O101111l1l0 - '0');
            }
            return -1;
        }
        private static char lOOO01O0Ol110Ol(int O10OO1Ol0O0l11O1)
        {
            if (O10OO1Ol0O0l11O1 > 9)
            {
                return (char)(O10OO1Ol0O0l11O1 + 55);
            }
            return (char)(O10OO1Ol0O0l11O1 + 48);
        }
    }
    */
}