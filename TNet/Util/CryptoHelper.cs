using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace TNet.Util
{ 
#region Crypto
/// <summary>
/// 非对称加密、解密
/// </summary>
public class Crypto
{
    /// <summary>
    /// 使用Rfc2898DeriveBytes算法分别生成Salt和Pwd的值（新增时使用）
    /// </summary>
    /// <param name="pwd">明文的密码</param>
    /// <param name="salt">密文后的salt</param>
    /// <param name="pwdhash">密文后的pwd</param>
    public static void GetPwdhashAndSalt(string pwd, out string salt, out string pwdHash)
    {
        //salt = "MJvwQJ/T8gJ5cjen0pMBmTHXugdhuTuZ5DRyZMhs1zg=";
        System.Security.Cryptography.Rfc2898DeriveBytes db;
        db = new System.Security.Cryptography.Rfc2898DeriveBytes(pwd, 32, 1000);
        salt = System.Convert.ToBase64String(db.Salt);
        pwdHash = System.Convert.ToBase64String(db.GetBytes(32));
        //如果需要添加用户，保存salt和散列侯的密码到数据存储设备
        //如果需要验证用户，从数据存储设备中读取slat，使用salt来计算出密码的散列值，并且与保存在数据存储设备中的密文密码进行比较
    }

    /// <summary>
    /// 根据参数pwd、salt使用Rfc2898DeriveBytes算法生成Pwd的值（判断合法性时使用）
    /// </summary>
    /// <param name="pwd">明文的密码</param>
    /// <param name="salt">密文的salt</param>
    /// <param name="pwdhash">密文的salt</param>
    public static string GetPwdhash(string pwd, string salt)
    {
        string pwdHash = "";
        System.Security.Cryptography.Rfc2898DeriveBytes db;
        db = new System.Security.Cryptography.Rfc2898DeriveBytes(pwd, System.Convert.FromBase64String(salt), 1000);
        return pwdHash = System.Convert.ToBase64String(db.GetBytes(32));
    }
}
#endregion

#region HashMD5
/// <summary>
/// MD5签名.
/// </summary>
public class HashMD5
{
    #region  字符串数组定义
    private static string[] strHexs = {"0", "1", "2", "3", "4", "5", "6", "7",

                                              "8", "9", "a", "b", "c", "d", "e", "f"};
    #endregion

    #region Compute Hash
    /// <summary>
    /// 计算Hash值
    /// </summary>
    /// <param name="bytValue"></param>
    /// <returns></returns>
    public static string CreateMD5Hash(byte[] bytValue)
    {
        byte[] bytHash;
        System.Security.Cryptography.MD5CryptoServiceProvider m_Hash = new System.Security.Cryptography.MD5CryptoServiceProvider();
        // Compute the Hash, returns an array of Bytes  
        bytHash = m_Hash.ComputeHash(bytValue);
        return ByteArrayToHexString(bytHash);
    }

    #endregion

    #region 类型转换
    public static string ByteToHexString(byte bytValue)
    {
        int num = (int)bytValue;
        int high = num / 16;
        int low = num % 16;
        return strHexs[high] + strHexs[low];
    }

    public static string ByteArrayToHexString(byte[] bytValue)
    {
        StringBuilder strReturn = new StringBuilder();
        for (int i = 0; i < bytValue.Length; i++)
        {
            strReturn.Append(ByteToHexString(bytValue[i]));
        }
        return strReturn.ToString();
    }
    #endregion

}
#endregion

#region SymmetricCrypto
/// <summary>
/// 3DES安全相关加密算法的工具类,3DES是可逆的加密算法，可以完成
/// </summary>
public class SymmetricCrypto
{
    /// <summary>
    /// 将指定的数据使用3DES算法和密码进行加密
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <returns>密文</returns>
    public static byte[] EncryptSymmetric(byte[] plainText, string hastCode)
    {
        byte[] cipherText = null;
        try
        {
            TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider provider2 = new MD5CryptoServiceProvider();
            provider1.Key = provider2.ComputeHash(Encoding.Default.GetBytes(hastCode));
            provider1.Mode = CipherMode.ECB;
            ICryptoTransform transform1 = provider1.CreateEncryptor();

            cipherText = transform1.TransformFinalBlock(plainText, 0, plainText.Length);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return cipherText;
    }

    /// <summary>
    /// 解密使用3DES算法加密的数据,如果解密失败将会抛出异常，成功将返回解密结果
    /// </summary>
    /// <param name="cipherText">密文</param>
    /// <returns>明文</returns>
    public static byte[] DecryptSymmetric(byte[] cipherText, string hastCode)
    {
        byte[] plainText = null;
        try
        {
            TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider provider2 = new MD5CryptoServiceProvider();
            provider1.Key = provider2.ComputeHash(Encoding.Default.GetBytes(hastCode));
            provider1.Mode = CipherMode.ECB;
            ICryptoTransform transform1 = provider1.CreateDecryptor();

            plainText = transform1.TransformFinalBlock(cipherText, 0, cipherText.Length);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return plainText;
    }
}
#endregion

#region Base64Crypto
/// <summary>
/// 字符串的编码、解码
/// </summary>
public class Base64Crypto
{
    /// <summary>
    /// 字符串编码
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static String ToBase64Encode(String content)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
    }

    /// <summary>
    /// 字符串解码
    /// </summary>
    /// <param name="Encontent"></param>
    /// <returns></returns>
    public static String ToBase64Decode(String Encontent)
    {
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Encontent));
    }
}
#endregion
}