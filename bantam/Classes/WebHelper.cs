﻿using System;
using System.Text;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Windows.Forms;

namespace bantam_php
{
    class WebHelper
    {

        /// <summary>
        /// 
        /// </summary>
        public static string g_GlobalDefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0";

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<int, string> commonUseragents = new Dictionary<int, string>() {
            {0, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36"},
            {1, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36" },
            {2, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0" },
            {3, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36" },
            {4, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0" },
            {5, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134" },
            {6, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36" },
            {7, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36"},
            {8, "Mozilla/5.0 (Linux; Android 7.0; SM-G892A Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/60.0.3112.107 Mobile Safari/537.36" },
            {9, "Mozilla/5.0 (Linux; Android 6.0.1; SM-G935S Build/MMB29K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Mobile Safari/537.36" },
            {10, "Opera/9.80 (Windows NT 5.1; U; en) Presto/2.10.289 Version/12.01" },
            {11, "Mozilla/5.0 (iPhone; U; CPU like Mac OS X; en) AppleWebKit/420+ (KHTML, like Gecko) Version/3.0 Mobile/1A543a Safari/419.3" },
            {12, "Mozilla/5.0 (PlayStation 4 1.70) AppleWebKit/536.26 (KHTML, like Gecko)" }
        };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static TKey randomDicionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TKey> keyList = new List<TKey>(dict.Keys);

            return keyList[rand.Next(keyList.Count)];
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        public static HttpClient client = new HttpClient(new HttpClientHandler { UseCookies = false });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyUrl"></param>
        /// <param name="proxyPort"></param>
        private static void AddProxy(string proxyUrl, string proxyPort)
        {
            client.CancelPendingRequests();
            client.Dispose();
            client = new HttpClient(new HttpClientHandler() {
                UseProxy = true,
                UseCookies = false,
                Proxy = new WebProxy("http://localhost:31337", false)
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<string> WebRequest(string url, string code)
        {
            string requestArgsName = BantamMain.Hosts[url].RequestArgName;
            bool sendViaCookie = BantamMain.Hosts[url].SendDataViaCookie;

           // try {
                HttpMethod method;
                if (sendViaCookie) {
                    method = HttpMethod.Get;
                } else {
                    method = HttpMethod.Post;
                }

                var request = new HttpRequestMessage(method, url);

                if (!string.IsNullOrEmpty(code)) {
                    string minifiedCode = PhpHelper.minifyCode(code);
                    string encodedCode = HttpUtility.UrlEncode(minifiedCode);
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encodedCode);
                    string b64 = System.Convert.ToBase64String(plainTextBytes);

                    if (sendViaCookie) {
                        request.Headers.TryAddWithoutValidation("Cookie", requestArgsName + "=" + b64);
                    } else {
                        var values = new Dictionary<string, string>
                        {
                           { requestArgsName, b64 }
                        };

                        var content = new FormUrlEncodedContent(values);
                        request.Content = content;
                    }
                }

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
          //  } catch (System.Net.Http.HttpRequestException e) {
          //      MessageBox.Show(e.ToString());
          //  }
          //  return "";
        }
    }
}
