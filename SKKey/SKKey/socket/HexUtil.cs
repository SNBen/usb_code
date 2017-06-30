using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKKey.utils
{
    class HexUtil
    {
        /// <summary> 
        /// 字符串转16进制字节数组 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        public static byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string bytesToHexString(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /**
         * 截取部分字节
         */
        public static string subHexStr(string hexStr, int start, int length)
        {
            if (hexStr != null && !"".Equals(hexStr))
            {
                return hexStr.Substring(2 * (start - 1), length * 2);
            }
            else
            {
                return hexStr;
            }
        }

        /**
	     * hex字符串转10进制字符串
	     * @param hex
	     * @return
	     */
        public static String hex2Integerstr(String hex)
        {
            return Convert.ToInt32(hex, 16) + "";
        }

        /**
         * 字节数组转ascii字节码
         * @param buf
         * @param i
         * @param j
         * @return
         * @throws UnsupportedEncodingException 
         */
        public static string bytes2Asc(byte[] buf, int start, int length)
        {
            return System.Text.Encoding.ASCII.GetString(buf, start - 1, length).Trim('\0');
        }

        /**
	     * 转换字节数组为字符串
	     * @param buf
	     * @param i
	     * @param j
	     * @return
	     * @throws UnsupportedEncodingException 
	     */
	    public static String subBytes2Str(byte[] buf, int start, int length, String charset)
        {
            if (null == charset || "gbk".Equals(charset))
            {
                return System.Text.Encoding.Default.GetString(buf, start - 1, length);
            }
            else
            {
                return System.Text.Encoding.UTF8.GetString(buf, start - 1, length);
            }

	    }

        /**
	     * integer转换为16进制字符串
	     * @param parseInt
	     * @return
	     */
        public static String int2Hex4BytesStr(int portNum)
        {
            string result = Convert.ToString(portNum, 16);
            int length = result.Length;
            if (length < 8)
            {
                string add0Sb = "";
                for (int i = 0; i < 8 - length; i++)
                {
                    add0Sb += "0";
                }
                result = add0Sb + result;
            }
            return result.ToUpper();
        }

        /**
	     * 字符串转ascii，再转16进制串
	     * @param str
	     * @return
	     * @throws UnsupportedEncodingException
	     */
	    public static String getHexStrByString(String str, String charset, int length)
        {
		    if (null != str && !"".Equals(str)) {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(str);
			    byte[] buf = new byte[length];
			    if (asciiBytes.Length > length) {
                    Array.ConstrainedCopy(asciiBytes, 0, buf, 0, length);
			    } else {
                    Array.ConstrainedCopy(asciiBytes, 0, buf, 0, asciiBytes.Length);
			    }
			    return HexUtil.bytesToHexString(buf);
		    }
		    return str;
	    }
    }
}
