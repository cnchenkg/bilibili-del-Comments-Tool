using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace B站视频历史评论删除工具
{
    /// <summary>
    ///  静态类，获取md5值。
    /// </summary>
    internal  class GetMd5
    {
        /// <summary>
        ///  两个key相加后得到的。
        /// </summary>
      static  string r = "ea1db124af3c7062474693fa704f4ff8";
        /// <summary>
        ///  根据传入的视频oid获取评论内容。
        /// </summary>
        /// <param name="oid">视频oid</param>
        /// <param name="webLocation">网页地址。</param>
        /// <param name="now">当前时间戳</param>
        /// <param name="R">两key值相加后结果。</param>
        /// <returns>返回值加密后的md5值。</returns>
        public static string HeadResultGetMd5(string oid, string webLocation)
        {
          
            string now= DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            // 未写w_rid;,编码字符串。
            string md5code = HttpUtility.UrlEncode($"mode=3&oid={oid}&pagination_str=%7B%22offset%22%3A%22%22%7D&plat=1&seek_rpid=&type=1&web_location={webLocation}&wts={now}{r}");
            // 解码转字符。
            md5code = HttpUtility.UrlDecode(md5code);
            // 将字符转为char类型。
            //  char[] chars = md5code.ToCharArray();
            int[] md5codeToArray = new int[md5code.Length];
            // 将
            for (int i = 0; i < md5code.Length; i++)
            {
                // 7.3不支持无符号右移，将值取为绝对值。
                md5codeToArray[i] = Math.Abs(md5code[i] & 255);
            }
            // z定义md5数组长度乘8后的值，参与后续的运算。
            int z = md5codeToArray.Length * 8;
            // 定义四个参与运算的long值。
            long Y = 1732584193, Q = -271733879, J = -1732584194, D = 271733878;
            // 右移数组，存放右移后的数据。
            // 定义右移数组，长度为最后一次赋值的位运算，即将z值赋值给46这个下标的位置。
            uint[] leftMoveNumber = new uint[(z + 64 >> 9 << 4) + 16];
            // 为右移数组进行赋值。
            for (int i = 0, j = 0; i < md5codeToArray.Length; i++, j += 8)
            {
                // j>>右移5位进行跳转下一个数组。
                // 进行算数或运算，二进制（两二进制数对比，都是1落1,1|0落1，）
                #region 官方例子。
                //    uint a = 0b_1010_0000;
                //    uint b = 0b_1001_0001;
                //    uint c = a | b;
                //    Console.WriteLine(Convert.ToString(c, toBase: 2));
                //Output:
                //    10110001
                #endregion
                leftMoveNumber[j >> 5] |= (uint)(md5codeToArray[i] << 24 - j % 32);
            }
            // 将右移数组进行变换。
            for (int i = 0; i < leftMoveNumber.Length; i++)
            {
                // 右移数组或运算后与上常量，在与右移数组相反值进行运算，在与上一个常量。
                #region  官方例子。
                //uint a = 0b_1111_1000;
                //uint b = 0b_1001_1101;
                //uint c = a & b;
                //Console.WriteLine(Convert.ToString(c, toBase: 2));
                //// Output:
                //// 10011000
                #endregion
                leftMoveNumber[i] = (((leftMoveNumber[i] << 8 | leftMoveNumber[i] >> 24) & 16711935) | ((leftMoveNumber[i] << 24 | leftMoveNumber[i] >> 8) & 4278255360));
            }
            // 添加一个新的值。
            // 将z*右移5位后得出的下标值参与进后面的128的或运算。
            leftMoveNumber[z >> 5] = (uint)(leftMoveNumber[z >> 5] | 128 << z % 32);
            // 将z值添加进数组
            leftMoveNumber[(z + 64 >> 9 << 4) + 14] = (uint)z;
            #region yqjd加密。
            for (int i = 0; i < leftMoveNumber.Length; i += 16)
            {
                long ie = Y, ue = Q, pe = J, de = D;
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 0], 7, -680876936);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 1], 12, -389564586);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 2], 17, 606105819);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 3], 22, -1044525330);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 4], 7, -176418897);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 5], 12, 1200080426);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 6], 17, -1473231341);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 7], 22, -45705983);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 8], 7, 1770035416);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 9], 12, -1958414417);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 10], 17, -42063);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 11], 22, -1990404162);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 12], 7, 1804603682);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 13], 12, -40341101);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 14], 17, -1502002290);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 15], 22, 1236535329);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 1], 5, -165796510);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 6], 9, -1069501632);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 11], 14, 643717713);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 0], 20, -373897302);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 5], 5, -701558691);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 10], 9, 38016083);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 15], 14, -660478335);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 4], 20, -405537848);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 9], 5, 568446438);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 14], 9, -1019803690);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 3], 14, -187363961);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 8], 20, 1163531501);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 13], 5, -1444681467);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 2], 9, -51403784);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 7], 14, 1735328473);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 12], 20, -1926607734);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 5], 4, -378558);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 8], 11, -2022574463);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 11], 16, 1839030562);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 14], 23, -35309556);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 1], 4, -1530992060);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 4], 11, 1272893353);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 7], 16, -155497632);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 10], 23, -1094730640);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 13], 4, 681279174);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 0], 11, -358537222);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 3], 16, -722521979);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 6], 23, 76029189);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 9], 4, -640364487);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 12], 11, -421815835);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 15], 16, 530742520);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 2], 23, -995338651);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 0], 6, -198630844);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 7], 10, 1126891415);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 14], 15, -1416354905);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 5], 21, -57434055);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 12], 6, 1700485571);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 3], 10, -1894986606);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 10], 15, -1051523);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 1], 21, -2054922799);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 8], 6, 1873313359);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 15], 10, -30611744);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 6], 15, -1560198380);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 13], 21, 1309151649);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 4], 6, -145523070);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 11], 10, -1120210379);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 2], 15, 718787259);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 9], 21, -343485551);
                Y = (uint)(Y + ie);
                Q = (uint)(Q + ue);
                J = (uint)(J + pe);
                D = (uint)(D + de);
            }
            #endregion
            // 定义数组将计算后的yqjd值存入
            long[] longs = new long[] { Y, Q, J, D };
            // 变换供下一步骤使用。
            for (int i = 0; i < longs.Length; i++)
            {
                longs[i] = (int)((longs[i] << 8 | ((uint)longs[i]) >> 32 - 8) & 16711935 | (longs[i] << 24 | ((uint)longs[i]) >> 32 - 24) & 4278255360);
            }
            // 存放变换后的字节。
            List<uint> bytes = new List<uint>();
            // 将yqjd变换为字节。
            for (int i = 0; i < longs.Length * 32; i += 8)
            {
                // 变为字节。
                bytes.Add(((uint)(longs[i >> 5])) >> 24 - i % 32 & 255);
            }

            // 计算wrid值
            StringBuilder stringBuilder = new StringBuilder();
            foreach (uint item in bytes)
            {
                // 字节转为字符串。
                stringBuilder.Append((item >> 4).ToString("x"));
                stringBuilder.Append((item & 15).ToString("x"));

            }
            if (stringBuilder != null)
            {// 写入W-rid。
                return $"https://api.bilibili.com/x/v2/reply/wbi/main?oid={oid}&type=1&mode=3&pagination_str=%7B%22offset%22:%22%22%7D&plat=1&seek_rpid=&web_location={webLocation}&w_rid={stringBuilder.ToString()}&wts={now}";
            }

            return md5code;
        }


        public static string NewResultGetMd5(string oid, string webLocation)
        {

            string now = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            // 未写w_rid;,编码字符串。
            string md5code = HttpUtility.UrlEncode($"mode=2&oid={oid}&pagination_str=%7B%22offset%22%3A%22%22%7D&plat=1&seek_rpid=&type=1&web_location={webLocation}&wts={now}{r}");
            // 解码转字符。
            md5code = HttpUtility.UrlDecode(md5code);
            // 将字符转为char类型。
            //  char[] chars = md5code.ToCharArray();
            int[] md5codeToArray = new int[md5code.Length];
            // 将
            for (int i = 0; i < md5code.Length; i++)
            {
                // 7.3不支持无符号右移，将值取为绝对值。
                md5codeToArray[i] = Math.Abs(md5code[i] & 255);
            }
            // z定义md5数组长度乘8后的值，参与后续的运算。
            int z = md5codeToArray.Length * 8;
            // 定义四个参与运算的long值。
            long Y = 1732584193, Q = -271733879, J = -1732584194, D = 271733878;
            // 右移数组，存放右移后的数据。
            // 定义右移数组，长度为最后一次赋值的位运算，即将z值赋值给46这个下标的位置。
            uint[] leftMoveNumber = new uint[(z + 64 >> 9 << 4) + 16];
            // 为右移数组进行赋值。
            for (int i = 0, j = 0; i < md5codeToArray.Length; i++, j += 8)
            {
                // j>>右移5位进行跳转下一个数组。
                // 进行算数或运算，二进制（两二进制数对比，都是1落1,1|0落1，）
                #region 官方例子。
                //    uint a = 0b_1010_0000;
                //    uint b = 0b_1001_0001;
                //    uint c = a | b;
                //    Console.WriteLine(Convert.ToString(c, toBase: 2));
                //Output:
                //    10110001
                #endregion
                leftMoveNumber[j >> 5] |= (uint)(md5codeToArray[i] << 24 - j % 32);
            }
            // 将右移数组进行变换。
            for (int i = 0; i < leftMoveNumber.Length; i++)
            {
                // 右移数组或运算后与上常量，在与右移数组相反值进行运算，在与上一个常量。
                #region  官方例子。
                //uint a = 0b_1111_1000;
                //uint b = 0b_1001_1101;
                //uint c = a & b;
                //Console.WriteLine(Convert.ToString(c, toBase: 2));
                //// Output:
                //// 10011000
                #endregion
                leftMoveNumber[i] = (((leftMoveNumber[i] << 8 | leftMoveNumber[i] >> 24) & 16711935) | ((leftMoveNumber[i] << 24 | leftMoveNumber[i] >> 8) & 4278255360));
            }
            // 添加一个新的值。
            // 将z*右移5位后得出的下标值参与进后面的128的或运算。
            leftMoveNumber[z >> 5] = (uint)(leftMoveNumber[z >> 5] | 128 << z % 32);
            // 将z值添加进数组
            leftMoveNumber[(z + 64 >> 9 << 4) + 14] = (uint)z;
            #region yqjd加密。
            for (int i = 0; i < leftMoveNumber.Length; i += 16)
            {
                long ie = Y, ue = Q, pe = J, de = D;
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 0], 7, -680876936);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 1], 12, -389564586);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 2], 17, 606105819);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 3], 22, -1044525330);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 4], 7, -176418897);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 5], 12, 1200080426);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 6], 17, -1473231341);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 7], 22, -45705983);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 8], 7, 1770035416);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 9], 12, -1958414417);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 10], 17, -42063);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 11], 22, -1990404162);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 12], 7, 1804603682);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 13], 12, -40341101);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 14], 17, -1502002290);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 15], 22, 1236535329);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 1], 5, -165796510);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 6], 9, -1069501632);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 11], 14, 643717713);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 0], 20, -373897302);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 5], 5, -701558691);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 10], 9, 38016083);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 15], 14, -660478335);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 4], 20, -405537848);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 9], 5, 568446438);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 14], 9, -1019803690);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 3], 14, -187363961);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 8], 20, 1163531501);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 13], 5, -1444681467);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 2], 9, -51403784);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 7], 14, 1735328473);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 12], 20, -1926607734);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 5], 4, -378558);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 8], 11, -2022574463);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 11], 16, 1839030562);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 14], 23, -35309556);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 1], 4, -1530992060);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 4], 11, 1272893353);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 7], 16, -155497632);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 10], 23, -1094730640);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 13], 4, 681279174);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 0], 11, -358537222);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 3], 16, -722521979);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 6], 23, 76029189);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 9], 4, -640364487);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 12], 11, -421815835);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 15], 16, 530742520);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 2], 23, -995338651);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 0], 6, -198630844);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 7], 10, 1126891415);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 14], 15, -1416354905);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 5], 21, -57434055);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 12], 6, 1700485571);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 3], 10, -1894986606);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 10], 15, -1051523);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 1], 21, -2054922799);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 8], 6, 1873313359);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 15], 10, -30611744);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 6], 15, -1560198380);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 13], 21, 1309151649);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 4], 6, -145523070);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 11], 10, -1120210379);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 2], 15, 718787259);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 9], 21, -343485551);
                Y = (uint)(Y + ie);
                Q = (uint)(Q + ue);
                J = (uint)(J + pe);
                D = (uint)(D + de);
            }
            #endregion
            // 定义数组将计算后的yqjd值存入
            long[] longs = new long[] { Y, Q, J, D };
            // 变换供下一步骤使用。
            for (int i = 0; i < longs.Length; i++)
            {
                longs[i] = (int)((longs[i] << 8 | ((uint)longs[i]) >> 32 - 8) & 16711935 | (longs[i] << 24 | ((uint)longs[i]) >> 32 - 24) & 4278255360);
            }
            // 存放变换后的字节。
            List<uint> bytes = new List<uint>();
            // 将yqjd变换为字节。
            for (int i = 0; i < longs.Length * 32; i += 8)
            {
                // 变为字节。
                bytes.Add(((uint)(longs[i >> 5])) >> 24 - i % 32 & 255);
            }

            // 计算wrid值
            StringBuilder stringBuilder = new StringBuilder();
            foreach (uint item in bytes)
            {
                // 字节转为字符串。
                stringBuilder.Append((item >> 4).ToString("x"));
                stringBuilder.Append((item & 15).ToString("x"));

            }
            if (stringBuilder != null)
            {// 写入W-rid。
                return $"https://api.bilibili.com/x/v2/reply/wbi/main?oid={oid}&type=1&mode=2&pagination_str=%7B%22offset%22:%22%22%7D&plat=1&seek_rpid=&web_location={webLocation}&w_rid={stringBuilder}&wts={now}";
            }

            return md5code;
        }


        public static string AllResultGetMd5(string oid, string webLocation,string next)
        {

            string now = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
           // next = Regex.Replace(next, "[!'()*]","");
            // 未写w_rid;,编码字符串。
            // string md5code = HttpUtility.UrlEncode($"mode=2&oid={oid}&pagination_str={next}&plat=1&type=1&web_location={webLocation}&wts={now}{r}");
            string md5code = HttpUtility.UrlEncode($"mode=2&oid={oid}&pagination_str=%7B%22offset%22%3A%22%7B%5C%22type%5C%22%3A3%2C%5C%22direction%5C%22%3A1%2C%5C%22Data%5C%22%3A%7B%5C%22cursor%5C%22%3A{next}%7D%7D%22%7D&plat=1&type=1&web_location={webLocation}&wts={now}{r}");
            // 解码转字符。
            md5code = HttpUtility.UrlDecode(md5code);
            // 将字符转为char类型。
            //  char[] chars = md5code.ToCharArray();
            int[] md5codeToArray = new int[md5code.Length];
            // 将
            for (int i = 0; i < md5code.Length; i++)
            {
                // 7.3不支持无符号右移，将值取为绝对值。
                md5codeToArray[i] = Math.Abs(md5code[i] & 255);
            }
            // z定义md5数组长度乘8后的值，参与后续的运算。
            int z = md5codeToArray.Length * 8;
            // 定义四个参与运算的long值。
            long Y = 1732584193, Q = -271733879, J = -1732584194, D = 271733878;
            // 右移数组，存放右移后的数据。
            // 定义右移数组，长度为最后一次赋值的位运算，即将z值赋值给46这个下标的位置。
            uint[] leftMoveNumber = new uint[(z + 64 >> 9 << 4) + 16];
            // 为右移数组进行赋值。
            for (int i = 0, j = 0; i < md5codeToArray.Length; i++, j += 8)
            {
                // j>>右移5位进行跳转下一个数组。
                // 进行算数或运算，二进制（两二进制数对比，都是1落1,1|0落1，）
                #region 官方例子。
                //    uint a = 0b_1010_0000;
                //    uint b = 0b_1001_0001;
                //    uint c = a | b;
                //    Console.WriteLine(Convert.ToString(c, toBase: 2));
                //Output:
                //    10110001
                #endregion
                leftMoveNumber[j >> 5] |= (uint)(md5codeToArray[i] << 24 - j % 32);
            }
            // 将右移数组进行变换。
            for (int i = 0; i < leftMoveNumber.Length; i++)
            {
                // 右移数组或运算后与上常量，在与右移数组相反值进行运算，在与上一个常量。
                #region  官方例子。
                //uint a = 0b_1111_1000;
                //uint b = 0b_1001_1101;
                //uint c = a & b;
                //Console.WriteLine(Convert.ToString(c, toBase: 2));
                //// Output:
                //// 10011000
                #endregion
                leftMoveNumber[i] = (((leftMoveNumber[i] << 8 | leftMoveNumber[i] >> 24) & 16711935) | ((leftMoveNumber[i] << 24 | leftMoveNumber[i] >> 8) & 4278255360));
            }
            // 添加一个新的值。
            // 将z*右移5位后得出的下标值参与进后面的128的或运算。
            leftMoveNumber[z >> 5] = (uint)(leftMoveNumber[z >> 5] | 128 << z % 32);
            // 将z值添加进数组
            leftMoveNumber[(z + 64 >> 9 << 4) + 14] = (uint)z;
            #region yqjd加密。
            for (int i = 0; i < leftMoveNumber.Length; i += 16)
            {
                long ie = Y, ue = Q, pe = J, de = D;
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 0], 7, -680876936);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 1], 12, -389564586);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 2], 17, 606105819);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 3], 22, -1044525330);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 4], 7, -176418897);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 5], 12, 1200080426);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 6], 17, -1473231341);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 7], 22, -45705983);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 8], 7, 1770035416);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 9], 12, -1958414417);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 10], 17, -42063);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 11], 22, -1990404162);
                Y = R_ff(Y, Q, J, D, leftMoveNumber[i + 12], 7, 1804603682);
                D = R_ff(D, Y, Q, J, leftMoveNumber[i + 13], 12, -40341101);
                J = R_ff(J, D, Y, Q, leftMoveNumber[i + 14], 17, -1502002290);
                Q = R_ff(Q, J, D, Y, leftMoveNumber[i + 15], 22, 1236535329);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 1], 5, -165796510);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 6], 9, -1069501632);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 11], 14, 643717713);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 0], 20, -373897302);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 5], 5, -701558691);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 10], 9, 38016083);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 15], 14, -660478335);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 4], 20, -405537848);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 9], 5, 568446438);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 14], 9, -1019803690);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 3], 14, -187363961);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 8], 20, 1163531501);
                Y = R_gg(Y, Q, J, D, leftMoveNumber[i + 13], 5, -1444681467);
                D = R_gg(D, Y, Q, J, leftMoveNumber[i + 2], 9, -51403784);
                J = R_gg(J, D, Y, Q, leftMoveNumber[i + 7], 14, 1735328473);
                Q = R_gg(Q, J, D, Y, leftMoveNumber[i + 12], 20, -1926607734);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 5], 4, -378558);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 8], 11, -2022574463);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 11], 16, 1839030562);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 14], 23, -35309556);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 1], 4, -1530992060);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 4], 11, 1272893353);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 7], 16, -155497632);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 10], 23, -1094730640);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 13], 4, 681279174);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 0], 11, -358537222);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 3], 16, -722521979);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 6], 23, 76029189);
                Y = R_hh(Y, Q, J, D, leftMoveNumber[i + 9], 4, -640364487);
                D = R_hh(D, Y, Q, J, leftMoveNumber[i + 12], 11, -421815835);
                J = R_hh(J, D, Y, Q, leftMoveNumber[i + 15], 16, 530742520);
                Q = R_hh(Q, J, D, Y, leftMoveNumber[i + 2], 23, -995338651);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 0], 6, -198630844);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 7], 10, 1126891415);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 14], 15, -1416354905);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 5], 21, -57434055);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 12], 6, 1700485571);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 3], 10, -1894986606);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 10], 15, -1051523);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 1], 21, -2054922799);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 8], 6, 1873313359);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 15], 10, -30611744);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 6], 15, -1560198380);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 13], 21, 1309151649);
                Y = R_ii(Y, Q, J, D, leftMoveNumber[i + 4], 6, -145523070);
                D = R_ii(D, Y, Q, J, leftMoveNumber[i + 11], 10, -1120210379);
                J = R_ii(J, D, Y, Q, leftMoveNumber[i + 2], 15, 718787259);
                Q = R_ii(Q, J, D, Y, leftMoveNumber[i + 9], 21, -343485551);
                Y = (uint)(Y + ie);
                Q = (uint)(Q + ue);
                J = (uint)(J + pe);
                D = (uint)(D + de);
            }
            #endregion
            // 定义数组将计算后的yqjd值存入
            long[] longs = new long[] { Y, Q, J, D };
            // 变换供下一步骤使用。
            for (int i = 0; i < longs.Length; i++)
            {
                longs[i] = (int)((longs[i] << 8 | ((uint)longs[i]) >> 32 - 8) & 16711935 | (longs[i] << 24 | ((uint)longs[i]) >> 32 - 24) & 4278255360);
            }
            // 存放变换后的字节。
            List<uint> bytes = new List<uint>();
            // 将yqjd变换为字节。
            for (int i = 0; i < longs.Length * 32; i += 8)
            {
                // 变为字节。
                bytes.Add(((uint)(longs[i >> 5])) >> 24 - i % 32 & 255);
            }

            // 计算wrid值
            StringBuilder stringBuilder = new StringBuilder();
            foreach (uint item in bytes)
            {
                // 字节转为字符串。
                stringBuilder.Append((item >> 4).ToString("x"));
                stringBuilder.Append((item & 15).ToString("x"));

            }
            if (stringBuilder != null)
            {// 写入W-rid。
                return $"https://api.bilibili.com/x/v2/reply/wbi/main?oid={oid}&type=1&mode=2&pagination_str=%7B%22offset%22%3A%22%7B%5C%22type%5C%22%3A3%2C%5C%22direction%5C%22%3A1%2C%5C%22Data%5C%22%3A%7B%5C%22cursor%5C%22%3A{next}%7D%7D%22%7D&plat=1&web_location={webLocation}&w_rid={stringBuilder.ToString()}&wts={now}";
            }

            return md5code;
        }
        #region R值计算。
        /// <summary>
        ///  将值转换为加密。
        /// </summary>
        /// <param name="Z"></param>
        /// <param name="W"></param>
        /// <param name="F"></param>
        /// <param name="z"></param>
        /// <param name="Y"></param>
        /// <param name="Q"></param>
        /// <param name="J"></param>
        /// <returns></returns>
        static Int64 R_ff(long Z, long W, long F, long z, long Y, int Q, int J)
        {

            int resultRff = (int)(Z + (W & F | ~W & z) + Y + J);
            return (resultRff << Q | ((uint)resultRff) >> (32 - Q)) + W;

        }
        /// <summary>
        ///  gg加密运算。
        /// </summary>
        /// <param name="Z"></param>
        /// <param name="W"></param>
        /// <param name="F"></param>
        /// <param name="z"></param>
        /// <param name="Y"></param>
        /// <param name="Q"></param>
        /// <param name="J"></param>
        /// <returns></returns>
        static Int64 R_gg(Int64 Z, Int64 W, Int64 F, Int64 z, uint Y, int Q, Int32 J)
        {

            int resultRff = (int)(Z + (W & z | F & ~z) + Y + J);
            return (resultRff << Q | ((uint)resultRff) >> (32 - Q)) + W;

        }
        /// <summary>
        ///  hh加密运算。
        /// </summary>
        /// <param name="Z"></param>
        /// <param name="W"></param>
        /// <param name="F"></param>
        /// <param name="z"></param>
        /// <param name="Y"></param>
        /// <param name="Q"></param>
        /// <param name="J"></param>
        /// <returns></returns>
        static Int64 R_hh(Int64 Z, Int64 W, Int64 F, Int64 z, uint Y, int Q, Int32 J)
        {

            int resultRff = (int)(Z + (W ^ F ^ z) + Y + J);
            return (resultRff << Q | ((uint)resultRff) >> (32 - Q)) + W;
        }
        /// <summary>
        ///  ii加密运算。
        /// </summary>
        /// <param name="Z"></param>
        /// <param name="W"></param>
        /// <param name="F"></param>
        /// <param name="z"></param>
        /// <param name="Y"></param>
        /// <param name="Q"></param>
        /// <param name="J"></param>
        /// <returns></returns>
        static Int64 R_ii(Int64 Z, Int64 W, Int64 F, Int64 z, uint Y, int Q, Int32 J)
        {
            int resultRff = (int)(Z + (F ^ (W | ~z)) + Y + J);

            return (resultRff << Q | ((uint)resultRff) >> (32 - Q)) + W;
        }
        #endregion
    }
}
