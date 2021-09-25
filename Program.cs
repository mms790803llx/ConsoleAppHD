using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Timers;

namespace ConsoleAppHD
{
    class Program
    {
        
        static void Main(string[] args)
        {
            runAsync().Wait();

        }
        public static async System.Threading.Tasks.Task runAsync()
        {
            HttpClient httpClient = new HttpClient();
            Console.WriteLine("PageNumber(1):");
            var pgNum = Console.ReadLine();
            Console.WriteLine("showCount(100):");
            var showCount = Console.ReadLine();
            Console.WriteLine("appType(0):");
            var appType = Console.ReadLine();
            Console.WriteLine("StudentID(d43037df-2b8c-414d-8bda-aa7d056db93d):");
            var sID = Console.ReadLine();
            Console.WriteLine("fkSchoolID(3018):");
            var scID = Console.ReadLine();
            var content = new StringContent("currentPage=" + pgNum + "&" + "showCount=" + showCount + "&appType=" + appType + "&manufactor=HUAWEI&padModel=BZT-W09&studentId=" + sID + "&fkSchoolId=" + scID);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var tsk = httpClient.PostAsync("http://www.istudyway.com.cn/ht/marketremote/listRemote.action", content);
            Console.WriteLine("Posted.");
            tsk.Wait();
            Console.WriteLine("Finished.");
            var msg = tsk.Result;
            try
            {
                msg.EnsureSuccessStatusCode();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message+"\nError.Type any key to exit.");
                Console.ReadKey();
                System.Environment.Exit(0);
            }
            string manifest =new UTF8Encoding().GetString(await msg.Content.ReadAsByteArrayAsync());
            Console.WriteLine(manifest);
            Root jsonRoot = JsonConvert.DeserializeObject<Root>(manifest);
            Console.Clear();
            Console.Error.WriteLine("Parse Result:");
            foreach(List l in jsonRoot.list)
            {
                Console.Error.WriteLine("Name:");
                Console.WriteLine(l.applicationLable+"("+l.packageName+")");
                Console.Error.WriteLine("Version:");
                Console.WriteLine(l.versionName+"("+l.versionCode+")");
                Console.Error.WriteLine("Download Num:");
                Console.WriteLine(l.downloadNum);
                Console.Error.WriteLine("Url:");
                Console.WriteLine(l.downloadUrl);
                Console.Error.WriteLine("Press any key to see next,or type d to download.");
                if (Console.ReadKey().KeyChar.Equals('d'))
                {
                    
                    try
                    {
                        download(l.downloadUrl, l.applicationLable + "(" + l.versionName + "(" + l.versionCode + ")" + ")" + ".apk");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Download Error:\n");
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Press any key to see next.");
                        Console.ReadKey();
                    }
                }
                Console.Clear();
                Console.WriteLine("Parse Result:");
            }
        }
        public static void download(string url,string name)
        {
            Console.Clear();
            Console.WriteLine("URL:\n" + url);
            
            Console.WriteLine("Type File Name(none for default name:"+name+"):");
            var fname = Console.ReadLine();
            if (fname == "") fname = name;
            FileStream fs = File.Create(fname);
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            //Stream stream = new FileStream(tempFile, FileMode.Create);
            Console.WriteLine("Download started.");
            Console.WriteLine("Total Size:"+((decimal)response.ContentLength)/1024/1024+"MB");
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            long loop = 0;

            while (size > 0)
            {
                //stream.Write(bArr, 0, size);
                fs.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
                if (loop%128 == 0)
                {
                    Console.WriteLine("Downloaded:" + loop*1024/8*100 / response.ContentLength+"%"+",Size:"+((decimal)loop)/1024/8+"MB");
                }
                loop++;
            }
            //stream.Close();
            responseStream.Close();
            Console.WriteLine("Finished.");
            Console.WriteLine("File path:"+fs.Name);
            fs.Close();
            Console.WriteLine("Press any key to continue.");
            Console.Clear();
        }
        public class ApplicationIcons
        {
        }

        public class List
        {
            /// <summary>
            /// 
            /// </summary>
            public int apkId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string versionCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string versionName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string packageName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> usesPermissions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sdkVersion { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string targetSdkVersion { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string applicationLable { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string serializeName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ApplicationIcons applicationIcons { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string applicationIcon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string uploadDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int downloadNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int accumulateDownloadNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string apkSize { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> impliedFeatures { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> features { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int fkSchoolId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string marketKey { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int appType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string downloadUrl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string apkIconUrl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mark { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isHide { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isPush { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isForceUpdate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int allSchool { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public int showCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int totalPage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int totalResult { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int currentPage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int currentResult { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<List> list { get; set; }
        }

    }
}
