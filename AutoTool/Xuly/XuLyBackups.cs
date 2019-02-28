﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.Xuly
{
    public class XuLyBackups
    {
        public static string PATH_FOLDER = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DataBackup";
        private XuLyBackups()
        {
        }
        public static XuLyBackups getInstance { get { return Nested.instance; } }
        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly XuLyBackups instance = new XuLyBackups();
        }
        public async Task GetDataJson(string url, string param, Action<string> acton)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url + param))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();

                // ... Display the result.
                if (result != null &&
                    result.Length >= 50)
                {
                    acton.Invoke(result);
                }
            }
        }
        public async Task GetDataJsonPost(string url, string param, Action<string> acton)
        {
            var myContent = JsonConvert.SerializeObject(param);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.PostAsync(url, byteContent))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                if (result != null &&
                    result.Length >= 50)
                {
                    acton.Invoke(result);
                }
            }
        }
        public void TaoFile(string fileName, Action<string> acion)
        {
            try
            {
                string backupFolder = PATH_FOLDER + "\\" + fileName;
                if (!Directory.Exists(PATH_FOLDER))
                {
                    Directory.CreateDirectory(PATH_FOLDER);
                }
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }
                //callback
                acion.Invoke(backupFolder);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
