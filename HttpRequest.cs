using System;
using System.IO;
using System.Net;
using System.Text;

namespace B站视频历史评论删除工具
{
    /// <summary>
    ///  网络请求。
    /// </summary>
    internal class HttpRequest : IDisposable
    {

        /// <summary>
        ///  post方法。
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="content">内容</param>
        /// <param name="cookie">cookie</param>
        /// <returns>返回结果。</returns>
        public static string Post(string url, string content, string cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (cookie != null)
            {
                request.Headers.Add("Cookie", cookie);
            }
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            using (Stream streamWriter = request.GetRequestStream())
            {
                streamWriter.Write(bytes, 0, bytes.Length);
                streamWriter.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string result = reader.ReadToEnd();
                reader.Close();
                
                return result;
            }
        }

        private readonly static object _ooc = new object();
        private bool disposedValue;

        /// <summary>
        ///  get方法。
        /// </summary>
        /// <param name="url">网址。</param>
        /// <param name="cookie">cookie</param>
        /// <returns>返回结果。</returns>
        public static string Get(string url, string cookie)
        {

            lock (_ooc)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                if (cookie != null)
                {
                    request.Headers.Add("cookie", cookie);
                }
                request.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8";
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }
        /// <summary>
        ///  构造函数。
        /// </summary>
        public HttpRequest()
        {

        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~HttpRequest()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
