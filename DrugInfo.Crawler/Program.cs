using Common;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrugInfo.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //查看第N页药品
            var driver1 = new PhantomJSDriver(GetPhantomJSDriverService());
            var db = new Model1();
            var frompage = int.Parse(System.Configuration.ConfigurationManager.AppSettings["fromPage"]);
            Pager page = new Pager { Currentpage = frompage };
            do
            {
                driver1.Navigate().GoToUrl(GetUrl(page.Currentpage + 1));

                Thread.Sleep(3000);

                if (driver1.Title == "403 Forbidden")
                {
                    Thread.Sleep(1000 * 60 * 10);
                    continue;
                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(driver1.PageSource);
                //共 18702条&nbsp;&nbsp;&nbsp;&nbsp;第 1页/共1247页
                var pageNodeText = doc.DocumentNode.SelectSingleNode(@"//tr[@height='70']/td[@width='200']").InnerText;
                page = PageParser.MedicalListParse(pageNodeText);
                Console.WriteLine("当前页：" + page.Currentpage + ",共" + page.TotalPage + "页");
                foreach (var item in MedicalListDataParser.MedicalListParse(doc))
                {
                    db.Productions.Add(new Production { ProductionName = item, LSST = DateTime.Now, FromPage = page.Currentpage });
                }
                db.SaveChanges();

            } while (page.Currentpage < page.TotalPage);

            Console.ReadKey();
        }

        private static string GetUrl(int page = 1, string optionType = "V1")
        {
            return string.Format("http://app2.sfda.gov.cn/datasearchp/gzcxSearch.do?page={0}&searchcx=&optionType={1}&paramter0=null&paramter1=null&paramter2=null&formRender=cx", page, optionType);
        }

        private static PhantomJSDriverService GetPhantomJSDriverService()
        {
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            //Proxy proxy = new Proxy();
            //proxy.HttpProxy = string.Format("36.41.143.57:8118");
            //service.ProxyType = "http";
            //service.Proxy = proxy.HttpProxy;
            return service;
        }
    }
}
