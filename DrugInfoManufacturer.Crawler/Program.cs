using DrugInfo.Crawler;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DrugInfoManufacturer.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {

            //查看第N页药品
            var driver1 = new PhantomJSDriver(GetPhantomJSDriverService());

            Pager page = new Pager { Currentpage = 0 };
            do
            {
                driver1.Navigate().GoToUrl(GetUrl(page.Currentpage + 1));

                Thread.Sleep(2000);

                if (driver1.Title == "403 Forbidden")
                {
                    Thread.Sleep(1000 * 60 * 10);
                    continue;
                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(driver1.PageSource);
                //共 18702条&nbsp;&nbsp;&nbsp;&nbsp;第 1页/共1247页
                var pageNodeText = doc.DocumentNode.SelectSingleNode(@"//tr[@height=70]/td[@width='30%' and @style='padding-left:30px']").InnerText;
                page = PageParser.MedicalListParse(pageNodeText);
                Console.WriteLine("当前页：" + page.Currentpage + ",共" + page.TotalPage + "页");

                foreach (var item in ManufacturerParser.Parse(doc))
                {

                }

            } while (page.Currentpage < page.TotalPage);

            Console.ReadKey();
        }

        private static string GetUrl(int page = 1, string name = "葡萄糖氯化钠注射液")
        {
            return string.Format("http://app2.sfda.gov.cn/datasearchp/all.do?page={0}&name={1}&tableName=TABLE25&formRender=cx&searchcx=&paramter0=&paramter1=&paramter2=", page, HttpUtility.UrlEncode(name));
        }

        private static PhantomJSDriverService GetPhantomJSDriverService()
        {
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            //Proxy proxy = new Proxy();
            //proxy.HttpProxy = string.Format("127.0.0.1:9999");
            //service.ProxyType = "http";
            //service.Proxy = proxy.HttpProxy;
            return service;
        }
    }
}
